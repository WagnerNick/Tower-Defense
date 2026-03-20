using System.Collections.Generic;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    [SerializeField]
    private MusicGroup[] musicGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    public void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (MusicGroup musicGroup in musicGroups)
        {
            soundDictionary[musicGroup.name] = musicGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }
}

[System.Serializable]
public struct MusicGroup
{
    public string name;
    public List<AudioClip> audioClips;
}
