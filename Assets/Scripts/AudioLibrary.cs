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

    //«‘‚É‚µ‚ÄAudioType‚©‚çAudioClip‚ğæ“¾‚Å‚«‚é‚æ‚¤‚É‚·‚é.
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
    /// audioType‚©‚çaudioClip‚ğæ“¾‚·‚éŠÖ”
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(AudioType type)
    {
        //«‘‚ª‰Šú‰»‚³‚ê‚Ä‚È‚©‚Á‚½‚ç‰Šú‰»‚·‚é.
        if (_audioDictionary == null)
        {
            _audioDictionary = BuildAudioEntryDuctionary();
        }

        AudioEntry audioEntry;
        if(_audioDictionary.TryGetValue((int)type,out audioEntry))
        {
            AudioClip audioClip;
            //audioClip‚ª“o˜^‚³‚ê‚Ä‚È‚¢ê‡‚Ínull‚ğ•Ô‚·
            if (audioEntry.clips.Length <= 0)
            {
                Debug.LogError("audioClip‚ª“o˜^‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ : "+((AudioType)type).ToString());
                return null;
            }
            //audioClip‚ª•¡”‚ ‚é‚Æ‚«‚Íƒ‰ƒ“ƒ_ƒ€‚É•Ô‚·.
            audioClip = audioEntry.clips[UnityEngine.Random.Range(0, audioEntry.clips.Length)];
            return audioClip;
        }

        return audioEntry.clips[0];
    }

    //audioClip‚ÆAudioType‚ğ•R‚Ã‚¯‚é\‘¢‘Ì.clips‚ª•¡”‚È‚Ì‚Íƒ‰ƒ“ƒ_ƒ€Ä¶—p.
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
