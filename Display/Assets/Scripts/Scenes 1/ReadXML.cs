using System.IO;
using System.Xml;
using UnityEngine;

public class ReadXML : MonoBehaviour
{
    private XmlDocument xmlDoc;
    private XmlNodeList node;
    private string xmlPath;

    private void Awake()
    {
        //��xml.xml��xml���ļ�������ַ��streamingAssetsPath/xml.xml��
        xmlPath = Path.Combine(Application.streamingAssetsPath, "xml.xml");
    }

    void Start()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlPath);

        //����body��ʼ
        node = xmlDoc.SelectSingleNode("body").ChildNodes;
        foreach (XmlElement xl in node)
        {
            //��XML��ȡ��
            switch (xl.Name)
            {
                case "touchIP":
                    UdpConfig.touchIP = xl.InnerText;
                    break;
                case "touchPort":
                    UdpConfig.touchPort = int.Parse(xl.InnerText);
                    break;
                case "displayPort":
                    UdpConfig.displayPort = int.Parse(xl.InnerText);
                    break;
            }
        }
    }
}

