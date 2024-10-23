using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using System.Linq;


public class MahjongUtils
{
    dynamic mj_tiles;
    dynamic mj_calculate;
    dynamic mj_shanten;
    public MahjongUtils()
    {
        mj_tiles = Py.Import("mahjong.tile");
        mj_calculate = Py.Import("mahjong.hand_calculating.hand");
        mj_shanten = Py.Import("mahjong.shanten");
    }


    /// <summary>
    /// �F�����ʂ�136�z��ɕϊ�����
    /// </summary>
    /// <param name="predictions"></param>
    /// <returns></returns>
    public dynamic ConvertPredictionsTo136Array(IPredictions predictions)
    {
        //�ݎq�A���q�A���q�A���v�̔ԍ������蓖�Ă镶����
        string man = "", sou = "", pin = "", honor = "";

        List<Prediction> predictionsList = predictions.GetAllPredictions();
        foreach (Prediction p in predictionsList)
        {
            //1�����ڂ�0~9�̐����A2�����ڂ͔v�̎��
            //m:�ݎq p:���q s:���q z:���v b:����
            //������0�͐�5�Ƃ��Ĉ����B0p�͐�5��
            char num = p.class_name[0];
            char group = p.class_name[1];

            //�v�̎�ނɑΉ�����string�^�ϐ��ɐ���������ǉ�����
            switch (group)
            {
                case 'm':
                    man += num.ToString();
                    break;
                case 'p':
                    pin += num.ToString();
                    break;
                case 's':
                    sou += num.ToString();
                    break;
                case 'z':
                    honor += num.ToString();
                    break;
                default:
                    break;
            }
        }

        //mahjong���C�u�����̊֐��ŕϊ�����
        var result = mj_tiles.TilesConverter.string_to_136_array(man, pin, sou, honor, true);
        return result;
    }

    public dynamic ConvertPredictionsTo136Array(Tiles tiles)
    {
        //�ݎq�A���q�A���q�A���v�̔ԍ������蓖�Ă镶����
        string man = "", sou = "", pin = "", honor = "";

        foreach (Tile t in tiles.TilesList)
        {
            switch (t.type)
            {
                case Tile.TileType.MAN:
                    man += t.Number.ToString();
                    break;
                case Tile.TileType.PIN:
                    pin += t.Number.ToString();
                    break;
                case Tile.TileType.SOU:
                    sou += t.Number.ToString();
                    break;
                case Tile.TileType.HONOR:
                    honor += t.Number.ToString();
                    break;
                default:
                    break;
            }
        }

        var result = mj_tiles.TilesConverter.string_to_136_array(man, pin, sou, honor, true);
        return result;

    }

    /// <summary>
    /// 136�z���34�z��ɕϊ�����
    /// </summary>
    /// <param name="tileArray_136"></param>
    /// <returns></returns>
    public dynamic To_34_array(dynamic tileArray_136)
    {
        var result = mj_tiles.TilesConverter.to_34_array(tileArray_136);
        return result;
    }

    /// <summary>
    /// 34�z�񂩂�V�����e�������v�Z����
    /// </summary>
    /// <param name="tileArray_34"></param>
    /// <returns></returns>
    public int GetShanten_from34Array(dynamic tileArray_34, bool use_chiitoitsu = true, bool use_kokushi = true)
    {
        int[] LengthCheckArray = tileArray_34;
        if (LengthCheckArray.Sum() > 14)
        {
            Debug.LogWarning("�v�̐����������܂�");
            return int.MaxValue;
        }
        var shantenCalculator = mj_shanten.Shanten();
        int shantenresult = shantenCalculator.calculate_shanten(tileArray_34, use_chiitoitsu, use_kokushi);
        return shantenresult;
    }

    /// <summary>
    /// 136�z�񂩂�V�����e�������v�Z����
    /// </summary>
    /// <param name="tileArray_136"></param>
    /// <returns></returns>
    public int GetShanten_from136Array(dynamic tileArray_136, bool use_chiitoitsu = true, bool use_kokushi = true)
    {
        //136�z���34�z��ɕϊ����ăV�����e���v�Z�֐��ɓ�����
        return GetShanten_from34Array(To_34_array(tileArray_136), use_chiitoitsu, use_kokushi);
    }

