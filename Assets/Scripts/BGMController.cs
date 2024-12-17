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
    private bool _isChanging = false;   //割り込み防止用フラグ.
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
        //和了時は無音.
        if (shanten == -1)
        {
            StartCoroutine(FadeOut(_fadeOutTime));
        }
        //聴牌時は専用のBGM.
        else if (shanten == 0)
        {
            StartCoroutine(ChangeBGM(_audioLibrary.GetAudioClip(AudioType.BGM_Tenpai)));
        }
        //1~7シャンテン(有効な手牌)のときは通常のBGM.
        else if (shanten > 0)
        {
            StartCoroutine(ChangeBGM(_audioLibrary.GetAudioClip(AudioType.BGM_Default)));
        }
        //無効な手牌の時は特に何もしない.

    }

    private IEnumerator ChangeBGM(AudioClip ac)
    {
        if (_audioSource.clip == ac) yield break;
        if (_audioSource.isPlaying)
        {
            //音量が0になるまで待つ.
            IEnumerator enumerator = FadeOut(_fadeOutTime);
            yield return enumerator;
        }
        _audioSource.clip = ac;
        _audioSource.Play();
    }

    //音量を徐々に小さくして停止させるコルーチン.
    private IEnumerator FadeOut(float time)
    {
        _isChanging = true;
        yield return null;  //ないはずだけどwhileに入らなかった時用.
        if (time == 0)  //0除算回避用.
        {
            time = 0.01f;   //適当に小さめの値.
        }

        float originalVolume = _audioSource.volume;
        float value = 1;
        //valueを1~0までtime秒かけて変化させる.
        while (_audioSource.volume > 0)
        {
            value -= Time.deltaTime / time;
            //元の音量をvalue倍して範囲制限して適用.
            float newVolume = Mathf.Clamp01(originalVolume * value);
            _audioSource.volume = newVolume;
            yield return null;
        }
        _audioSource.Stop();
        _isChanging = false;
    }
}
