using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test_MJUtil : MonoBehaviour
{
    [SerializeField] private string man;
    [SerializeField] private string pin;
    [SerializeField] private string sou;
    [SerializeField] private string honor;
    private string latestStr = "";

    [SerializeField] private int shanten;

    MahjongUtils mahjongUtils;
    void Start()
    {
        mahjongUtils = new MahjongUtils();
        Debug.Log("--------------麻雀関連の機能のテスト開始--------------");
        Debug.Log("jsonから赤あり136配列への変換");
        Convert136Test();
        Debug.Log("シャンテン数の計算");
        GetShanten();
    }

    private void Update()
    {
        //値が変更されたときだけ実行する
        if (man + pin + sou + honor != latestStr)
        {
            Tiles tiles = new(man, pin, sou, honor);
            shanten = GetShantenFromTiles(tiles);
        }
        latestStr = man + pin + sou + honor;
    }

    private void Convert136Test()
    {
        string tileJson = Addressables.LoadAssetAsync<TextAsset>("MockTile1").WaitForCompletion().ToString();
        //文字列をIPredictionの形に変換する
        Mock_TileData data = new Mock_TileData(tileJson);
        //IPredictionを136配列に変換する
        var result = mahjongUtils.ConvertPredictionsTo136Array(data);
    }

    private void GetShanten()
    {
        string tileJson = Addressables.LoadAssetAsync<TextAsset>("MockTile1").WaitForCompletion().ToString();
        //文字列をIPredictionの形に変換する
        Mock_TileData data = new Mock_TileData(tileJson);
        //IPredictionを136配列に変換する
        var result = mahjongUtils.ConvertPredictionsTo136Array(data);
        int shanten = mahjongUtils.GetShanten_from136Array(result);
        Debug.Log("シャンテン : "+shanten);

        Tiles testTiles1 = new Tiles("2345", "2255589", "45");
        shanten = mahjongUtils.GetShanten_from136Array(mahjongUtils.ConvertPredictionsTo136Array(testTiles1));
        Debug.Log("シャンテン : " + shanten);
        Tiles testTiles2 = new Tiles("5", "12234573", "36", "46");
        shanten = mahjongUtils.GetShanten_from136Array(mahjongUtils.ConvertPredictionsTo136Array(testTiles2));
        Debug.Log("シャンテン : " + shanten);
        Tiles testTiles3 = new Tiles("19", "19", "19", "1234567");
        shanten = mahjongUtils.GetShanten_from136Array(mahjongUtils.ConvertPredictionsTo136Array(testTiles3));
        Debug.Log("シャンテン : " + shanten);
    }

    private int GetShantenFromTiles(Tiles tiles)
    {
        return mahjongUtils.GetShanten_from136Array(mahjongUtils.ConvertPredictionsTo136Array(tiles));
    }
}
