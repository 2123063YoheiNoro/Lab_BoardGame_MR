using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
public class HandConfig
{
    private static int NONE = -1;
    private static int EAST = 27;
    private static int SOUTH = 28;
    private static int WEST = 29;
    private static int NORTH = 30;

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

    public int player_wind = NONE;
    public int round_wind = NONE;


    private dynamic mj_hand_config;

    public HandConfig()
    {
        mj_hand_config = Py.Import("mahjong.hand_calculating.hand_config");
    }

    public dynamic GetHandConfig()
    {
        dynamic config = mj_hand_config.HandConfig(
            is_tsumo,
            is_riichi,
            is_ippatsu,
            is_rinshan,
            is_chankan,
            is_haitei,
            is_houtei,
            is_daburu_riichi,
            is_nagashi_mangan,
            is_tenhou,
            is_renhou,
            is_chiihou,
            is_open_riichi,
            player_wind,
            round_wind
            );
        return config;
    }
}
