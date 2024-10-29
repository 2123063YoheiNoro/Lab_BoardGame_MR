using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Tile : IEquatable<Tile>
{
    public Tile(int num, TileType tileType)
    {
        this.type = tileType;
        //num��0~9�Ɏ��܂�悤�ɏC������
        num = Mathf.Clamp(num, 0, 9);
        this.Number = num;
        this.Id_37 = (int)tileType * 10 + Number;
    }
    public Tile(int id_37)
    {
        this.Id_37 = id_37;
        //tileType��10�̈�
        this.type = (TileType)(id_37 / 10);
        //number��1�̈�
        this.Number = id_37 % 10;
        //���v�͐��v�ƈقȂ�1����n�܂�̂�1�����Ă���
        if (this.type == TileType.HONOR)
        {
            Number++;
        }
    }
    //�R�s�[�p�R���X�g���N�^
    public Tile(Tile tile)
    {
        this.type = tile.type;
        this.Number = tile.Number;
        this.Id_37 = tile.Id_37;
    }

    public enum TileType
    {
        NONE = -1,
        MAN = 0,    //�ݎq
        PIN = 1,    //���q
        SOU = 2,    //���q
        HONOR = 3   //���v
    }

    [field: SerializeField] public TileType type { get; private set; } = TileType.NONE;
    /// <summary>
    /// ���v 0:�ԃh�� 1~9:�Ή����鐔
    /// ���v 1:�� 2:�� 3:�� 4:�k 5:�� 6:� 7:��
    /// </summary>
    [field: SerializeField] public int Number { get; private set; } = -1;
    [field: SerializeField] public int Id_37 { get; private set; }

    public bool Equals(Tile tile)
    {
        return this.Id_37 == tile.Id_37;
    }

    public override int GetHashCode()
    {
        return Id_37.GetHashCode();
    }
}

/// <summary>
/// �����̔v���܂Ƃ߂Ĉ������߂̃N���X
/// </summary>
[Serializable]
public class Tiles
{
    public Tiles(string man = "", string pin = "", string sou = "", string honor = "", List<Meld> melds = null)
    {
        TilesList = new();
        AddTileToList(man, Tile.TileType.MAN);
        AddTileToList(pin, Tile.TileType.PIN);
        AddTileToList(sou, Tile.TileType.SOU);
        AddTileToList(honor, Tile.TileType.HONOR);
        if (melds != null)
        {
            this.MeldsList = new List<Meld>(melds);
        }
        else
        {
            MeldsList = new();
        }
    }
    //�R�s�[�p�R���X�g���N�^
    public Tiles(Tiles tiles)
    {
        TilesList = new(tiles.TilesList);
        MeldsList = new(tiles.MeldsList);
    }

    /// <summary>
    /// �����񂩂�v�ɕϊ����ă��X�g�ɒǉ�����
    /// </summary>
    /// <param name="str"></param>
    /// <param name="type"></param>
    public void AddTileToList(string str, Tile.TileType type)
    {

        //�������1�������������ă^�C���̃C���X�^���X�𐶐�����
        foreach (char c in str)
        {
            Tile t = new Tile((int)char.GetNumericValue(c), type);
            TilesList.Add(t);
        }
    }
    /// <summary>
    /// Tile�N���X�����X�g�ɒǉ�����
    /// </summary>
    /// <param name="t"></param>
    public void AddTileToList(Tile t)
    {
        //�R�s�[������ŎQ�Ɠn�����������
        Tile tile = new Tile(t);
        TilesList.Add(tile);
    }

    public bool RemoveTileFromList(int num, Tile.TileType type)
    {
        Tile t = new Tile(num, type);
        return TilesList.Remove(t);
    }
    public bool RemoveTileFromList(Tile tile)
    {
        return TilesList.Remove(tile);
    }

    /// <summary>
    /// ��v���l�����Ĕv�̎c�薇����Ԃ��֐�
    /// </summary>
    /// <returns></returns>
    public int GetRemainingTilesCount(int num, Tile.TileType type)
    {
        Tile t = new Tile(num, type);
        int maxCountPerTile = 4;
        //��5��������v�̍ő吔��1
        if (num == 0)
        {
            maxCountPerTile = 1;
        }

        int result = maxCountPerTile - TilesList.Count(tile => tile == t);
        return result;
    }


    [field: SerializeField] public List<Tile> TilesList { get; private set; }

    [field: SerializeField] public List<Meld> MeldsList { get; private set; }
}