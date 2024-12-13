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

    [SerializeField] private bool followRotationX = false; // X���̉�]��Ǐ]���邩
    [SerializeField] private bool followRotationY = false; // Y���̉�]��Ǐ]���邩
    [SerializeField] private bool followRotationZ = false; // Z���̉�]��Ǐ]���邩

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


        // ��]�̒Ǐ]
        if (followRotationX || followRotationY || followRotationZ)
        {
            Vector3 targetEulerAngles = target.rotation.eulerAngles;
            Vector3 currentEulerAngles = transform.rotation.eulerAngles;

            transform.rotation = Quaternion.Euler(
                followRotationX ? targetEulerAngles.x : currentEulerAngles.x,
                followRotationY ? targetEulerAngles.y : currentEulerAngles.y,
                followRotationZ ? targetEulerAngles.z : currentEulerAngles.z
            );
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
