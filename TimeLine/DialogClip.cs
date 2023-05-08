using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public class DialogClip : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps => ClipCaps.None;
    //类只是一个模板工厂用模板来生成playable，实际上类里面的东西绑定在clip里面赋值的，所以要创建一个实例进行赋值，用赋好值的模板来进行工厂生产
    public DialogBehaviour template = new DialogBehaviour();

    //把资产实例化
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        //通过工厂把类放入clip，然后再由clip生成一个playable
        var playable = ScriptPlayable<DialogBehaviour>.Create(graph, template);
        return playable;
    }
}
