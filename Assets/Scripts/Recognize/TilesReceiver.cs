using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UniRx;

public class TilesReceiver : MonoBehaviour
{

    static UdpClient udp;
    IPEndPoint remoteEP = null;
    public Predictions predictions;
    public Subject<Predictions> subject = new();
    public ReactiveProperty<Predictions> rpPredictions;


    void Start()
    {
        //受信周りの設定をする.
        int LOCALPORT = 21230;
        udp = new UdpClient(LOCALPORT);
        udp.BeginReceive(OnReceived, udp);

        subject
            .ObserveOnMainThread()
            .Subscribe(p =>
            {
                predictions = p;
                rpPredictions.Value = p;
            })
            .AddTo(this);

    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint iPEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref iPEnd);
        string jsonText = Encoding.UTF8.GetString(getByte);
        //オブジェクトに変換.
        Predictions _predictions = Convert_Json_To_Predictions(jsonText);
        subject.OnNext(_predictions);

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private Predictions Convert_Json_To_Predictions(string jsonData)
    {
        //「class」という変数名が使えないため「class_name」に置換する.
        string fixedText = jsonData.Replace("class", "class_name");
        //上の変換で「class_id」が「class_name_id」に変わってしまうので修正する.
        fixedText = fixedText.Replace("class_name_id", "class_id");

        //オブジェクトに変換.
        Predictions predictions = JsonUtility.FromJson<Predictions>(fixedText);
        return predictions;
    }

    private void OnDestroy()
    {
        udp.Close();
    }

}
