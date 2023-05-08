using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineManager : Singleton<TimeLineManager>
{
    public PlayableDirector startDir;
    private PlayableDirector currentDir;
    public bool isDone;
    private bool isPause;
    public bool storeTL = false;
    protected override void Awake()
    {
        base.Awake();
        currentDir = startDir;
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        // currentDir.played += TimeLinePlay;
        // currentDir.played += TimeLinePause;
        EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
    }

    private void OnAfterLoadSceneEvent()
    {
        currentDir = FindObjectOfType<PlayableDirector>();
        // if (currentDir != null)
        //     currentDir.Play();
    }

    // private void TimeLinePause(PlayableDirector dir)
    // {
    //     if (dir != null)
    //     {
    //         EventHandler.CallPauseGame(GameState.GamePlay);

    //     }
    // }

    // private void TimeLinePlay(PlayableDirector dir)
    // {
    //     if (dir != null)
    //     {
    //         EventHandler.CallPauseGame(GameState.Pause);
    //     }
    // }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (isPause && Input.GetKeyDown(KeyCode.Space) && isDone)
        {
            isPause = false;
            currentDir.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        }
    }
    public void PauseTL(PlayableDirector dir)
    {
        currentDir = dir;
        currentDir.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        isPause = true;
    }
}



