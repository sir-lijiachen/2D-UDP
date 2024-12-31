using System.Xml;
using UnityEngine;

public class ReadXML : MonoBehaviour
{
    private XmlDocument xmlDoc;
    private XmlNodeList node;
    private string xmlPath;

    private void Awake()
    {
        //xml.xml是xml的文件名，地址在streamingAssetsPath/xml.xml
        xmlPath = Application.streamingAssetsPath + "/xml.xml";
    }
    void Start()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlPath);

        //是以body开始
        node = xmlDoc.SelectSingleNode("body").ChildNodes;
        foreach (XmlElement xl in node)
        {
            switch (xl.Name)
            {
                case "touchPort":
                    UdpConfig.touchPort = int.Parse(xl.InnerText);
                    break;
                case "displayIP":
                    UdpConfig.displayIP = xl.InnerText;
                    break;
                case "displayPort":
                    UdpConfig.displayPort = int.Parse(xl.InnerText);
                    break;
                case "standby":
                    UdpConfig.standbyTime = int.Parse(xl.InnerText);
                    break;

            }
        }
    }

}
