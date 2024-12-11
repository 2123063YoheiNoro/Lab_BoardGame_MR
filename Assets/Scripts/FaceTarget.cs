//************************
//GameObject����������Ɍ�������������N���X
//************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    //�ق�chatgpt�Y.��������.
    [SerializeField] private Transform target;

    private void Update()
    {
        if (target != null)
        {
            Vector3 dir = target.position - this.gameObject.transform.position;
            //���������΂������̂ł��������C��.
            RotateTowardsOnYAxis(-dir);
        }
    }
    private void RotateTowardsOnYAxis(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            direction.y = 0;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Slerp���g�p���ăX���[�Y��Y����]
            Quaternion smoothedRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, smoothedRotation, Time.deltaTime * 5f);
        }
    }
}
