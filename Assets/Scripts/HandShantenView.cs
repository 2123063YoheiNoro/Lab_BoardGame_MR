using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;

public class HandShantenView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioSource _audioSource;

    private int _latestShanten = int.MaxValue; //シャンテン数変化検知用.
    private void Start()
    {
        text.text = "";
    }
    public void UpdateText(int shanten)
    {
        Debug.Log("update shanten : " + shanten);
        string result = "";
        //シャンテン数が-1より下は不正な値 牌が多すぎるときに渡される.
        if (shanten < -1)
        {
            result = "---";
        }
        else
        {
            if (shanten == -1)  //和了時.
            {
                result = "和了";
            }
            else if (shanten == 0)  //聴牌時.
            {
                result = "聴牌";
            }
            else
            {
                //コードベタ書き気持ち悪いので後でちゃんと管理する.
                result = shanten.ToString() + "シャンテン";
            }
        }
        text.text = result;

        //効果音を鳴らす.
        PlaySE(shanten, _latestShanten);

        //比較が終わったので最新の値に書き換えておく.
        _latestShanten = shanten;
    }

    /// <summary>
    /// 効果音を鳴らす.効果音の種類はここで判別する.
    /// </summary>
    /// <param name="shanten"></param>
    /// <param name="latestShanten"></param>
    private void PlaySE(int shanten, int latestShanten)
    {
        //シャンテン数が無効なときは何もしない.
        if (shanten < -1)
        {
            return;
        }
        AudioLibrary audioLibrary = Addressables.LoadAssetAsync<AudioLibrary>("AudioLibrary").WaitForCompletion();
        //シャンテン数が減ったとき.かつ聴牌していないとき.または無効な値から有効な値になった場合.
        if (shanten < latestShanten && shanten != 0 || latestShanten < -1)
        {
            AudioClip ac_countDown = audioLibrary.GetAudioClip(AudioType.Shanten_CountDown);
            if (_audioSource != null && ac_countDown != null)
            {
                _audioSource.PlayOneShot(ac_countDown);
            }
            Addressables.Release(audioLibrary);
        }
        //シャンテン数が増えたとき.ただし聴牌が解除された場合を除く.
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
