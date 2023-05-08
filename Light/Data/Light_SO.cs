using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Light_SO", menuName = "Light/Light_SO")]
public class Light_SO : ScriptableObject
{
    public List<LightDetails> lightDetailsList;
    public LightDetails FindLight(Season season, LightShift shift)
    {
        return lightDetailsList.Find(l => l.season == season && l.lightShift == shift);
    }
}
[System.Serializable]
public class LightDetails
{
    public Season season;
    public Color color;
    public float intensity;
    public LightShift lightShift;
}

