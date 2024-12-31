using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Receive : MonoBehaviour
{
    //需要从main函数中加载ContentPage；；；；；；【UdpDisplay.contentPage = contentPage】
   // public ContentPage contentPage;

    public void ReceiveData(string data)
    {
        Debug.Log(data);
        //解析primaryBtnName,xxxxxx;secondaryButtonValue,2
        //获取一级按钮名，二级按钮对应数
        //【Header("第一个if可能有修改")】
        if (data.StartsWith("primaryBtnName,")) 
        {
            string[] parts = data.Split(';');
            {
                string primaryBtnName = parts[0].Split(',')[1];
                int secondaryButtonValue = int.Parse(parts[1].Split(',')[1]);

                //contentPage.ReceiveButton(primaryBtnName, secondaryButtonValue);//传值
            }
        }
        // 向后翻一页
        else if (data.StartsWith("LastPage"))
        {
            //contentPage.ReceiveUpdatePage(-1); //传值
        }
        // 向前翻一页
        else if (data.StartsWith("NextPage"))
        {
            //contentPage.ReceiveUpdatePage(1); //传值
        }
        //待机
        else if (data.StartsWith("Standby"))
        {
            //contentPage.StandbyPage(true);//告知要待机
        }
    }
}
