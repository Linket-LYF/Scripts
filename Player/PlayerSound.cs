using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public void PlaySound()
    {
        EventHandler.CallGetSoundDeatil(soundName.FootStep);
    }
}
