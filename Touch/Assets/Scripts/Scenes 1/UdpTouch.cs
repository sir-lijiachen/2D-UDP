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

    private string serverIPAddress;//本机ip
    //需要从main函数中加载Receive；；；；；；【UdpDisplay.receiveData=Receive】
    public Receive receiveData;

    void Start()
    {
        //获取本机ip
        serverIPAddress = GetLocalIP();
        InitSocket(); 
    }


    //初始化
    void InitSocket()
    {
        //【Header("UdpConfig.displayPort、UdpConfig.touchIP、UdpConfig.touchPort")】

        //IPAddress.Parse 方法是用来将一个 IP 地址的字符串表示形式转换为 IPAddress 类
        //IPAddress.Parse(clientIPAddress)  自身的字符串转换为IPAddress ，UdpConfig.clientPort是读取xml存放在静态的UdpConfig里的clientPort
        ipEnd = new IPEndPoint(IPAddress.Parse(serverIPAddress), UdpConfig.touchPort);                                                                                
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipEnd);
        //IPAddress.ParseUdpConfig.serverIP)  UdpConfig.serverIP字符串转换为IPAddress ，UdpConfig.serverPort是读取xml存放在静态的UdpConfig里的serverPort
        IPEndPoint sender = new IPEndPoint(IPAddress.Parse(UdpConfig.displayIP), UdpConfig.displayPort);
        clientEnd = (EndPoint)sender;
        Debug.Log(clientEnd);
        print("waiting for UDP dgram");

        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    //服务器发送
    public void SocketSend(string sendStr)
    {

        sendData = new byte[1024];
        sendData = Encoding.UTF8.GetBytes(sendStr);
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, clientEnd);
    }

    /// <summary>
    /// 服务器接收
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

                //通过Loom将数据传给Receive来解析
                Loom.QueueOnMainThread((param) =>
                {
                    receiveData.ReceiveData(recvStr);
                }, null);
            }
            catch
            {
                Debug.Log("消息未发送，检查对方是否开启UDP连接");
            }
        }
    }

    //连接关闭
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
    /// 获取自身IP
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

