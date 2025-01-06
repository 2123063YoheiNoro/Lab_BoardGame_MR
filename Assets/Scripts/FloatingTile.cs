//****************************
//rigid body�̔v���󒆂ɌŒ肷��
//****************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTile : MonoBehaviour
{
    [SerializeField] public Vector3 targetLocalPos;
    [SerializeField] public Transform lookingPos;
    [SerializeField] public float speed = 1;
    [SerializeField] public float torqueStrength = 0.01f;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //�ʒu�̏���.
        //�^�[�Q�b�g�̃��[�J�����W�����[���h���W�ɕϊ����č������v�Z����.
        Vector3 targetPos = transform.parent.TransformPoint(targetLocalPos);
        Vector3 dir = targetPos - transform.position;
        _rb.AddForce(dir * speed, ForceMode.Force);

        //��]�̏���.
        if (lookingPos == null) return;
        Vector3 targetRotPos = lookingPos.transform.position;
        Vector3 rotDir = targetRotPos - transform.position;
        //�I�u�W�F�N�g�̐��ʑ����^�[�Q�b�g�Ɍ����悤�ɂ���.
        Quaternion fixedRotDir = Quaternion.FromToRotation(transform.forward, rotDir) * transform.rotation;
        //�������牺��chatgpt.
        Quaternion deltaRotation = fixedRotDir * Quaternion.Inverse(transform.rotation);
        Vector3 axis;
        float angle;
        deltaRotation.ToAngleAxis(out angle, out axis);

        // �����Ȋp�x�̉�]�͖������邽�߁A臒l��ݒ�.
        if (angle > 0.1f)
        {
            // ��]���ɉ����ăg���N��������.
            _rb.AddTorque(axis * angle * torqueStrength, ForceMode.VelocityChange);
        }
    }
}
