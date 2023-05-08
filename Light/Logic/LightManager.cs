using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public LightControl[] sceneLights;
    private LightShift currentShift;
    private Season season;
    private float timeDiff = Settings.lightChangeDuration;

    private void OnEnable()
    {
        EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
        EventHandler.ChangeLightEvent += OnChangeLightEvent;
        EventHandler.StartNewGame += OnStartNewGame;
    }

    private void OnStartNewGame(int obj)
    {
        currentShift = LightShift.Morning;
    }

    private void OnAfterLoadSceneEvent()
    {
        sceneLights = FindObjectsOfType<LightControl>();

        foreach (LightControl light in sceneLights)
        {
            //改变灯光
            light.ChangeLift(this.season, currentShift, timeDiff);
        }

    }
    private void OnChangeLightEvent(Season season, LightShift shift, float time)
    {

        this.season = season;
        timeDiff = time;

        if (currentShift != shift)
        {
            currentShift = shift;

            foreach (LightControl light in sceneLights)
            {
                //改变灯光
                light.ChangeLift(this.season, currentShift, timeDiff);
            }

        }
    }

    private void OnDisable()
    {
        EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
        EventHandler.ChangeLightEvent -= OnChangeLightEvent;
        EventHandler.StartNewGame -= OnStartNewGame;

    }


}
