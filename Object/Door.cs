using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator ani;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ani.SetBool("isOpen", true);
            EventHandler.CallGetSoundDeatil(soundName.DoorOpen);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ani.SetBool("isOpen", false);
            ani.SetBool("isClose", true);
            EventHandler.CallGetSoundDeatil(soundName.DoorClose);
        }
    }
}
