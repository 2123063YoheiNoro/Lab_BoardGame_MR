using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;

public class HandShantenView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioSource _audioSource;

    private int _latestShanten = int.MaxValue; //�V�����e�����ω����m�p.
    private void Start()
    {
        text.text = "";
    }
    public void UpdateText(int shanten)
    {
        Debug.Log("update shanten : " + shanten);
        string result = "";
        //�V�����e������-1��艺�͕s���Ȓl �v����������Ƃ��ɓn�����.
        if (shanten < -1)
        {
            result = "---";
        }
        else
        {
            if (shanten == -1)  //�a����.
            {
                result = "�a��";
            }
            else if (shanten == 0)  //���v��.
            {
                result = "���v";
            }
            else
            {
                //�R�[�h�x�^�����C���������̂Ō�ł����ƊǗ�����.
                result = shanten.ToString() + "�V�����e��";
            }
        }
        text.text = result;

        //���ʉ���炷.
        PlaySE(shanten, _latestShanten);

        //��r���I������̂ōŐV�̒l�ɏ��������Ă���.
        _latestShanten = shanten;
    }

    /// <summary>
    /// ���ʉ���炷.���ʉ��̎�ނ͂����Ŕ��ʂ���.
    /// </summary>
    /// <param name="shanten"></param>
    /// <param name="latestShanten"></param>
    private void PlaySE(int shanten, int latestShanten)
    {
        //�V�����e�����������ȂƂ��͉������Ȃ�.
        if (shanten < -1)
        {
            return;
        }
        AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        //�V�����e�������������Ƃ�.�����v���Ă��Ȃ��Ƃ�.�܂��͖����Ȓl����L���Ȓl�ɂȂ����ꍇ.
        if (shanten < latestShanten && shanten != 0 || latestShanten < -1)
        {
            AudioClip ac_countDown = audioLibrary.GetAudioClip(AudioType.Shanten_CountDown);
            if (_audioSource != null && ac_countDown != null)
            {
                _audioSource.PlayOneShot(ac_countDown);
            }
            Addressables.Release(audioLibrary);
        }
        //�V�����e�������������Ƃ�.���������v���������ꂽ�ꍇ������.
        else if (shanten > latestShanten && latestShanten != 0)
        {
            AudioClip ac_countUp = audioLibrary.GetAudioClip(AudioType.Shanten_CountUp);
            if (_audioSource != null && ac_countUp != null)
            {
                _audioSource.PlayOneShot(ac_countUp);
            }
            Addressables.Release(audioLibrary);
        }

    }
}
