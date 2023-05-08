using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class TimeUI : MonoBehaviour
{
    public RectTransform dayNight;
    public RectTransform clockParent;
    public Image seasonImage;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;
    public Sprite[] seasonSprite;
    private List<GameObject> clockBlocks = new List<GameObject>();
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        for (int i = 0; i < clockParent.childCount; i++)
        {
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameHourEvent += OnGameHourEvent;
    }

    private void OnGameHourEvent(int hour, int day, int month, int year, Season season)
    {
        dateText.text = year + "年" + month.ToString("00") + "月" + day.ToString("00") + "日";
        seasonImage.sprite = seasonSprite[(int)season];
        SwitchHourImage(hour);
        SwitchDayNight(hour);
    }

    private void OnGameMinuteEvent(int minute, int hour, int day)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameHourEvent -= OnGameHourEvent;
    }
    //切换小时显示UI
    private void SwitchHourImage(int hour)
    {
        int index = hour / 4;
        if (index == 0)
        {
            foreach (var item in clockBlocks)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < clockBlocks.Count; i++)
            {
                if (i < index + 1)
                {
                    clockBlocks[i].SetActive(true);
                }
                else
                {
                    clockBlocks[i].SetActive(false);
                }
            }
        }
    }
    private void SwitchDayNight(int hour)
    {
        dayNight.DORotate(new Vector3(0, 0, hour * 15 - 90), 1, RotateMode.Fast);
    }

}
