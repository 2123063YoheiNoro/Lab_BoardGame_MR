using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandShantenView : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    private void Start()
    {
        text.text = "";
    }
    public void UpdateText(int shanten)
    {
        string result = "";
        //シャンテン数が-1より下は不正な値 牌が多すぎるときに渡される
        if (shanten < -1)
        {
            result = "too many tiles";
        }
        else
        {
            //コードベタ書き気持ち悪いので後でちゃんと管理する
            result = shanten.ToString() + " shanten";
        }
        text.text = result;
    }
}
