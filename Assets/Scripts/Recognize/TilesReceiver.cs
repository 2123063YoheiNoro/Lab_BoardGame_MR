using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;

public class TilesReceiver: MonoBehaviour
{

    static UdpClient udp;
    IPEndPoint remoteEP = null;
    [SerializeField] Text uiText;
    [SerializeField] public Predictions predictions;

    void Start()
    {
        int LOCALPORT = 21230;
        udp = new UdpClient(LOCALPORT);
        udp.Client.ReceiveTimeout = 2000;
    }

    // Update is called once per frame
    void Update()
    {
        IPEndPoint remoteEP = null;
        byte[] data = udp.Receive(ref remoteEP);
        string text = Encoding.UTF8.GetString(data);
        //�uclass�v�Ƃ����ϐ������g���Ȃ����߁uclass_name�v�ɒu������.
        string fixedText = text.Replace("class", "class_name");
        //�I�u�W�F�N�g�ɕϊ�.
        predictions = JsonUtility.FromJson<Predictions>(fixedText);
    }

}
