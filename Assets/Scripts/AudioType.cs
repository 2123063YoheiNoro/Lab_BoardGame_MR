using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    None,
    Shanten_CountUp,
    Shanten_CountDown,
    Shanten_Tenpai,
    Shanten_EnableEffectiveTile,
    Shanten_DisableEffectiveTile,

    Agari_Enable,
    Agari_EnableYakuText,
    Agari_EnableScoreText,
    Agari_Disable,

    RecommendedDiscard_Enable,
    RecommendedDiscard_Disable,

    Environment_BreakCeiling,

    BGM_Default,
    BGM_Tenpai
}