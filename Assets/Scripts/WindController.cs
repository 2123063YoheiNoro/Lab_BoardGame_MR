using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindController : MonoBehaviour
{
    [SerializeField] private HandScoreModel _scoreModel;
    private HandConfig _handConfig;

    [SerializeField] private TMP_Text _playerWindText;
    [SerializeField] private TMP_Text _roundWindText;

    private enum Wind
    {
        EAST = 0,
        SOUTH = 1,
        WEST = 2,
        NORTH = 3
    }

    private Wind _playerWind = Wind.EAST;   //自風.
    private Wind _roundWind = Wind.EAST;    //場風.

    private void Start()
    {
        _handConfig = _scoreModel._handConfig;
    }

    public void ChangeWind()
    {
        //自風を1つ隣の方角にする.
        _playerWind = (Wind)(((int)_playerWind + 1) % 4);

        //自風が北→東になったとき場風を変化させる.
        if (_playerWind == Wind.EAST)
        {
            _roundWind = (_roundWind == Wind.EAST) ? Wind.SOUTH : Wind.EAST;
        }

        //点数計算用の自風と場風を更新.
        ApplyWind();
    }

    private void ApplyWind()
    {
        //enumをintに変換して適用する.
        int playerWindValue=ConvertWindToStaticInt(_playerWind);
        int roundWindValue = ConvertWindToStaticInt(_roundWind);

        _handConfig.player_wind = playerWindValue;
        _handConfig.round_wind = roundWindValue;

        _playerWindText.text = $"{ConvertWindToString(_playerWind)}家";
        _roundWindText.text = $"{ConvertWindToString(_roundWind)}場";
    }

    //HandConfigで実際に使う値に変換する.
    private int ConvertWindToStaticInt(Wind wind)
    {
        switch (wind)
        {
            case Wind.EAST:
                return HandConfig.EAST;
            case Wind.SOUTH:
                return HandConfig.SOUTH;
            case Wind.WEST:
                return HandConfig.WEST;
            case Wind.NORTH:
                return HandConfig.SOUTH;
        }

        return HandConfig.EAST;
    }

    private string ConvertWindToString(Wind wind)
    {
        switch (wind)
        {
            case Wind.EAST:
                return "東";
            case Wind.SOUTH:
                return "南";
            case Wind.WEST:
                return "西";
            case Wind.NORTH:
                return "北";
        }
        return "東";
    }
}
