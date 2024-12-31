using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Receive : MonoBehaviour
{
    //需要从main函数中加载OptionPage；；；；；；【UdpDisplay.optionPage = optionPage】
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
            //获取二级页码，二级总页数
            //[Header("更具情况修改")]
            string[] parts = data.Split(';');
            {
                int secondaryPage = int.Parse(parts[0].Split(',')[1]);
                int secondaryPageCount = int.Parse(parts[1].Split(',')[1]);

                //optionPage.ReceiveSecondaryPage(secondaryPage, secondaryPageCount);
            }
        }
    }
}
