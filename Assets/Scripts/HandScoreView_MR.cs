using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HandScoreView_MR : MonoBehaviour
{
    [SerializeField] GameObject yaku_prefub;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject yaku_parent;
    [SerializeField] TMP_Text cost; //�_��
    [SerializeField] TMP_Text cost_detail;//�x�����̏ڍׂ�\������e�L�X�g 2000�I�[���Ƃ�18000,6000�Ƃ�
    [SerializeField] TMP_Text han;  //�͂�@�P�ʂ������œ����
    [SerializeField] TMP_Text fu;   //���@������P�ʍ���

    private float appearIntervalSec = 0.75f;
    private List<GameObject> yaku_objectList = new();

    private IEnumerator latestC;

    public void UpdateResult(HandResponse handResponse)
    {
        Debug.Log("���X�V");
        StopAllCoroutines();
        StartCoroutine(InstanceYakuUI(handResponse));
    }

    //��ʏ�̖�S������ UI������
    public void ClearScore()
    {
        foreach (GameObject yaku in yaku_objectList)
        {
            Destroy(yaku);
        }
        canvas.SetActive(false);
        cost.text = "";
        cost_detail.text = "";
        han.text = "";
        fu.text = "";
    }

    IEnumerator InstanceYakuUI(HandResponse handResponse)
    {
        //ClearScore();
        canvas.SetActive(true);
        List<string> yaku_list = ConvertYaku_To_YakuList(handResponse.yaku);
        //���̖��O�Ƃ͂񐔂�K�p�����e�L�X�g���쐬����
        foreach (string yaku in yaku_list)
        {
            GameObject yaku_element = Instantiate(yaku_prefub);
            yaku_objectList.Add(yaku_element);
            yaku_element.transform.SetParent(yaku_parent.transform, false);
            yaku_element.SetActive(false);

            //�v���n�u�̎q���𖼃e�L�X�g�A�����͂�e�L�X�g
            Transform yaku_name = yaku_element.transform.GetChild(0);
            Transform han = yaku_name.transform.GetChild(0);
            TMP_Text yaku_name_text = yaku_name.GetComponent<TMP_Text>();
            TMP_Text han_text = han.GetComponent<TMP_Text>();

            yaku_name_text.text = yaku;
            han_text.text = ""; //�����Ƃ̂͂�̎擾�͕��G�Ȃ̂Ō��
        }

        //��������\������
        foreach (GameObject yaku in yaku_objectList)
        {
            //�L�������Ď��̖��̕\���܂ő҂�
            yaku.SetActive(true);
            yield return new WaitForSeconds(appearIntervalSec);
        }

        //�_����\������
        int _cost = handResponse.cost_main + handResponse.cost_aditional * 2;
        cost.text = _cost.ToString();
        cost_detail.text = String.Format("{0} - {1}", handResponse.cost_main.ToString(), handResponse.cost_aditional.ToString());
        han.text = handResponse.han.ToString();
        fu.text= handResponse.fu.ToString();
    }

    //python���C�u��������̓��͂����X�g�ɕϊ�����
    private List<string> ConvertYaku_To_YakuList(string yaku_string)
    {
        //[tannyao,dora,aka]�̂悤�ȂP�̕����񂩂烊�X�g�ɕϊ�����
        string trimmed = yaku_string.Trim('[', ']');
        List<string> result = new List<string>(trimmed.Split(','));

        return result;
    }
}
