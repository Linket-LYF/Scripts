using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyFarm.Save;
public class TimeManager : Singleton<TimeManager>, Isavealbe
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    private Season gameSeason = Season.春天;
    private int monthInSeason = 3;
    public bool gameClockPause;
    private float tikTime;
    public TimeSpan GameTime => new TimeSpan(gameHour, gameMinute, gameSecond);

    public string GUID => GetComponent<DataGUID>().guid;

    private float timeDuration;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    // protected override void Awake()
    // {
    //     base.Awake();
    //     InitGameTime();

    // }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    protected void OnEnable()
    {
        EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.StartNewGame += OnStartNewGame;
        EventHandler.EndGameEvent += OnEndGameEvent;
        EventHandler.OtherPlayerJoin += OnOtherPlayerJoin;
        EventHandler.OtherPlayerJoinFinish += OnOtherPlayerJoinFinish;
    }

    private void OnEndGameEvent()
    {
        gameClockPause = true;
    }

    private void OnStartNewGame(int obj)
    {
        InitGameTime();
        gameClockPause = false;
    }

    private void OnAfterLoadSceneEvent()
    {
        gameClockPause = false;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    protected void OnDisable()
    {
        EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.StartNewGame -= OnStartNewGame;
        EventHandler.EndGameEvent -= OnEndGameEvent;
        EventHandler.OtherPlayerJoin -= OnOtherPlayerJoin;
        EventHandler.OtherPlayerJoinFinish -= OnOtherPlayerJoinFinish;

    }
    private void OnOtherPlayerJoin(int obj)
    {
        gameClockPause = false;
    }

    private void OnOtherPlayerJoinFinish()
    {
        gameClockPause = true;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        gameClockPause = true;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        // EventHandler.CallGameMinuteEvent(gameMinute, gameHour, gameDay);
        // EventHandler.CallGameHourEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        // EventHandler.CallChangeLightEven(gameSeason, GetCurrentLightShift(), timeDuration);
        Isavealbe save = this;
        save.RegisterSaveble();
        gameClockPause = true;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;
            if (tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }
        if (Input.GetKey(KeyCode.T))
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gameDay++;
            EventHandler.CallGameDayEvent(gameDay, gameSeason);
            EventHandler.CallGameHourEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        }


    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {

    }
    //初始化游戏时间
    private void InitGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 6;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2022;
        gameSeason = Season.春天;
    }

    //更新游戏时间
    private void UpdateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;
            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;
                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;
                    if (gameDay > Settings.dayHold)
                    {
                        gameMonth++;

                        gameDay = 1;
                        if (gameMonth > 12)
                        {
                            gameMonth = 1;
                        }
                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
                            if (seasonNumber > Settings.seasonHold)
                            {
                                gameYear++;
                                seasonNumber = 0;
                            }
                            gameSeason = (Season)seasonNumber;
                        }


                    }
                    EventHandler.CallGameDayEvent(gameDay, gameSeason);
                }
                EventHandler.CallGameHourEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour, gameDay);
            //切换游戏灯光
            EventHandler.CallChangeLightEven(gameSeason, GetCurrentLightShift(), timeDuration);

        }

    }
    private LightShift GetCurrentLightShift()
    {
        if (GameTime >= Settings.morningTime && GameTime < Settings.nightTime)
        {
            timeDuration = (float)(GameTime - Settings.morningTime).TotalMinutes;

            return LightShift.Morning;
        }
        if (GameTime < Settings.morningTime || GameTime >= Settings.nightTime)
        {
            timeDuration = Math.Abs((float)(GameTime - Settings.nightTime).TotalMinutes);

            return LightShift.Night;
        }
        return LightShift.Morning;
    }

    public SaveData SaveGame()
    {
        SaveData saveData = new SaveData();
        saveData.timeDic = new Dictionary<string, int>();
        saveData.timeDic.Add("gameYear", gameYear);
        saveData.timeDic.Add("gameSeason", (int)gameSeason);
        saveData.timeDic.Add("gameMonth", gameMonth);
        saveData.timeDic.Add("gameDay", gameDay);
        saveData.timeDic.Add("gameHour", gameHour);
        saveData.timeDic.Add("gameMinute", gameMinute);
        saveData.timeDic.Add("gameSecond", gameSecond);
        return saveData;
    }

    public void LoadGame(SaveData saveData)
    {
        gameYear = saveData.timeDic["gameYear"];
        gameSeason = (Season)saveData.timeDic["gameSeason"];
        gameMonth = saveData.timeDic["gameMonth"];
        gameDay = saveData.timeDic["gameDay"];
        gameHour = saveData.timeDic["gameHour"];
        gameMinute = saveData.timeDic["gameMinute"];
        gameSecond = saveData.timeDic["gameSecond"];
    }
}
