using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;
using UniRx;

public class TilesReceiver: MonoBehaviour
{

    static UdpClient udp;
    IPEndPoint remoteEP = null;
    [SerializeField] public Predictions predictions;

    void Start()
    {
        //��M����̐ݒ������.
        int LOCALPORT = 21230;
        udp = new UdpClient(LOCALPORT);
        udp.Client.ReceiveTimeout = 2000;

        var rpPredictions = new ReactiveCollection<Prediction>();
    }

    void Update()
    {
        IPEndPoint remoteEP = null;
        byte[] data = udp.Receive(ref remoteEP);
        string text = Encoding.UTF8.GetString(data);
        //�uclass�v�Ƃ����ϐ������g���Ȃ����߁uclass_name�v�ɒu������.
        string fixedText = text.Replace("class", "class_name");
        //��̕ϊ��Łuclass_id�v���uclass_name_id�v�ɕς���Ă��܂��̂ŏC������.
        fixedText = fixedText.Replace("class_name_id", "class_id");

        //�I�u�W�F�N�g�ɕϊ�.
        predictions = JsonUtility.FromJson<Predictions>(fixedText);
        Debug.Log(fixedText);
    }

}
