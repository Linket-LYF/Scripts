using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using MyFarm.Dialog;
[System.Serializable]
public class DialogBehaviour : PlayableBehaviour
{
    private PlayableDirector dir;
    public DialogPiece dialogPiece;
    public override void OnPlayableCreate(Playable playable)
    {
        //playable是自定义的运行时的对象，通过对象得到图，然后解析图返回图用于解析 ExposedReference 的表。
        //由于不挂在物体上，这样写可以找到自己属于那一个导演
        dir = playable.GetGraph().GetResolver() as PlayableDirector;
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        EventHandler.CallShowDialogEvent(dialogPiece);
        if (Application.isPlaying)
        {
            if (dialogPiece.hasToPause)
            {
                //暂停timeline
                TimeLineManager.Instance.PauseTL(dir);
            }
            else
            {
                EventHandler.CallShowDialogEvent(null);
            }
        }
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (Application.isPlaying)
        {
            TimeLineManager.Instance.isDone = dialogPiece.isDone;
        }
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        EventHandler.CallShowDialogEvent(null);
    }
    public override void OnGraphStart(Playable playable)
    {
        EventHandler.CallPauseGame(GameState.Pause);
    }
    public override void OnGraphStop(Playable playable)
    {
        EventHandler.CallPauseGame(GameState.GamePlay);
    }
}
