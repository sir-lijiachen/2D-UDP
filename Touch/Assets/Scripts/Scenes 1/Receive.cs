using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Receive : MonoBehaviour
{
    //��Ҫ��main�����м���OptionPage��������������UdpDisplay.optionPage = optionPage��
    //public OptionPage optionPage;

    public void ReceiveData(string data)
    {
        Debug.Log(data);

        if (data.StartsWith("All Animations Completed")|| data.StartsWith("Reconnection"))
        {
            //optionPage.InteractiveSwitch(true);
        }
        else if (data.StartsWith("secondaryPage,"))
        {
            //secondaryPage,xx;secondaryPageCount,xx
            //��ȡ����ҳ�룬������ҳ��
            //[Header("��������޸�")]
            string[] parts = data.Split(';');
            {
                int secondaryPage = int.Parse(parts[0].Split(',')[1]);
                int secondaryPageCount = int.Parse(parts[1].Split(',')[1]);

                //optionPage.ReceiveSecondaryPage(secondaryPage, secondaryPageCount);
            }
        }
    }
}
