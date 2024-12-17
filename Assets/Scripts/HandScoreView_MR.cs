using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.AddressableAssets;

public class HandScoreView_MR : MonoBehaviour
{
    [SerializeField] private Transform yakuTextParent;  //�e�L�X�g�̐e,�e�L�X�g�����������ꏊ.
    [SerializeField] private Vector3 _instancePosoffset = Vector3.zero;

    [SerializeField] private GameObject _preParticleEffect; //�_���\���̑O�ɐ��������p�[�e�B�N��.
    [SerializeField] private GameObject _virtualHole;   //�V��ɋ󂭌�.
    [SerializeField] private LayerMask _sceneMeshLayer; //�V��̃��C���[.

    [SerializeField] private float TextSpeed = 0.5f;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float appearIntervalSec = 0.75f;    //�����̕\������Ԋu(�b).
    [SerializeField] private float displayTime = 5; //�\���������鎞��(�b).


    private Coroutine _latestCoroutine = null;  //�R���[�`����~�p.
    private List<GameObject> _yaku_textObjectList = new List<GameObject>();  //�e�L�X�g�폜�p.
    [SerializeField] private HandResponse _latestValidHandResponse = null;    //�ω����m�p.
    private bool _isCountDown = false;  //�\������߂�܂ł̃J�E���g�_�E���t���O.
    private bool _isInstanceCoroutineRunning = false;
    [SerializeField] private float _reminingTime = 0;

    //��\���܂ł̃J�E���g�_�E��������.
    private void Update()
    {
        if (_isCountDown)
        {
            _reminingTime -= Time.deltaTime;
        }
        //�J�E���g���Ȃ��Ȃ����A���V�[����Ƀe�L�X�g���c���Ă���ꍇ�A���R���[�`���������ĂȂ��ꍇ.
        if (_reminingTime < 0 && _yaku_textObjectList.Count != 0 && !_isInstanceCoroutineRunning)
        {
            ClearScoreTextObject();
            _virtualHole.gameObject.SetActive(false);
            _latestValidHandResponse = null;
        }
    }

    public void UpdateResult(HandResponse handResponse)
    {
        Debug.Log("���X�V : " + handResponse.yaku);

        //�L���Ȃ�����łȂ��Ȃ牽�����Ȃ�.
        if (handResponse.cost_main == 0)
        {
            //��\���܂ł̃J�E���g�_�E���J�n.
            if (!_isCountDown)
            {
                StartCountDown();
            }
            return;
        }
        //�O��̂�����Ɠ����Ȃ牽�����Ȃ�.��u�����v�̔F�����r�؂ꂽ���z��.
        else if (handResponse.Equals(_latestValidHandResponse))
        {
            StopCountDown();
            return;
        }
        else
        {
            StopCountDown();
        }

        _latestValidHandResponse = handResponse;

        //���o���ɍX�V�͂�������I�����ĐV������������.
        if (_latestCoroutine != null)
        {
            StopCoroutine(_latestCoroutine);
        }
        //�V�[����̕��������ĐV��������.
        ClearScoreTextObject();
        _latestCoroutine = StartCoroutine(InstanceYakuTextObject(handResponse));
    }

    //�V�[����̕����S������.
    public void ClearScoreTextObject()
    {
        //�I�u�W�F�N�g�����ĎQ�Ƃ�����.
        foreach (GameObject yaku in _yaku_textObjectList)
        {
            Destroy(yaku);
        }
        _yaku_textObjectList.Clear();
    }

    private IEnumerator InstanceYakuTextObject(HandResponse handResponse)
    {
        _isInstanceCoroutineRunning = true;

        //�G�t�F�N�g�̕\��.
        Instantiate(_preParticleEffect, yakuTextParent.transform.position, _preParticleEffect.transform.rotation);
        _virtualHole.gameObject.SetActive(true);

        //�e�L�X�g�̐����ʒu(�V��)���擾.
        Vector3 instancePos = GetInstantiatePosition();

        //����炷.
        AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        AudioClip ac_Agari_enable = audioLibrary.GetAudioClip(AudioType.Agari_Enable);
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(ac_Agari_enable);
        }

        yield return new WaitForSeconds(appearIntervalSec);

        //�������X�g������.
        List<string> yaku_list = ConvertYaku_To_YakuList(handResponse.yaku);

