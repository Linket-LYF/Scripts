using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("音乐数据")]
    public SoundDataList_SO soundDataList_SO;
    public SceneSoundList sceneSoundList;
    [Header("AudioSource")]
    public AudioSource ambientMusic;
    public AudioSource gameMusic;
    //播放音乐协程
    public Coroutine soundRoutine;
    [Header("AudioMixer")]
    public AudioMixer audioMixer;
    [Header("SnapShots")]
    public AudioMixerSnapshot ambientSnapShot;
    public AudioMixerSnapshot normalSnapShot;
    public AudioMixerSnapshot muteSnapShot;

    private void OnEnable()
    {
        EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
        EventHandler.GetSoundDeatil += OnGetSoundDeatil;
        EventHandler.EndGameEvent += OnEndGameEvent;
    }

    private void OnEndGameEvent()
    {
        if (soundRoutine != null)
            StopCoroutine(soundRoutine);
        muteSnapShot.TransitionTo(1f);
    }

    private void OnGetSoundDeatil(soundName soundName)
    {
        SoundDetails soundDetails = soundDataList_SO.FindSoundDetail(soundName);
        if (soundDetails != null)
            EventHandler.CallInitSoundEffect(soundDetails);
    }

    private void OnDisable()
    {
        EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
        EventHandler.GetSoundDeatil -= OnGetSoundDeatil;
        EventHandler.EndGameEvent -= OnEndGameEvent;
    }
    private void OnAfterLoadSceneEvent()
    {
        //获取当前场景
        string currentScene = SceneManager.GetActiveScene().name;
        //根据当前场景找到对应SO
        SceneSound sceneSound = sceneSoundList.FindSceneSound(currentScene);
        if (sceneSound != null)
        {
            //根据当前SO找到detail
            SoundDetails ambient = soundDataList_SO.FindSoundDetail(sceneSound.ambient);

            SoundDetails game = soundDataList_SO.FindSoundDetail(sceneSound.music);

            if (soundRoutine != null)
            {
                StopCoroutine(soundRoutine);

            }

            soundRoutine = StartCoroutine(PlaySound(ambient, game));

        }

    }
    //协程播放音乐
    private IEnumerator PlaySound(SoundDetails soundDetails, SoundDetails game)
    {
        if (soundDetails != null && game != null)
        {
            PlayAmientSoundClip(soundDetails, 1f);
            yield return new WaitForSeconds(Random.Range(3f, 9f));
            PlayGameSoundClip(game, 5f);
        }
    }
    //播放背景音乐
    private void PlayGameSoundClip(SoundDetails soundDetails, float time)
    {
        audioMixer.SetFloat("MusicVolume", TransferDB(soundDetails.soundVolume));
        gameMusic.clip = soundDetails.soundClip;
        if (gameMusic.isActiveAndEnabled)
            gameMusic.Play();
        normalSnapShot.TransitionTo(time);
    }
    //播放环境音乐
    private void PlayAmientSoundClip(SoundDetails soundDetails, float time)
    {
        audioMixer.SetFloat("AmbientVolume", TransferDB(soundDetails.soundVolume));
        ambientMusic.clip = soundDetails.soundClip;
        if (ambientMusic.isActiveAndEnabled)
            ambientMusic.Play();
        ambientSnapShot.TransitionTo(time);
    }
    //分贝转换
    private float TransferDB(float amount)
    {
        return (amount * 100 - 80);
    }
    //设置音量
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", (value * 100 - 80));
    }

}
