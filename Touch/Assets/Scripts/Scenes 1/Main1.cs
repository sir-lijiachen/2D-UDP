using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main1 : MonoBehaviour
{
    public UdpTouch udpTouch;
    public Receive receive;

    private void Awake()
    {
        gameObject.AddComponent<ReadXML>();
        gameObject.AddComponent<Loom>();
        udpTouch = gameObject.AddComponent<UdpTouch>();

        receive = gameObject.AddComponent<Receive>();

        //¿‡œ‡ª•
        udpTouch.receiveData = receive;
    }

}
