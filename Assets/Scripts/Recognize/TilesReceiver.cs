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
    public Predictions predictions;
    public Subject<Predictions> subject = new();
    //�v�̔F�����ʂ̃��A�N�e�B�u�v���p�e�B�@�v�̍��W�Ƃ��������擾�������ꍇ�Ɏg��
    public ReactiveProperty<Predictions> rpPredictions;
    //��v�̃��A�N�e�B�u�v���p�e�B�@�v�̎�ނ̂ݕK�v�ȏꍇ�Ɏg���@���ɂ͖��Ή�
    public ReactiveProperty<Tiles> rpTiles;


    void Start()
    {
        //��M����̐ݒ������.
        int LOCALPORT = 21230;
        udp = new UdpClient(LOCALPORT);
        udp.BeginReceive(OnReceived, udp);

        subject
            .ObserveOnMainThread()
            .Subscribe(p =>
            {
                predictions = p;
                rpPredictions.Value = p;
                rpTiles.Value = new Tiles(p);
            })
            .AddTo(this);

    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint iPEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref iPEnd);
        string jsonText = Encoding.UTF8.GetString(getByte);
        //�I�u�W�F�N�g�ɕϊ�.
        Predictions _predictions = Convert_Json_To_Predictions(jsonText);
        subject.OnNext(_predictions);

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private Predictions Convert_Json_To_Predictions(string jsonData)
    {
        //�uclass�v�Ƃ����ϐ������g���Ȃ����߁uclass_name�v�ɒu������.
        string fixedText = jsonData.Replace("class", "class_name");
        //��̕ϊ��Łuclass_id�v���uclass_name_id�v�ɕς���Ă��܂��̂ŏC������.
        fixedText = fixedText.Replace("class_name_id", "class_id");

        //�I�u�W�F�N�g�ɕϊ�.
        Predictions predictions = JsonUtility.FromJson<Predictions>(fixedText);
        return predictions;
    }

    private void OnDestroy()
    {
        udp.Close();
    }

}
