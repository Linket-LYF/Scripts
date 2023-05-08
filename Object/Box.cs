using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator ani;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ani.SetBool("Open", true);
            EventHandler.CallGetSoundDeatil(soundName.BoxOpen);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ani.SetBool("Open", false);
            EventHandler.CallGetSoundDeatil(soundName.BoxClose);
            //ani.SetBool("Close", true);

        }
    }
}
