# 2D-通信
主要UDP连接触摸屏和显示屏，但也含有串口通信

## Scenes 1 --拖放
**功能：** 拖动图片，如果松开图片时，不在中心范围，这移动到初始位置，如果在移动范围，则进行动画。  
**配置：** Loom.cs  
### Touch
需要在StreamingAssets配置xml文件，用来获取IP地址。

- UdpConfig是一个自定义的类
- ReadXML是获取XML文件中的IP地址
- UdpTouch是udp通信，发送 SocketSend 方法，接收为 SocketReceive 方法
- Receive是通过 UdpTouch 中的 SocketReceive 方法接收数据，然后进行整理处理
- Main1是整体把控

**XML格式**
```C#
<body>
	<touchPort>4001</touchPort>
	<displayIP>127.0.0.1</displayIP>
	<displayPort>5001</displayPort>
</body>
```
**读取xml：** `UdpConfig.XXXX`
### Display
需要在StreamingAssets配置xml文件，用来获取IP地址。

- UdpConfig是一个自定义的类
- ReadXML是获取XML文件中的IP地址
- UdpDisPlay是udp通信，发送 SocketSend 方法，接收为 SocketReceive 方法
- Receive是通过 UdpTouch 中的 SocketReceive 方法接收数据，然后进行整理处理
- Main1是整体把控

**XML格式**
```C#
<body>
	<touchIP>127.0.0.1</touchIP>
	<touchPort>4001</touchPort>
	<displayPort>5001</displayPort>
</body>
```
**读取xml：** `UdpConfig.XXXX`


## Scenes 2 串口通信
**功能：** 点击按钮，让特定的灯亮起来
**配置：** Loom.cs  
### Touch

先在 Unity 编辑器中，去 `Edit -> Project Settings -> Player -> Other Settings` 中将`Scripting Runtime Version`设置为`.NET Framework`，这样`using System.IO.Ports`就启用了。LightUp是预先固定了指令，可以改为读取Json来获取指令。

- SerialCommunication是串口通信，发送为SendHexData方法。其中设置了串口名称、波特率、奇偶校验、数据位、停止位。
- LightUp是发送指令让灯亮起来。定时发送指令，让灯一个一个亮起来，在灭掉。

**定时器推荐使用**
```C#
    // 启动定时器
    void StartTimer()
    {
        // 创建新的定时器，设置时间间隔为 readTime 毫秒
        timer = new System.Timers.Timer(readTime);

        // 绑定定时器的 Elapsed 事件，触发时调用 onTimerHandler 方法
        timer.Elapsed += new ElapsedEventHandler(onTimerHandler);

        // 启动定时器
        timer.Start();
    }

    //销毁上面计时器
    public void OnDestroy()
    {
        serialCommunication.SendHexData(lightAllOffCommand);
        // 确保在对象销毁时停止定时器
        if (timer != null)
        {
            timer.Stop();
        }
    }

    // 重新启动定时器
    public void RestartTimer()
    {
        currentIndex = 0;
        // 重新启动定时器，先停止再开始
        if (timer != null)
        {
            timer.Stop();  // 停止当前定时器
            timer.Start(); // 重新启动定时器
        }
    }
```
