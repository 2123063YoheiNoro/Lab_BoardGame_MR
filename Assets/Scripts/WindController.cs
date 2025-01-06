using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    [SerializeField] private HandConfig _handConfig;

    private enum Wind
    {
        EAST = 0,
        SOUTH = 1,
        WEST = 2,
        NORTH = 3
    }

    private Wind _playerWind = Wind.EAST;   //����.
    private Wind _roundWind = Wind.EAST;    //�ꕗ.

    public void ChangeWind()
    {
        //������1�ׂ̕��p�ɂ���.
        _playerWind = (Wind)(((int)_playerWind + 1) % 4);

        //�������k�����ɂȂ����Ƃ��ꕗ��ω�������.
        if (_playerWind == Wind.EAST)
        {
            _roundWind = (_roundWind == Wind.EAST) ? Wind.SOUTH : Wind.EAST;
        }

        //�_���v�Z�p�̎����Əꕗ���X�V.
        ApplyWind();
    }

    private void ApplyWind()
    {
        //enum��int�ɕϊ����ēK�p����.
        int playerWindValue=ConvertWindToStaticInt(_playerWind);
        int roundWindValue = ConvertWindToStaticInt(_roundWind);

        _handConfig.player_wind = playerWindValue;
        _handConfig.round_wind = roundWindValue;
    }

    //HandConfig�Ŏ��ۂɎg���l�ɕϊ�����.
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
}