        //���̕����𐶐�!.
        foreach (string yaku in yaku_list)
        {
            //���̕����I�u�W�F�N�g����.
            GameObject yakuTextPrefab = Addressables.LoadAssetAsync<GameObject>("Result_YakuText").WaitForCompletion();
            GameObject yakuText = Instantiate(yakuTextPrefab, instancePos, yakuTextParent.transform.rotation);

            //�Ǘ��p�̃��X�g�ɒǉ�.
            _yaku_textObjectList.Add(yakuText);
            //�e�ݒ�.
            yakuText.transform.SetParent(yakuTextParent);


            //��������R���|�[�l���g�֘A.
            //�e�L�X�g�̐ݒ�.
            TMP_Text tmp_yaku = yakuText.GetComponent<TMP_Text>();
            tmp_yaku.text = yaku;
            tmp_yaku.ForceMeshUpdate();

            yield return null;

            //BoxCollider��ǉ�����.
            BoxCollider boxCollider = yakuText.AddComponent<BoxCollider>();
            Bounds tmpBounds = tmp_yaku.bounds;
            boxCollider.size = new Vector3(tmpBounds.size.x, tmpBounds.size.y, 1);  //���݂�ǉ�����.���l�͓K��.
            boxCollider.center = tmpBounds.center;

            //rigid body �������ɗ͂�������.
            float yaku_speed = TextSpeed;
            Rigidbody yaku_rb = yakuText.GetComponent<Rigidbody>();
            yaku_rb.AddForce(-Vector3.up * yaku_speed, ForceMode.VelocityChange);

            //����炷
            AudioClip ac_enableYakuText = audioLibrary.GetAudioClip(AudioType.Agari_EnableYakuText);
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(ac_enableYakuText);
            }

            //���̕��������܂ł�����Ƒ҂�.
            yield return new WaitForSeconds(appearIntervalSec);
        }

        //�_���\���܂ŏ����Ԃ��J����.
        yield return new WaitForSeconds(appearIntervalSec);

        //�_���̕����𐶐�����.
        InstanceScoreTextObject(handResponse);

        //����炷
        AudioClip ac_enableScoreText = audioLibrary.GetAudioClip(AudioType.Agari_EnableScoreText);
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(ac_enableScoreText);
        }


        Addressables.Release(audioLibrary);

        yield return new WaitForSeconds(appearIntervalSec);

        _isInstanceCoroutineRunning = false;
    }

    private void InstanceScoreTextObject(HandResponse handResponse)
    {
        //�e�L�X�g�̐����ʒu(�V��)���擾.
        Vector3 instancePos = GetInstantiatePosition();

        GameObject scoreTextPrefab = Addressables.LoadAssetAsync<GameObject>("Result_ScoreText").WaitForCompletion();
        GameObject scoreText = Instantiate(scoreTextPrefab, instancePos, scoreTextPrefab.transform.rotation);

        //�Ǘ��p�̃��X�g�ɒǉ�.
        _yaku_textObjectList.Add(scoreText);
        //�e�ݒ�.
        scoreText.transform.SetParent(yakuTextParent);

        //�e�L�X�g�̐ݒ�.
        TMP_Text tmp_score = scoreText.GetComponent<TMP_Text>();
        int cost = handResponse.cost_main + handResponse.cost_aditional * 2;
        tmp_score.text = cost.ToString();

        //rigid body �������ɗ͂�������.
        float score_speed = 1.0f;
        Rigidbody score_rb = scoreText.GetComponent<Rigidbody>();
        score_rb.AddForce(-Vector3.up * score_speed, ForceMode.VelocityChange);
    }


    private Vector3 GetInstantiatePosition()
    {
        Vector3 result;
        //�^��Ƀ��C��ł��ē��������ꏊ��Ԃ�.
        Ray ray = new Ray(Vector3.up, yakuTextParent.position);
        RaycastHit hitInfo;
        float length = 5;
        //���C���΂�.
        if (Physics.Raycast(ray, out hitInfo, length, _sceneMeshLayer))
        {
            result = hitInfo.point + _instancePosoffset;
        }
        else
        {
            //���C��������Ȃ���ΐe�̈ʒu��Ԃ��Ă���.
            result = yakuTextParent.position;
        }
        return result;

    }

    private void StartCountDown()
    {
        _isCountDown = true;
        _reminingTime = displayTime;
    }

    private void StopCountDown()
    {
        _isCountDown = false;
        _reminingTime = displayTime;
    }

    //python���C�u��������̓��͂����X�g�ɕϊ�����
    private List<string> ConvertYaku_To_YakuList(string yaku_string)
    {
        //[tannyao,dora,aka]�̂悤�ȂP�̕����񂩂烊�X�g�ɕϊ�����
        string trimmed = yaku_string.Trim('[', ']');
        List<string> result = new List<string>(trimmed.Split(','));

        return result;
    }
}
