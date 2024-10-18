using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test_MJUtil : MonoBehaviour
{
    MahjongUtils mahjongUtils;
    void Start()
    {
        mahjongUtils = new MahjongUtils();
        Debug.Log("--------------�����֘A�̋@�\�̃e�X�g�J�n--------------");
        Convert136Test();
        GetShanten();
    }

    private void Convert136Test()
    {
        string tileJson = Addressables.LoadAssetAsync<TextAsset>("MockTile1").WaitForCompletion().ToString();
        //�������IPrediction�̌`�ɕϊ�����
        Mock_TileData data = new Mock_TileData(tileJson);
        //IPrediction��136�z��ɕϊ�����
        int[] result = mahjongUtils.ConvertPredictionsTo136Array(data);
    }

    private void GetShanten()
    {
        string tileJson = Addressables.LoadAssetAsync<TextAsset>("MockTile1").WaitForCompletion().ToString();
        //�������IPrediction�̌`�ɕϊ�����
        Mock_TileData data = new Mock_TileData(tileJson);
        //IPrediction��136�z��ɕϊ�����
        int[] result = mahjongUtils.ConvertPredictionsTo136Array(data);
        int shanten = mahjongUtils.GetShanten_from136Array(result);
        Debug.Log("�V�����e�� : "+shanten);
    }

}