    /// <summary>
    /// Tiles�N���X����V�����e�������v�Z����
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public int GetShanten(Tiles tiles)
    {
        bool _use_chiitoitsu = true;
        bool _use_kokushi = true;

        //���I������ꍇ
        if (tiles.MeldsList.Count > 0)
        {
            //���Ύq�ƍ��m���o�͍l������O��
            _use_chiitoitsu = false;
            _use_kokushi = false;

            foreach (Meld m in tiles.MeldsList)
            {
                if (m.IsValid() == false)
                {
                    Debug.LogWarning("�s���ȕ��I���܂܂�Ă��܂�");
                    return int.MaxValue;
                }
            }
        }

        //�v�̐���14�𒴂���ꍇ�͒��f����
        //���� = �v�̐� + ���I�̐� * 3
        int tileTotalCount = tiles.TilesList.Count + tiles.MeldsList.Count * 3; //�Ō��*3�͕��I�ŎN�����ŏ��̔v�̐��@�����h���͍l�����Ă��Ȃ�
        if (tileTotalCount > 14)
        {
            Debug.LogWarning("�v�̐����������܂�");
            return int.MaxValue;
        }

        var array136 = ConvertPredictionsTo136Array(tiles);
        return GetShanten_from136Array(array136, _use_chiitoitsu, _use_kokushi);
    }

    /// <summary>
    /// �L���v��Ԃ��֐�
    /// </summary>
    /// <returns></returns>
    public List<Tile> GetEffectiveTiles(Tiles tiles)
    {
        //�V�����e�����̕]�����\�Ȗ���-1 �𒴂��Ă���ꍇ�͏I������
        if (tiles.TilesList.Count + tiles.MeldsList.Count * 3 > 13)
        {
            Debug.LogWarning("�v�̐����������܂�");
            return null;
        }

        List<Tile> result = new();
        int baseShanten = GetShanten(tiles);    //�]���̊�ƂȂ�V�����e����
        Tiles tmpTiles = new Tiles(tiles);  //���낢��M��̂�Tiles�̒l���R�s�[�������X�g���g��

        //37��ޑS�Ă̔v��1���ǉ������Ƃ��̃V�����e�����̕ω�����ɗL���v���擾����
        int maxTileID = 37;
        for (int i = 0; i < maxTileID; i++)
        {
            Tile t = new Tile(i);
            tmpTiles.AddTileToList(t);
            //�V�����e�����������Ă����ꍇ�͕Ԃ�l�ɒǉ�����
            if (GetShanten(tmpTiles) < baseShanten)
            {
                result.Add(t);
            }
            tmpTiles.RemoveTileFromList(t);
        }
        return result;
    }

    /// <summary>
    /// �_���v�Z���s���֐�
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="win_tile"></param>
    /// <param name="dora"></param>
    /// <param name="config"></param>
    /// <returns>HandResponse�I�u�W�F�N�g</returns>
    public HandResponse EstimateHandValue(Tiles tiles, Tile win_tile, List<Tile> dora, HandConfig config)
    {
        dynamic _calculator = mj_calculate.HandCalculator();
        HandResponse _handResponse = new HandResponse();

        //����1   tiles: array with 14 tiles in 136-tile format
        dynamic _tile_136Array = ConvertPredictionsTo136Array(tiles);

        //����2   win_tile: 136 format tile that caused win (ron or tsumo)
        Tiles win_tile_inTiles = new Tiles();
        win_tile_inTiles.AddTileToList(win_tile);
        dynamic _winTile_136Array = ConvertPredictionsTo136Array(win_tile_inTiles);

        //����3   array with Meld objects
        List<dynamic> _meldObjects =new();
        foreach(Meld m in tiles.MeldsList)
        {
            _meldObjects.Add(m.GetMeldObject());
        }

        //����4   array of tiles in 136-tile format
        Tiles dora_inTiles = new Tiles();
        foreach (Tile tile in dora)
        {
            dora_inTiles.AddTileToList(tile);
        }
        dynamic _dora_136Array = ConvertPredictionsTo136Array(dora_inTiles);

        //����5   HandConfig object
        dynamic _configObject = config.GetHandConfig();

        //�����Ōv�Z
        dynamic result = _calculator.estimate_hand_value(_tile_136Array, _winTile_136Array, _meldObjects, _dora_136Array, _configObject);

        //dynamic resul ���� HandConig�N���X�ւ̕ϊ�
        _handResponse.cost_main = result.cost["main"];
        _handResponse.cost_aditional = result.cost["additionl"];
        _handResponse.han=result.han;
        _handResponse.fu=result.fu;
        _handResponse.yaku=result.yaku;

        return _handResponse;
    }

}
