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

    //シャンテン数を監視してリー棒の表示を切り替える.
    private void OnChangeShanten(int shanten)
    {
        //立直が成立していない　かつ　聴牌しているときに有効化.
        if (_handConfig.is_riichi == false)
        {
            //聴牌しているなら有効.
            if (shanten == 0)
            {
                ActivateRiichiStick();
            }
            //不正な値でなく、聴牌が成立していないなら無効化.
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

    //リー棒とレシーバーを表示する.
    private void ActivateRiichiStick()
    {
        _riichiStick.SetActive(true);
        _riichiStickReceiver.SetActive(true);
    }

    //リー棒とレシーバーを非表示にする.
    private void DeactivateRiichiStick()
    {
        _riichiStick.SetActive(false);
        _riichiStickReceiver.SetActive(false);
    }

    //立直有効化.
    public void OnEnableRiichi(Unit unit)
    {
        _handConfig.is_riichi = true;
        DeactivateRiichiStick();
    }

    //立直無効化.
    public void OnDisableRiichi()
    {
        _handConfig.is_riichi = false;
    }
}
