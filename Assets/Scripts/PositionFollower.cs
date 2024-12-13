//******************************
//�I�u�W�F�N�g��ʂ̃I�u�W�F�N�g�ɒǏ]������N���X
//******************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PositionFollower : MonoBehaviour
{
    // �Ǐ]�Ώۂ̃I�u�W�F�N�g
    public Transform target;

    // �����I�t�Z�b�g�l�i�Ǐ]�ΏۂƂ̏������΋����j
    private Vector3 initialOffset;

    private void Start()
    {
        if (target != null)
        {
            // �����I�t�Z�b�g���v�Z
            initialOffset = transform.position - target.position;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // �����I�t�Z�b�g��ۂ��Ȃ���Ǐ]
            transform.position = target.position + initialOffset;
        }
    }

    private void OnValidate()
    {
        if (target != null)
        {
            // �����I�t�Z�b�g���Čv�Z�i�^�[�Q�b�g�ύX���ȂǂɑΉ��j
            initialOffset = transform.position - target.position;
        }
    }
}
