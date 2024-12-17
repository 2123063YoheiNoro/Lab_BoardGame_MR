using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[RequireComponent(typeof(AudioSource))]
public class BGMController : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioLibrary _audioLibrary;
    private Coroutine _latestCoroutine;
    private bool _isChanging = false;   //���荞�ݖh�~�p�t���O.
    private float _fadeOutTime = 1;

    [SerializeField] HandShantenModel shantenModel;
    private int shanten;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        AudioClip ac_default = _audioLibrary.GetAudioClip(AudioType.BGM_Default);
        ChangeBGM(ac_default);
    }

    private void Update()
    {
        if (_isChanging) return;
        shanten = shantenModel.shanten.Value;
        //�a�����͖���.
        if (shanten == -1)
        {
            StartCoroutine(FadeOut(_fadeOutTime));
        }
        //���v���͐�p��BGM.
        else if (shanten == 0)
        {
            StartCoroutine(ChangeBGM(_audioLibrary.GetAudioClip(AudioType.BGM_Tenpai)));
        }
        //1~7�V�����e��(�L���Ȏ�v)�̂Ƃ��͒ʏ��BGM.
        else if (shanten > 0)
        {
            StartCoroutine(ChangeBGM(_audioLibrary.GetAudioClip(AudioType.BGM_Default)));
        }
        //�����Ȏ�v�̎��͓��ɉ������Ȃ�.

    }

    private IEnumerator ChangeBGM(AudioClip ac)
    {
        if (_audioSource.clip == ac) yield break;
        if (_audioSource.isPlaying)
        {
            //���ʂ�0�ɂȂ�܂ő҂�.
            IEnumerator enumerator = FadeOut(_fadeOutTime);
            yield return enumerator;
        }
        _audioSource.clip = ac;
        _audioSource.Play();
    }

    //���ʂ����X�ɏ��������Ē�~������R���[�`��.
    private IEnumerator FadeOut(float time)
    {
        _isChanging = true;
        yield return null;  //�Ȃ��͂�������while�ɓ���Ȃ��������p.
        if (time == 0)  //0���Z���p.
        {
            time = 0.01f;   //�K���ɏ����߂̒l.
        }

        float originalVolume = _audioSource.volume;
        float value = 1;
        //value��1~0�܂�time�b�����ĕω�������.
        while (_audioSource.volume > 0)
        {
            value -= Time.deltaTime / time;
            //���̉��ʂ�value�{���Ĕ͈͐������ēK�p.
            float newVolume = Mathf.Clamp01(originalVolume * value);
            _audioSource.volume = newVolume;
            yield return null;
        }
        _audioSource.Stop();
        _isChanging = false;
    }
}
