using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorModeDisplayController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _editorModeObjects;    // �ҏW���[�h�̎������\�������I�u�W�F�N�g
    private bool _isEditorMode = false; // �ҏW���[�h���ǂ����̃t���O

    //�n���h�g���b�L���O���̌��o�p.
    [SerializeField] private OVRHand _OVRHand;
    private bool _wasGuestureDetected;  //�O�t���[���Ƃ̔�r�p.

    private void Start()
    {
        //������.
        ToggleEditorModeObjects(_isEditorMode);
    }

    private void Update()
    {
        if (_OVRHand.IsTracked)
        {
            //�W�F�X�`���[��F�������^�C�~���O�Ń��[�h��؂�ւ�.
            if ((_wasGuestureDetected != IsGestureDetected()) && IsGestureDetected())
            {
                Debug.Log("toggled");
                ToggleEditorMode();
            }
            _wasGuestureDetected = IsGestureDetected();
        }
    }

    private bool IsGestureDetected()
    {
        Debug.Log($"{_OVRHand.GetFingerIsPinching(OVRHand.HandFinger.Middle)},{_OVRHand.GetFingerIsPinching(OVRHand.HandFinger.Ring)}");
        return _OVRHand.GetFingerIsPinching(OVRHand.HandFinger.Ring);
    }


    // �ҏW���[�h�ɉ����ăI�u�W�F�N�g��\��/��\���ɂ���
    private void ToggleEditorModeObjects(bool isEditorMode)
    {
        foreach (var obj in _editorModeObjects)
        {
            if (obj != null)
            {
                obj.SetActive(isEditorMode); // isEditorMode��true�Ȃ�\���Afalse�Ȃ��\��
            }
        }
    }

    // �ҏW���[�h��؂�ւ��郁�\�b�h
    public void ToggleEditorMode()
    {
        _isEditorMode = !_isEditorMode;
        ToggleEditorModeObjects(_isEditorMode); // �؂�ւ�����ɕ\��/��\�����X�V
    }
}
