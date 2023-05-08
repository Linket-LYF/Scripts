using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader faders = other.GetComponentInChildren<ItemFader>();
        if (faders != null)
        {
            faders.FadeOut();

        }


    }
    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader faders = other.GetComponent<ItemFader>();
        if (faders != null)
        {
            faders.FadeIn();

        }
    }
}
