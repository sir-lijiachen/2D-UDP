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

    // 35���صƵ�16����ָ�ʹ�����ṩ��ָ�
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

    private bool isRunning = true;  // ����ѭ���Ƿ�����
    private bool isLightsOn = true; // ���ڱ���Ƿ������


    private string lightAllOnCommand = "0105002FFF00BDF3";    // �������еƵ�ָ��
    private string lightAllOffCommand= "0105002F0000FC03";    // �ر����еƵ�ָ��

    private int readTime=3000;
    int i = 0;

    // ��ʱ����ÿ3�봥��һ��
    private System.Timers.Timer timer;
    private int currentIndex = 0;  // ��ǰָ������
    void Start()
    {
        // ��ʼѭ���������ȴ����еƣ�Ȼ��ر����е�
        //StartCoroutine(ControlLights());

        // ��ʼ����ʱ��������ʱ����Ϊ3000����(��3��)
        timer = new System.Timers.Timer(readTime);
        // �󶨶�ʱ���� Elapsed �¼�������ʱ���� onTimerHandler ����
        timer.Elapsed += new ElapsedEventHandler(onTimerHandler);
        StartTimer();
    }

    void onTimerHandler(object source, System.Timers.ElapsedEventArgs args)
    {
        Loom.QueueOnMainThread((param) =>
        {

            // ȷ�� currentIndex �����鷶Χ��
            if (currentIndex < lightOnCommands.Length)
            {
                // ���͵�ǰָ��
                string command = lightOnCommands[currentIndex];
                serialCommunication.SendHexData(command);

                Debug.Log($"Sending command{currentIndex}: " + command);
                // ���µ�ǰָ������
                currentIndex++;

                // ����Ѿ�����������ָ�������������
                if (currentIndex >= lightOnCommands.Length)
                {
                    serialCommunication.SendHexData(lightAllOffCommand);
                    currentIndex = 0; // ���¿�ʼѭ������
                }
            }
        }, null);
    }

    // ������ʱ��
    void StartTimer()
    {
        // �����µĶ�ʱ��������ʱ����Ϊ readTime ����
        timer = new System.Timers.Timer(readTime);

        // �󶨶�ʱ���� Elapsed �¼�������ʱ���� onTimerHandler ����
        timer.Elapsed += new ElapsedEventHandler(onTimerHandler);

        // ������ʱ��
        timer.Start();
    }

    //���������ʱ��
    public void OnDestroy()
    {
        serialCommunication.SendHexData(lightAllOffCommand);
        // ȷ���ڶ�������ʱֹͣ��ʱ��
        if (timer != null)
        {
            timer.Stop();
        }
    }

    // ����������ʱ��
    public void RestartTimer()
    {
        currentIndex = 0;
        // ����������ʱ������ֹͣ�ٿ�ʼ
        if (timer != null)
        {
            timer.Stop();  // ֹͣ��ǰ��ʱ��
            timer.Start(); // ����������ʱ��
        }
    }


    // ���Ƶ���������
    public void TurnOnSingleLight(int lightIndex)
    {
        if (lightIndex >= 0 && lightIndex < lightOnCommands.Length)
        {
            // ����ָ���Ŀ���ָ��
            serialCommunication.SendHexData(lightOnCommands[lightIndex]);
            Debug.Log($"{lightIndex}:{lightOnCommands[lightIndex]}");
        }
        else
        {
            Debug.LogWarning("Invalid light index.");
        }
    }

    // ���Ƶ�����Ϩ��
    public void TurnOffSingleLight(int lightIndex)
    {
        if (lightIndex >= 0 && lightIndex < lightOffCommands.Length)
        {
            // ����ָ���Ĺص�ָ��
            serialCommunication.SendHexData(lightOffCommands[lightIndex]);
        }
        else
        {
            Debug.LogWarning("Invalid light index.");
        }
    }

}
