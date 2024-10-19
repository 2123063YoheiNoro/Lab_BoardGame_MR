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
        Debug.Log("--------------�����֘A�̋@�\�̃e�X�g�J�n--------------");
        Debug.Log("json����Ԃ���136�z��ւ̕ϊ�");
        Convert136Test();
        Debug.Log("�V�����e�����̌v�Z");
        GetShanten();
    }

    private void Update()
    {
        //�l���ύX���ꂽ�Ƃ��������s����
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
        //�������IPrediction�̌`�ɕϊ�����
        Mock_TileData data = new Mock_TileData(tileJson);
        //IPrediction��136�z��ɕϊ�����
        var result = mahjongUtils.ConvertPredictionsTo136Array(data);
    }

    private void GetShanten()
    {
        string tileJson = Addressables.LoadAssetAsync<TextAsset>("MockTile1").WaitForCompletion().ToString();
        //�������IPrediction�̌`�ɕϊ�����
        Mock_TileData data = new Mock_TileData(tileJson);
        //IPrediction��136�z��ɕϊ�����
        var result = mahjongUtils.ConvertPredictionsTo136Array(data);
        int shanten = mahjongUtils.GetShanten_from136Array(result);
        Debug.Log("�V�����e�� : "+shanten);

        Tiles testTiles1 = new Tiles("2345", "2255589", "45");
        shanten = mahjongUtils.GetShanten_from136Array(mahjongUtils.ConvertPredictionsTo136Array(testTiles1));
        Debug.Log("�V�����e�� : " + shanten);
        Tiles testTiles2 = new Tiles("5", "12234573", "36", "46");
        shanten = mahjongUtils.GetShanten_from136Array(mahjongUtils.ConvertPredictionsTo136Array(testTiles2));
        Debug.Log("�V�����e�� : " + shanten);
        Tiles testTiles3 = new Tiles("19", "19", "19", "1234567");
        shanten = mahjongUtils.GetShanten_from136Array(mahjongUtils.ConvertPredictionsTo136Array(testTiles3));
        Debug.Log("�V�����e�� : " + shanten);
    }

    private int GetShantenFromTiles(Tiles tiles)
    {
        return mahjongUtils.GetShanten_from136Array(mahjongUtils.ConvertPredictionsTo136Array(tiles));
    }
}
