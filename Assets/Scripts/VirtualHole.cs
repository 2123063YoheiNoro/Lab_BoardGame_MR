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
        //表示された瞬間に音を鳴らす.
        _audioLibrary ??= Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        _audioClip  ??= _audioLibrary.GetAudioClip(AudioType.Environment_BreakCeiling);
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(_audioClip);
        }
        //位置を天井に合わせる.
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
        //真上にレイを打って当たった場所を返す.
        Ray ray = new Ray(Vector3.up, defaultPositon.position);
        RaycastHit hitInfo;
        float length = 5;
        //レイを飛ばす.
        if (Physics.Raycast(ray, out hitInfo, length, _layerMask))
        {
            result = hitInfo.point;
        }
        else
        {
            //レイが当たらなければ親の位置を返しておく.
            result = defaultPositon.position;
        }
        return result;

    }
}
