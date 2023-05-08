using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AddBinding : MonoBehaviour
{
    public Animator ani;
    private PlayableDirector playableDirector;
    Dictionary<string, Object> Name_BindObj = new Dictionary<string, Object>();

    // Start is called before the first frame update
    void Start()
    {
        // ani = FindObjectOfType<Player>().GetComponent<Animator>();
        // playableDirector = GetComponent<PlayableDirector>();
        // //playableDirector.SetGenericBinding(ani, gameObject);

        // //playableDirector.SetGenericBinding(ani,)
        // foreach (PlayableBinding item in playableDirector.playableAsset.outputs)
        // {
        //     Debug.LogError("item.streamName=" + item.streamName);
        //     if (!Name_BindObj.TryGetValue(item.streamName, out var obj))
        //     {
        //         var gameObj = GameObject.Find(item.streamName);
        //         Name_BindObj.Add(item.streamName, item.sourceObject);
        //     }
        //     if (playableDirector.GetGenericBinding(item.sourceObject) == null)
        //     {
        //         Debug.LogError("already have binding, name=" + item.streamName);
        //     }
        //     else
        //     {
        //         Debug.LogError("轨道中绑定丢失，轨道名字是Error null binding name=" + item.streamName);

        //     }
        //     if (item.streamName.Equals("Markers"))
        //     {
        //         continue;
        //     }

    }

    // Update is called once per frame

}
