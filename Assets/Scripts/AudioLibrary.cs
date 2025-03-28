using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName ="AudioLibrary",menuName = "ScriptableObjects/CreateAudioLibraryAsset")]
public class AudioLibrary : ScriptableObject
{
    public AudioEntry[] audioEntries;
    private Dictionary<int, AudioEntry> _audioDictionary;

    //辞書にしてAudioTypeからAudioClipを取得できるようにする.
    public Dictionary<int,AudioEntry> BuildAudioEntryDuctionary()
    {
        Dictionary<int, AudioEntry> dictionary = new Dictionary<int, AudioEntry>();
        for(int i = 0; i < this.audioEntries.Length; i++)
        {
            dictionary.Add((int)this.audioEntries[i].type, this.audioEntries[i]);
        }
        return dictionary;
    }
    /// <summary>
    /// audioTypeからaudioClipを取得する関数
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(AudioType type)
    {
        //辞書が初期化されてなかったら初期化する.
        if (_audioDictionary == null)
        {
            _audioDictionary = BuildAudioEntryDuctionary();
        }

        AudioEntry audioEntry;
        if(_audioDictionary.TryGetValue((int)type,out audioEntry))
        {
            AudioClip audioClip;
            //audioClipが登録されてない場合はnullを返す
            if (audioEntry.clips.Length <= 0)
            {
                Debug.LogError("audioClipが登録されていません : "+((AudioType)type).ToString());
                return null;
            }
            //audioClipが複数あるときはランダムに返す.
            audioClip = audioEntry.clips[UnityEngine.Random.Range(0, audioEntry.clips.Length)];
            return audioClip;
        }

        return audioEntry.clips[0];
    }

    //audioClipとAudioTypeを紐づける構造体.clipsが複数なのはランダム再生用.
    [Serializable]
    public struct AudioEntry
    {
        public AudioType type;
        public AudioClip[] clips;
        public float volume;

        public AudioEntry(AudioType t,AudioClip[] c)
        {
            this.type = t;
            this.clips = c;
            this.volume = 1.0f;
        }
        public AudioEntry(AudioType t, AudioClip[] c,float v)
        {
            this.type = t;
            this.clips = c;
            this.volume = v;
        }
    }
}
