using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Tile(int num, TileType tileType)
    {
        type = tileType;
        number = num;
    }

    public enum TileType
    {
        NONE,
        MAN,    //�ݎq
        PIN,    //���q
        SOU,    //���q
        HONOR   //���v
    }

    public TileType type = TileType.NONE;
    /// <summary>
    /// ���v 0:�ԃh�� 1~9:�Ή����鐔
    /// ���v 1:�� 2:�� 3:�� 4:�k 5:�� 6:� 7:��
    /// </summary>
    public int number = -1;
}

public class Tiles
{
    public Tiles(string man="", string pin="", string sou="", string honor="")
    {
        tiles = new();
        AddTileToList(man, Tile.TileType.MAN);
        AddTileToList(pin, Tile.TileType.PIN);
        AddTileToList(sou, Tile.TileType.SOU);
        AddTileToList(honor, Tile.TileType.HONOR);
    }

    private void AddTileToList(string str,Tile.TileType type)
    {
        if (str == null) return;

        foreach (char c in str)
        {
            Tile t = new Tile((int)char.GetNumericValue(c), type);
            tiles.Add(t);
        }
    }

    public List<Tile> tiles;
}