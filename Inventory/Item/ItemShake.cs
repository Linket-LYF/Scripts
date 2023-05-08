using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShake : MonoBehaviour
{
    private bool isShake;
    private WaitForSeconds time = new WaitForSeconds(0.02f);
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShake)
        {
            if (other.transform.position.x > gameObject.transform.position.x)
            {
                StartCoroutine(ShakeItemLeft());
            }
            else
            {
                StartCoroutine(ShakeItemRight());
            }
            EventHandler.CallGetSoundDeatil(soundName.Rustle);
        }
    }
    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isShake)
        {
            if (other.transform.position.x < gameObject.transform.position.x)
            {
                StartCoroutine(ShakeItemLeft());
            }
            else
            {
                StartCoroutine(ShakeItemRight());
            }
        }
    }
    IEnumerator ShakeItemLeft()
    {
        isShake = true;
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0, 0, 2);
            yield return time;
        }
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0, 0, -2);
            yield return time;
        }
        gameObject.transform.GetChild(0).Rotate(0, 0, 2);
        yield return time;
        isShake = false;
    }
    IEnumerator ShakeItemRight()
    {
        isShake = true;
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0, 0, -2);
            yield return time;
        }
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.GetChild(0).Rotate(0, 0, 2);
            yield return time;
        }
        gameObject.transform.GetChild(0).Rotate(0, 0, -2);
        yield return time;
        isShake = false;
    }
}
