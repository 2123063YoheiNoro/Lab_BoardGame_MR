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
        //受信周りの設定をする.
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
        //「class」という変数名が使えないため「class_name」に置換する.
        string fixedText = text.Replace("class", "class_name");
        //上の変換で「class_id」が「class_name_id」に変わってしまうので修正する.
        fixedText = fixedText.Replace("class_name_id", "class_id");

        //オブジェクトに変換.
        predictions = JsonUtility.FromJson<Predictions>(fixedText);
        Debug.Log(fixedText);
    }

}
