using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RiichiController : MonoBehaviour
{
    [SerializeField] private HandConfig _handConfig;
    [SerializeField] private GameObject _riichiStick;
    [SerializeField] private GameObject _riichiStickReceiver;
    [SerializeField] private HandShantenModel _handShanten;

    private void Start()
    {
        RiichiStickReceiver riichiStickReceiver = _riichiStickReceiver.GetComponent<RiichiStickReceiver>();
        riichiStickReceiver.subject
            .Subscribe(unit => OnEnableRiichi(unit))
            .AddTo(this);
        _handShanten.shanten
            .Subscribe(OnChangeShanten)
            .AddTo(this);
    }

    //�V�����e�������Ď����ă��[�_�̕\����؂�ւ���.
    private void OnChangeShanten(int shanten)
    {
        //�������������Ă��Ȃ��@���@���v���Ă���Ƃ��ɗL����.
        if (_handConfig.is_riichi == false)
        {
            //���v���Ă���Ȃ�L��.
            if (shanten == 0)
            {
                ActivateRiichiStick();
            }
            //�s���Ȓl�łȂ��A���v���������Ă��Ȃ��Ȃ疳����.
            else if (shanten > 0)
            {
                DeactivateRiichiStick();
            }
        }
        else
        {
            DeactivateRiichiStick();
        }
    }

    //���[�_�ƃ��V�[�o�[��\������.
    private void ActivateRiichiStick()
    {
        _riichiStick.SetActive(true);
        _riichiStickReceiver.SetActive(true);
    }

    //���[�_�ƃ��V�[�o�[���\���ɂ���.
    private void DeactivateRiichiStick()
    {
        _riichiStick.SetActive(false);
        _riichiStickReceiver.SetActive(false);
    }

    //�����L����.
    public void OnEnableRiichi(Unit unit)
    {
        _handConfig.is_riichi = true;
        DeactivateRiichiStick();
    }

    //����������.
    public void OnDisableRiichi()
    {
        _handConfig.is_riichi = false;
    }
}
