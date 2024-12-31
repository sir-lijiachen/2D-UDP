using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Timers;

public class LightUp : MonoBehaviour
{
    public SerialCommunication serialCommunication;
    private string[] lightOnCommands = new string[]
    {
    "01050004FF00CDFB", "0105001BFF00FC3D", "01050008FF000DF8", "01050017FF003C3E", "01050019FF005DFD",
    "01050002FF002DFA", "0105000AFF00AC38", "01050011FF00DC3F", "01050015FF009DFE", "01050009FF005C38",
    "01050003FF007C3A", "01050014FF00CC3E", "0105000DFF001DF9", "0105001AFF00ADFD", "01050022FF002C30",
    "0105000FFF00BC39", "01050012FF002C3F", "0105001FFF00BDFC", "01050010FF008DFF", "01050013FF007DFF",
    "0105000CFF004C39", "0105001DFF001C3C", "01050016FF006DFE", "0105001EFF00EC3C", "0105000EFF00EDF9",
    "0105001CFF004DFC", "0105000BFF00FDF8", "01050000FF008C3A", "01050007FF003DFB", "01050006FF006C3B",
    "01050006FF006C3B", "01050005FF009C3B", "01050020FF008DF0","01050018FF000C3D","01050001FF00DDFA"
    };

    // 35个关灯的16进制指令（使用你提供的指令）
    private string[] lightOffCommands = new string[]
    {
    "0105000400008C0B", "0105001B0000BDCD", "0105000800004C08", "0105001700007DCE", "0105001900001C0D",
    "0105000200006C0A", "0105000A0000EDC8", "0105001100009DCF", "010500150000DC0E", "0105000900001DC8",
    "0105000300003DCA", "0105001400008DCE", "0105000D00005C09", "0105001A0000EC0D", "0105002200006DC0",
    "0105000F0000FDC9", "0105001200006DCF", "0105001F0000FC0C", "010500100000CC0F", "0105001300003C0F",
    "0105000C00000DC9", "0105001D00005DCC", "0105001600002C0E", "0105001E0000ADCC", "0105000E0000AC09",
    "0105001C00000C0C", "0105000B0000BC08", "010500000000CDCA", "0105000700007C0B", "0105000600002DCB",
    "0105000600002DCB", "010500050000DDCB", "010500200000CC00","0105001800004DCD","0105000100009C0A"
    };

    private bool isRunning = true;  // 控制循环是否运行
    private bool isLightsOn = true; // 用于标记是否灯亮着


    private string lightAllOnCommand = "0105002FFF00BDF3";    // 开启所有灯的指令
    private string lightAllOffCommand= "0105002F0000FC03";    // 关闭所有灯的指令

    private int readTime=3000;
    int i = 0;

    // 定时器，每3秒触发一次
    private System.Timers.Timer timer;
    private int currentIndex = 0;  // 当前指令索引
    void Start()
    {
        // 开始循环操作：先打开所有灯，然后关闭所有灯
        //StartCoroutine(ControlLights());

        // 初始化定时器，设置时间间隔为3000毫秒(即3秒)
        timer = new System.Timers.Timer(readTime);
        // 绑定定时器的 Elapsed 事件，触发时调用 onTimerHandler 方法
        timer.Elapsed += new ElapsedEventHandler(onTimerHandler);
        StartTimer();
    }

    void onTimerHandler(object source, System.Timers.ElapsedEventArgs args)
    {
        Loom.QueueOnMainThread((param) =>
        {

            // 确保 currentIndex 在数组范围内
            if (currentIndex < lightOnCommands.Length)
            {
                // 发送当前指令
                string command = lightOnCommands[currentIndex];
                serialCommunication.SendHexData(command);

                Debug.Log($"Sending command{currentIndex}: " + command);
                // 更新当前指令索引
                currentIndex++;

                // 如果已经发送完所有指令，可以重置索引
                if (currentIndex >= lightOnCommands.Length)
                {
                    serialCommunication.SendHexData(lightAllOffCommand);
                    currentIndex = 0; // 重新开始循环发送
                }
            }
        }, null);
    }

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


    // 控制单个灯亮起
    public void TurnOnSingleLight(int lightIndex)
    {
        if (lightIndex >= 0 && lightIndex < lightOnCommands.Length)
        {
            // 发送指定的开灯指令
            serialCommunication.SendHexData(lightOnCommands[lightIndex]);
            Debug.Log($"{lightIndex}:{lightOnCommands[lightIndex]}");
        }
        else
        {
            Debug.LogWarning("Invalid light index.");
        }
    }

    // 控制单个灯熄灭
    public void TurnOffSingleLight(int lightIndex)
    {
        if (lightIndex >= 0 && lightIndex < lightOffCommands.Length)
        {
            // 发送指定的关灯指令
            serialCommunication.SendHexData(lightOffCommands[lightIndex]);
        }
        else
        {
            Debug.LogWarning("Invalid light index.");
        }
    }

}
