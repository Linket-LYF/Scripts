using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataList_SO", menuName = "Sound/SoundDataList")]
public class SoundDataList_SO : ScriptableObject
{
    public List<SoundDetails> soundDetailsList;
    public SoundDetails FindSoundDetail(soundName name)
    {
        return soundDetailsList.Find(s => s.soundName == name);
    }
}
[System.Serializable]
public class SoundDetails
{
    public soundName soundName;
    public AudioClip soundClip;
    [Range(0.1f, 1.5f)]
    public float soundPitchMin;
    [Range(0.1f, 1.5f)]
    public float soundPitchMax;
    [Range(0.1f, 1f)]
    public float soundVolume;
}