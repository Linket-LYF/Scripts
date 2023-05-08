using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class StartTimeLine : MonoBehaviour
{
    private PlayableDirector playableDirector;
    private GameObject TL;
    //public Animator ani;

    //private Dictionary<string,PlayableBinding> dic= new Dictionary<string, PlayableBinding>
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        TL = GameObject.FindGameObjectWithTag("StoreTL");
        playableDirector = GameObject.FindGameObjectWithTag("StoreTL").GetComponent<PlayableDirector>();
        TL.SetActive(false);


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && TimeLineManager.Instance.storeTL == false)
        {

            TL.SetActive(true);
            TimeLineManager.Instance.storeTL = true;
            playableDirector.Play();


        }
    }
}
