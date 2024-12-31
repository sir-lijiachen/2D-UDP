using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public UdpDisplay udpDisplay;
    public Receive receive;
    private void Awake()
    {
        gameObject.AddComponent<ReadXML>();
        gameObject.AddComponent<Loom>();
        udpDisplay = gameObject.AddComponent<UdpDisplay>();

        receive = gameObject.AddComponent<Receive>();

        //¿‡œ‡Õ¨
        udpDisplay.receiveData = receive;
    }
}
