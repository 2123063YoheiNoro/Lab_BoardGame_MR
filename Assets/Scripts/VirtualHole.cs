using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class VirtualHole : MonoBehaviour
{
    [SerializeField] Transform defaultPositon;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private AudioSource _audioSource;
    private AudioClip _audioClip;
    private AudioLibrary _audioLibrary;
    private void OnEnable()
    {
        //�\�����ꂽ�u�Ԃɉ���炷.
        _audioLibrary ??= Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        _audioClip  ??= _audioLibrary.GetAudioClip(AudioType.Environment_BreakCeiling);
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(_audioClip);
        }
        //�ʒu��V��ɍ��킹��.
        Vector3 position = GetHolePosition();
        transform.position = position;
    }

    private void Update()
    {
        Vector3 position = GetHolePosition();
        transform.position = position;
    }
    private Vector3 GetHolePosition()
    {
        Vector3 result;
        //�^��Ƀ��C��ł��ē��������ꏊ��Ԃ�.
        Ray ray = new Ray(Vector3.up, defaultPositon.position);
        RaycastHit hitInfo;
        float length = 5;
        //���C���΂�.
        if (Physics.Raycast(ray, out hitInfo, length, _layerMask))
        {
            result = hitInfo.point;
        }
        else
        {
            //���C��������Ȃ���ΐe�̈ʒu��Ԃ��Ă���.
            result = defaultPositon.position;
        }
        return result;

    }
}
