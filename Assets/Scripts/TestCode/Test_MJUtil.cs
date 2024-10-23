using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test_MJUtil : MonoBehaviour
{
    //シャンテン数計算用変数
    [SerializeField] private string shanten_man;
    [SerializeField] private string shanten_pin;
    [SerializeField] private string shanten_sou;
    [SerializeField] private string shanten_honor;
    [SerializeField] private List<Meld> shanten_melds;
    [SerializeField] private int shanten_value;
    [SerializeField] private List<Tile> shanten_effectiveTiles;

    [SerializeField] private string dora_man;
    [SerializeField] private string dora_pin;
    [SerializeField] private string dora_sou;
    [SerializeField] private string dora_honor;
    [SerializeField] private int cost_main;
    [SerializeField] private int cost_additional;
    [SerializeField] private HandConfig hand_config;
    private string latestStr = "";

    //鳴きテスト用変数
    [SerializeField] private Meld.MeldType meldType = Meld.MeldType.CHI;
    [SerializeField] private string meld_man;
    [SerializeField] private string meld_pin;
    [SerializeField] private string meld_sou;
    [SerializeField] private string meld_honor;
    [SerializeField] private bool meld_isValid;
    private string meld_latestStr = "";

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
        //シャンテン数テスト
        //値が変更されたときだけ実行する
        if (shanten_man + shanten_pin + shanten_sou + shanten_honor != latestStr)
        {
            Tiles tiles = new(shanten_man, shanten_pin, shanten_sou, shanten_honor, shanten_melds);
            shanten_value = mahjongUtils.GetShanten(tiles);
            shanten_effectiveTiles = mahjongUtils.GetEffectiveTiles(tiles);

            /*
            //点数計算テスト
            HandResponse handResponse = mahjongUtils.EstimateHandValue(tiles, tiles.TilesList[0], null, hand_config);
            cost_main=handResponse.cost_main;
            cost_additional = handResponse.cost_aditional;
            */
        }
        latestStr = shanten_man + shanten_pin + shanten_sou + shanten_honor;


        //鳴きテスト
        if (meld_man + meld_pin + meld_sou + meld_honor != meld_latestStr)
        {
            Meld meld = new Meld(meldType, meld_man, meld_pin, meld_sou, meld_honor);
            meld_isValid = meld.IsValid();
        }
        meld_latestStr = meld_man + meld_pin + meld_sou + meld_honor;
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
        Debug.Log("シャンテン : " + shanten);

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
