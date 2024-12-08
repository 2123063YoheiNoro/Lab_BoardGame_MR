using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HandShantenPresenter : MonoBehaviour
{
    [SerializeField]private HandShantenView shantenView;
    [SerializeField]private HandShantenModel shantenModel;
    private void Start()
    {
        //���f���̃V�����e�������r���[�ɓo�^����
        shantenModel.shanten
            .Subscribe(shantenView.UpdateText)
            .AddTo(this);
    }
}
