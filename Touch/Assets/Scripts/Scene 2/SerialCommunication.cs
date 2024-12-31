using System;
using System.IO.Ports;
using System.Text;
using UnityEngine;

public class SerialCommunication : MonoBehaviour
{
    private SerialPort serialPort;

    // ���ô�������
    private string portName = "COM3";    // ��������
    private int baudRate = 9600;         // ������
    private Parity parity = Parity.None; // ��żУ��
    private int dataBits = 8;            // ����λ
    private StopBits stopBits = StopBits.One; // ֹͣλ

    // ���ݽ��ջ�����
    private string receivedData = string.Empty;

    void Start()
    {
        // ��ʼ������
        serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        serialPort.ReadTimeout = 500;  // ���ö�ȡ��ʱʱ��
        serialPort.WriteTimeout = 500; // ����д�볬ʱʱ��

        // �򿪴�������
        try
        {
            serialPort.Open();
            Debug.Log("���������ӣ�");
        }
        catch (Exception e)
        {
            Debug.LogError("��������ʧ��: " + e.Message);
        }
    }

    void Update()
    {
        // ��鴮���Ƿ��Ѿ���
        if (serialPort.IsOpen)
        {
            // ���Զ�ȡ��������
            try
            {
                if (serialPort.BytesToRead > 0)
                {
                    byte[] buffer = new byte[serialPort.BytesToRead];
                    serialPort.Read(buffer, 0, buffer.Length);
                    receivedData = BitConverter.ToString(buffer).Replace("-", " ");
                    Debug.Log("���յ�����: " + receivedData);
                }
            }
            catch (TimeoutException)
            {
                // ��ʱʱ�����κ�����
            }
        }
    }

    // ����16��������
    public void SendHexData(string hexData)
    {
        try
        {
            byte[] dataToSend = HexStringToByteArray(hexData);
            serialPort.Write(dataToSend, 0, dataToSend.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("��������ʧ��: " + e.Message);
        }
    }

    // ��16�����ַ���ת��Ϊ�ֽ�����
    private byte[] HexStringToByteArray(string hex)
    {
        int length = hex.Length;
        byte[] byteArray = new byte[length / 2];
        for (int i = 0; i < length; i += 2)
        {
            byteArray[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return byteArray;
    }

    // �رմ�������
    private void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("�����ѹرգ�");
        }
    }
}
