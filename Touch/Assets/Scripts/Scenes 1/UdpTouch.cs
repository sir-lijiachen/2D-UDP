using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UdpTouch : MonoBehaviour
{
    private Socket socket; 
    private EndPoint clientEnd; 
    private IPEndPoint ipEnd; 
    private string recvStr; 
    private string sendStr; 
    private byte[] recvData = new byte[1024]; 
    private byte[] sendData = new byte[1024]; 
    private int recvLen; 
    private Thread connectThread; 

    private string serverIPAddress;//����ip
    //��Ҫ��main�����м���Receive��������������UdpDisplay.receiveData=Receive��
    public Receive receiveData;

    void Start()
    {
        //��ȡ����ip
        serverIPAddress = GetLocalIP();
        InitSocket(); 
    }


    //��ʼ��
    void InitSocket()
    {
        //��Header("UdpConfig.displayPort��UdpConfig.touchIP��UdpConfig.touchPort")��

        //IPAddress.Parse ������������һ�� IP ��ַ���ַ�����ʾ��ʽת��Ϊ IPAddress ��
        //IPAddress.Parse(clientIPAddress)  ������ַ���ת��ΪIPAddress ��UdpConfig.clientPort�Ƕ�ȡxml����ھ�̬��UdpConfig���clientPort
        ipEnd = new IPEndPoint(IPAddress.Parse(serverIPAddress), UdpConfig.touchPort);                                                                                
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipEnd);
        //IPAddress.ParseUdpConfig.serverIP)  UdpConfig.serverIP�ַ���ת��ΪIPAddress ��UdpConfig.serverPort�Ƕ�ȡxml����ھ�̬��UdpConfig���serverPort
        IPEndPoint sender = new IPEndPoint(IPAddress.Parse(UdpConfig.displayIP), UdpConfig.displayPort);
        clientEnd = (EndPoint)sender;
        Debug.Log(clientEnd);
        print("waiting for UDP dgram");

        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    //����������
    public void SocketSend(string sendStr)
    {

        sendData = new byte[1024];
        sendData = Encoding.UTF8.GetBytes(sendStr);
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, clientEnd);
    }

    /// <summary>
    /// ����������
    /// </summary>
    void SocketReceive()
    {
        while (true)
        {
            try
            {
                recvData = new byte[1024];
                recvLen = socket.ReceiveFrom(recvData, ref clientEnd);
                recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);

                //ͨ��Loom�����ݴ���Receive������
                Loom.QueueOnMainThread((param) =>
                {
                    receiveData.ReceiveData(recvStr);
                }, null);
            }
            catch
            {
                Debug.Log("��Ϣδ���ͣ����Է��Ƿ���UDP����");
            }
        }
    }

    //���ӹر�
    void SocketQuit()
    {
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        if (socket != null)
            socket.Close();
        print("disconnect");
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }

    /// <summary>
    /// ��ȡ����IP
    /// </summary>
    string GetLocalIP()
    {
        string ipAddress = "";
        foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
        {
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = address.ToString();
                break;
            }
        }
        return ipAddress;
    }
}

