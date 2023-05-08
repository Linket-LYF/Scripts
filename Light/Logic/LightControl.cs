using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class LightControl : MonoBehaviour
{
    public Light_SO light_SO;
    private Light2D currentLight;
    private LightDetails currentLightDetail;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        currentLight = GetComponent<Light2D>();
    }
    //实际切换灯光
    public void ChangeLift(Season season, LightShift shift, float time)
    {
        currentLightDetail = light_SO.FindLight(season, shift);
        if (time < Settings.lightChangeDuration)
        {
            //颜色的差值除以变化时间=每分变化率    *现在的出来时间，把颜色变回到这个时间本应的颜色
            var colorOffst = (currentLightDetail.color - currentLight.color) / Settings.lightChangeDuration * time;
            Debug.Log("color" + colorOffst);
            currentLight.color += colorOffst;
            //目标，
            DOTween.To(() => currentLight.color, c => currentLight.color = c, currentLightDetail.color, Settings.lightChangeDuration - time);
            DOTween.To(() => currentLight.intensity, i => currentLight.intensity = i, currentLightDetail.intensity, Settings.lightChangeDuration - time);

        }
        else
        {
            currentLight.color = currentLightDetail.color;
            currentLight.intensity = currentLightDetail.intensity;
        }

    }
}
