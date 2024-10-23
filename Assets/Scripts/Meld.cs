using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using System.Linq;
using System;

[Serializable]
public class Meld
{
    //python���C�u�����C���|�[�g�p�̕ϐ�
    private dynamic mjpy_utils;
    private dynamic mjpy_tiles;
    public enum MeldType
    {
        CHI,
        PON,
        ANKAN,
        DAIMINKAN,
        SHOUMINKAN,
        NUKI
    }
    public MeldType meldType;
    public Tiles tiles;

    public Meld(MeldType type, Tiles tiles)
    {
        this.meldType = type;
        this.tiles = tiles;

        mjpy_utils = Py.Import("mahjong.utils");
        mjpy_tiles = Py.Import("mahjong.tile");
    }
    public Meld(MeldType type, string man, string pin, string sou, string honor)
    {
        this.meldType = type;
        tiles = new Tiles(man, pin, sou, honor);

        mjpy_utils = Py.Import("mahjong.utils");
        mjpy_tiles = Py.Import("mahjong.tile");
    }

    //���̎�ނƔv�̑g�ݍ��킹���L�����ǂ�����Ԃ�
    public bool IsValid()
    {
        bool result;
        switch (meldType)
        {
            case MeldType.CHI:
                result = Is_CHI();
                break;
            case MeldType.PON:
                result = Is_PON();
                break;
            case MeldType.ANKAN:
            case MeldType.DAIMINKAN:
            case MeldType.SHOUMINKAN:
                result = Is_KAN();
                break;
            case MeldType.NUKI:
                result = true;
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    /// <summary>
    /// �`�[���������邩�𔻒肷��
    /// </summary>
    /// <returns></returns>
    private bool Is_CHI()
    {
        //�������v�̑�����3�ł���E���v�ł���E�A�Ԃł���

        //�v�̑�����3���ǂ������`�F�b�N����
        if (tiles.TilesList.Count != 3) return false;

        //�S�ē�����ނ̐��v���ǂ������`�F�b�N����
        Tile.TileType tmpTileType = Tile.TileType.NONE;
        foreach (Tile t in tiles.TilesList)
        {
            //���v�������Ă�����s����
            if (t.type == Tile.TileType.HONOR)
            {
                return false;
            }

            //�v�̎�ނ��L�^���A�قȂ�v�̎�ނ�����Εs����;
            if (tmpTileType == Tile.TileType.NONE)
            {
                tmpTileType = t.type;
            }
            else
            {
                if (tmpTileType != t.type)
                {
                    return false;
                }
            }
        }

        //�A�Ԃ��ǂ������`�F�b�N����
        //0(��5��\��)��5�ɏ���������
        Tiles fixedTiles = tiles;
        if (fixedTiles.RemoveTileFromList(0, tmpTileType))
        {
            fixedTiles.AddTileToList("5", tmpTileType);
        }
        //�\�[�g���Ĕ�r����
        var _sortedTiles = fixedTiles.TilesList.OrderBy(t => t.Number).ToList();
        if ((_sortedTiles[0].Number == _sortedTiles[1].Number - 1) &&
            (_sortedTiles[0].Number == _sortedTiles[2].Number - 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// �|�����������邩�𔻒肷��
    /// </summary>
    /// <returns></returns>
    private bool Is_PON()
    {
        //�������v�̑�����3�ł���E���������ł���

        //�v�̑�����3�ł��邩���`�F�b�N����
        if (tiles.TilesList.Count != 3) return false;

        //���������ł��邩���`�F�b�N����
        if ((tiles.TilesList[0].Number == tiles.TilesList[1].Number) &&
            (tiles.TilesList[0].Number == tiles.TilesList[2].Number))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// �J�����������邩�𔻒肷��
    /// </summary>
    /// <returns></returns>
    private bool Is_KAN()
    {
        //�������v�̑�����4�ł���E���������ł���

        //�v�̑�����4�ł��邩���`�F�b�N����
        if (tiles.TilesList.Count != 4) return false;

        //���������ł��邩���`�F�b�N����
        if ((tiles.TilesList[0].Number == tiles.TilesList[1].Number) &&
            (tiles.TilesList[0].Number == tiles.TilesList[2].Number) &&
            (tiles.TilesList[0].Number == tiles.TilesList[3].Number))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
