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
        //�V�����e������-1��艺�͕s���Ȓl �v����������Ƃ��ɓn�����
        if (shanten < -1)
        {
            result = "too many tiles";
        }
        else
        {
            //�R�[�h�x�^�����C���������̂Ō�ł����ƊǗ�����
            result = shanten.ToString() + " shanten";
        }
        text.text = result;
    }
}
