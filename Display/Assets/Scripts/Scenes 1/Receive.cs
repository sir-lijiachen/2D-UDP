using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Receive : MonoBehaviour
{
    //��Ҫ��main�����м���ContentPage��������������UdpDisplay.contentPage = contentPage��
   // public ContentPage contentPage;

    public void ReceiveData(string data)
    {
        Debug.Log(data);
        //����primaryBtnName,xxxxxx;secondaryButtonValue,2
        //��ȡһ����ť����������ť��Ӧ��
        //��Header("��һ��if�������޸�")��
        if (data.StartsWith("primaryBtnName,")) 
        {
            string[] parts = data.Split(';');
            {
                string primaryBtnName = parts[0].Split(',')[1];
                int secondaryButtonValue = int.Parse(parts[1].Split(',')[1]);

                //contentPage.ReceiveButton(primaryBtnName, secondaryButtonValue);//��ֵ
            }
        }
        // ���һҳ
        else if (data.StartsWith("LastPage"))
        {
            //contentPage.ReceiveUpdatePage(-1); //��ֵ
        }
        // ��ǰ��һҳ
        else if (data.StartsWith("NextPage"))
        {
            //contentPage.ReceiveUpdatePage(1); //��ֵ
        }
        //����
        else if (data.StartsWith("Standby"))
        {
            //contentPage.StandbyPage(true);//��֪Ҫ����
        }
    }
}
