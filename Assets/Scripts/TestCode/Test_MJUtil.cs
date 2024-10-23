using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test_MJUtil : MonoBehaviour
{
    //�V�����e�����v�Z�p�ϐ�
    [SerializeField] private string shanten_man;
    [SerializeField] private string shanten_pin;
    [SerializeField] private string shanten_sou;
    [SerializeField] private string shanten_honor;
    [SerializeField] private List<Meld> shanten_melds;
    [SerializeField] private int shanten_value;
    [SerializeField] private List<Tile> shanten_effectiveTiles;
    private string latestStr = "";

    [SerializeField] List<Tile> doraList;
    public bool is_tsumo = false;
    public bool is_riichi = false;
    public bool is_ippatsu = false;
    public bool is_rinshan = false;
    public bool is_chankan = false;
    public bool is_haitei = false;
    public bool is_houtei = false;
    public bool is_daburu_riichi = false;
    public bool is_nagashi_mangan = false;
    public bool is_tenhou = false;
    public bool is_renhou = false;
    public bool is_chiihou = false;
    public bool is_open_riichi = false;
    [SerializeField] private int cost_main;
    [SerializeField] private int cost_adtional;
    private HandResponse handResponse;

    //���e�X�g�p�ϐ�
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
        Debug.Log("--------------�����֘A�̋@�\�̃e�X�g�J�n--------------");
        Debug.Log("json����Ԃ���136�z��ւ̕ϊ�");
        Convert136Test();
        Debug.Log("�V�����e�����̌v�Z");
        GetShanten();
    }

    private void Update()
    {
        //�V�����e�����e�X�g
        //�l���ύX���ꂽ�Ƃ��������s����
        if (shanten_man + shanten_pin + shanten_sou + shanten_honor != latestStr)
        {
            Tiles tiles = new(shanten_man, shanten_pin, shanten_sou, shanten_honor, shanten_melds);
            shanten_value = mahjongUtils.GetShanten(tiles);
            shanten_effectiveTiles = mahjongUtils.GetEffectiveTiles(tiles);

            HandConfig config= new HandConfig();
            config.is_tsumo = is_tsumo;
            config.is_riichi = is_riichi;
            config.is_ippatsu = is_ippatsu;
            config.is_chankan = is_chankan;
            config.is_daburu_riichi= is_daburu_riichi;
            config.is_rinshan = is_rinshan;
            config.is_haitei = is_haitei;
            config.is_houtei = is_houtei;
            config.is_nagashi_mangan = is_nagashi_mangan;
            config.is_tenhou = is_tenhou;
            config.is_chiihou = is_chiihou;
            config.is_renhou = is_renhou;
            config.is_open_riichi=is_open_riichi;

            if (tiles.TilesList.Count == 14)
            {
                handResponse = mahjongUtils.EstimateHandValue(tiles, tiles.TilesList[0], doraList, config);
                cost_main = handResponse.cost_main;
                cost_adtional = handResponse.cost_aditional;
            }
        }
        latestStr = shanten_man + shanten_pin + shanten_sou + shanten_honor;


        //���e�X�g
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
        Debug.Log("�V�����e�� : " + shanten);

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
