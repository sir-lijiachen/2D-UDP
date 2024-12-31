using System;
using System.IO.Ports;
using System.Text;
using UnityEngine;

public class SerialCommunication : MonoBehaviour
{
    private SerialPort serialPort;

    // 设置串口配置
    private string portName = "COM3";    // 串口名称
    private int baudRate = 9600;         // 波特率
    private Parity parity = Parity.None; // 奇偶校验
    private int dataBits = 8;            // 数据位
    private StopBits stopBits = StopBits.One; // 停止位

    // 数据接收缓冲区
    private string receivedData = string.Empty;

    void Start()
    {
        // 初始化串口
        serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        serialPort.ReadTimeout = 500;  // 设置读取超时时间
        serialPort.WriteTimeout = 500; // 设置写入超时时间

        // 打开串口连接
        try
        {
            serialPort.Open();
            Debug.Log("串口已连接！");
        }
        catch (Exception e)
        {
            Debug.LogError("串口连接失败: " + e.Message);
        }
    }

    void Update()
    {
        // 检查串口是否已经打开
        if (serialPort.IsOpen)
        {
            // 尝试读取串口数据
            try
            {
                if (serialPort.BytesToRead > 0)
                {
                    byte[] buffer = new byte[serialPort.BytesToRead];
                    serialPort.Read(buffer, 0, buffer.Length);
                    receivedData = BitConverter.ToString(buffer).Replace("-", " ");
                    Debug.Log("接收到数据: " + receivedData);
                }
            }
            catch (TimeoutException)
            {
                // 超时时不做任何事情
            }
        }
    }

    // 发送16进制数据
    public void SendHexData(string hexData)
    {
        try
        {
            byte[] dataToSend = HexStringToByteArray(hexData);
            serialPort.Write(dataToSend, 0, dataToSend.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("发送数据失败: " + e.Message);
        }
    }

    // 将16进制字符串转换为字节数组
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

    // 关闭串口连接
    private void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("串口已关闭！");
        }
    }
}
