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
        Debug.Log("--------------麻雀関連の機能のテスト開始--------------");
        Convert136Test();
        GetShanten();
    }

    private void Convert136Test()
    {
        string tileJson = Addressables.LoadAssetAsync<TextAsset>("MockTile1").WaitForCompletion().ToString();
        //文字列をIPredictionの形に変換する
        Mock_TileData data = new Mock_TileData(tileJson);
        //IPredictionを136配列に変換する
        int[] result = mahjongUtils.ConvertPredictionsTo136Array(data);
    }

    private void GetShanten()
    {
        string tileJson = Addressables.LoadAssetAsync<TextAsset>("MockTile1").WaitForCompletion().ToString();
        //文字列をIPredictionの形に変換する
        Mock_TileData data = new Mock_TileData(tileJson);
        //IPredictionを136配列に変換する
        int[] result = mahjongUtils.ConvertPredictionsTo136Array(data);
        int shanten = mahjongUtils.GetShanten_from136Array(result);
        Debug.Log("シャンテン : "+shanten);
    }

}
