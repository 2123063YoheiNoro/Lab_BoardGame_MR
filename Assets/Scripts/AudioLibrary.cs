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

    //�����ɂ���AudioType����AudioClip���擾�ł���悤�ɂ���.
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
    /// audioType����audioClip���擾����֐�
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(AudioType type)
    {
        //����������������ĂȂ������珉��������.
        if (_audioDictionary == null)
        {
            _audioDictionary = BuildAudioEntryDuctionary();
        }

        AudioEntry audioEntry;
        if(_audioDictionary.TryGetValue((int)type,out audioEntry))
        {
            AudioClip audioClip;
            //audioClip���o�^����ĂȂ��ꍇ��null��Ԃ�
            if (audioEntry.clips.Length <= 0)
            {
                Debug.LogError("audioClip���o�^����Ă��܂��� : "+((AudioType)type).ToString());
                return null;
            }
            //audioClip����������Ƃ��̓����_���ɕԂ�.
            audioClip = audioEntry.clips[UnityEngine.Random.Range(0, audioEntry.clips.Length)];
            return audioClip;
        }

        return audioEntry.clips[0];
    }

    //audioClip��AudioType��R�Â���\����.clips�������Ȃ̂̓����_���Đ��p.
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
