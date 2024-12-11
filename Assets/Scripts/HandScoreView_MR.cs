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
    [SerializeField] private float TextSpeed = 0.5f;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float appearIntervalSec = 0.75f;    //�����̕\������Ԋu(�b).
    private Coroutine _latestCoroutine = null;  //�R���[�`����~�p.
    private List<GameObject> _yaku_textObjectList = new List<GameObject>();  //�e�L�X�g�폜�p.

    public void UpdateResult(HandResponse handResponse)
    {
        Debug.Log("���X�V : "+handResponse.yaku);
        if (handResponse.cost_main == 0)
        {
            return;
        }

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
            GameObject yakuText = Instantiate(yakuTextPrefab, yakuTextParent.position, yakuTextPrefab.transform.rotation);

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

            //rigid body ������ɗ͂�������.
            float yaku_speed = TextSpeed;
            Rigidbody yaku_rb = yakuText.GetComponent<Rigidbody>();
            yaku_rb.AddForce(Vector3.up * yaku_speed, ForceMode.VelocityChange);

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
    }

    private void InstanceYakuTextObject(string yaku)
    {
        GameObject yakuTextPrefab = Addressables.LoadAssetAsync<GameObject>("Result_YakuText").WaitForCompletion();
        GameObject yakuText = Instantiate(yakuTextPrefab, yakuTextParent.position, yakuTextPrefab.transform.rotation);

        //�Ǘ��p�̃��X�g�ɒǉ�.
        _yaku_textObjectList.Add(yakuText);
        //�e�ݒ�.
        yakuText.transform.SetParent(yakuTextParent);


        //��������R���|�[�l���g�֘A.
        //�e�L�X�g�̐ݒ�.
        TMP_Text tmp_yaku = yakuText.GetComponent<TMP_Text>();
        tmp_yaku.text = yaku;
        tmp_yaku.ForceMeshUpdate();

        //BoxCollider��ǉ�����.
        BoxCollider boxCollider = yakuText.AddComponent<BoxCollider>();
        Bounds tmpBounds = tmp_yaku.bounds;
        boxCollider.size = new Vector3(tmpBounds.size.x, tmpBounds.size.y, 1);
        boxCollider.center = tmpBounds.center;

        //rigid body ������ɗ͂�������.
        float yaku_speed = 1.0f;
        Rigidbody yaku_rb = yakuText.GetComponent<Rigidbody>();
        yaku_rb.AddForce(Vector3.up * yaku_speed, ForceMode.VelocityChange);
    }

    private void InstanceScoreTextObject(HandResponse handResponse)
    {
        GameObject scoreTextPrefab = Addressables.LoadAssetAsync<GameObject>("Result_ScoreText").WaitForCompletion();
        GameObject scoreText = Instantiate(scoreTextPrefab, yakuTextParent.position, scoreTextPrefab.transform.rotation);

        //�Ǘ��p�̃��X�g�ɒǉ�.
        _yaku_textObjectList.Add(scoreText);
        //�e�ݒ�.
        scoreText.transform.SetParent(yakuTextParent);

        //�e�L�X�g�̐ݒ�.
        TMP_Text tmp_score = scoreText.GetComponent<TMP_Text>();
        int cost = handResponse.cost_main + handResponse.cost_aditional * 2;
        tmp_score.text = cost.ToString();

        //rigid body ������ɗ͂�������.
        float score_speed = 1.0f;
        Rigidbody score_rb = scoreText.GetComponent<Rigidbody>();
        score_rb.AddForce(Vector3.up * score_speed, ForceMode.VelocityChange);
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
