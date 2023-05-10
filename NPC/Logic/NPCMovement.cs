using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.Astar;
using UnityEngine.SceneManagement;
using System;
using MyFarm.Save;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class NPCMovement : MonoBehaviour, Isavealbe
{
    public ScheduleDetailsList_SO schedule_SO;
    private SortedSet<ScheduleDetails> scheduleSet;
    private ScheduleDetails currentSchedule;
    //临时存储信息
    public string currentScene;
    public string targetScene;
    private Vector3Int currentGridPos;
    private Vector3Int targetGridPos;
    private Grid grid;
    private Vector3Int nextGridPos;//npc下个移动网格位置

    public string startScene { set => currentScene = value; }
    [Header("移动速度")]
    public float speed = 2f;
    private float minSpeed = 1f;
    private float maxSpeed = 3f;
    private Vector2 dir;//移动方向
    public bool isMoving;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    private Animator ani;
    private Stack<MovementStep> movementSteps;
    private bool isInit;
    public bool isNPCMove;//npc是否在移动
    public bool interactable;
    private bool isLoadScene;
    private bool isFristLoad;
    private Season curSeason;
    private AnimationClip stopAnimationClip;
    private bool canPlayStopAnimation;
    private float playStopAnimationTime;
    //动画切换
    public AnimationClip blankAnimationClip;
    private AnimatorOverrideController aniOver;
    private TimeSpan timeSpan => TimeManager.Instance.GameTime;
    private Coroutine npcMove;

    public string GUID => GetComponent<DataGUID>().guid;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        movementSteps = new Stack<MovementStep>();
        aniOver = new AnimatorOverrideController(ani.runtimeAnimatorController);
        ani.runtimeAnimatorController = aniOver;
        scheduleSet = new SortedSet<ScheduleDetails>();
        foreach (var sch in schedule_SO.scheduleDetailsList)
        {
            scheduleSet.Add(sch);
        }
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        Isavealbe save = this;
        save.RegisterSaveble();
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.EndGameEvent += OnEndGameEvent;
        EventHandler.StartNewGame += OnStartNewGame;
    }

    private void OnStartNewGame()
    {
        isInit = false;
        isFristLoad = true;
    }

    private void OnEndGameEvent()
    {
        isLoadScene = false;
        isNPCMove = false;
        if (npcMove != null)
        {
            StopCoroutine(npcMove);
        }
    }

    private void OnBeforeSceneUnloadEvent()
    {
        isLoadScene = false;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (isLoadScene)
            SwitchAnimation();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        playStopAnimationTime -= Time.fixedDeltaTime;
        if (isLoadScene)
        {
            Movement();
            //Debug.Log("isload");
        }

    }

    private void OnAfterLoadSceneEvent()
    {
        grid = FindObjectOfType<Grid>();
        CheckVisiable();
        if (isInit == false)
        {
            InitNPC();
            isInit = true;
        }
        isLoadScene = true;
        if (!isFristLoad)
        {
            currentGridPos = grid.WorldToCell(transform.position);
            var schedule = new ScheduleDetails(0, 0, 0, 0, curSeason, targetScene, (Vector2Int)targetGridPos, stopAnimationClip, interactable);
            BuildPaht(schedule);
            isFristLoad = true;
        }
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.EndGameEvent -= OnEndGameEvent;
        EventHandler.StartNewGame -= OnStartNewGame;
    }

    private void OnGameMinuteEvent(int minute, int hour, int day)
    {
        curSeason = Season.春天;
        int time = (hour * 100) + minute;
        ScheduleDetails match = null;
        foreach (var sch in scheduleSet)
        {
            if (sch.Time == time)
            {
                if (sch.day != day && sch.day != 0)
                    continue;
                match = sch;
            }
            else if (sch.Time > time)
            {
                break;
            }
        }
        if (match != null)
        {
            BuildPaht(match);

        }

    }

    private void CheckVisiable()
    {

        if (currentScene == SceneManager.GetActiveScene().name)
        {
            SetActiveInSCene();
        }
        else
            SetFalseInSCene();
    }
    //初始化NPC
    private void InitNPC()
    {
        targetScene = currentScene;
        currentGridPos = grid.WorldToCell(transform.position);
        transform.position = new Vector3(currentGridPos.x + 0.5f, currentGridPos.y + 0.5f, 0);
        targetGridPos = currentGridPos;

    }
    private void Movement()
    {
        if (!isNPCMove)
        {
            // Debug.Log("movementSteps.Count " + movementSteps.Count);
            if (movementSteps.Count > 0)
            {

                //根据栈弹出的信息来构建移动
                MovementStep step = movementSteps.Pop();
                currentScene = step.sceneName;
                CheckVisiable();
                nextGridPos = (Vector3Int)step.gridPos;
                TimeSpan newNextTime = new TimeSpan(step.hour, step.minute, step.second);
                MoveToTargetPos(nextGridPos, newNextTime);
            }
            else if (!isMoving && canPlayStopAnimation)
            {
                StartCoroutine("SetStopAnimation");
            }
        }

    }
    private void MoveToTargetPos(Vector3Int nextGridPos, TimeSpan nextTime)
    {
        npcMove = StartCoroutine(NextMove(nextGridPos, nextTime));
    }
    IEnumerator NextMove(Vector3Int nextGridPos, TimeSpan nextTime)
    {
        isNPCMove = true;
        Vector3 nextWorldGridPos = CellPosToWorld(nextGridPos);
        //如果还有时间来移动
        if (nextTime > timeSpan)
        {

            //可用来移动的时间
            float time = (float)(nextTime.TotalSeconds - timeSpan.TotalSeconds);
            //可移动的距离
            float distance = Vector3.Distance(transform.position, nextWorldGridPos);
            //实际移动速度
            float speed = Math.Max(minSpeed, (distance / time / Settings.secondThreshold));
            Debug.Log("nextTime:" + nextTime + " timeSpan:" + timeSpan + " time:" + time + " distance:" + distance + " speed:" + speed);
            if (speed <= maxSpeed)
            {
                while (Vector3.Distance(transform.position, nextWorldGridPos) > 0.05f)
                {
                    dir = (nextGridPos - transform.position).normalized;
                    //当前坐标到下一个坐标的位移
                    Vector2 posOffset = new Vector2(dir.x * speed * Time.fixedDeltaTime, dir.y * speed * Time.fixedDeltaTime);
                    rb.MovePosition(rb.position + posOffset);
                    Debug.Log("MoveTo" + rb.position + posOffset);
                    yield return new WaitForFixedUpdate();
                }
            }

        }
        rb.position = nextWorldGridPos;
        currentGridPos = nextGridPos;
        nextGridPos = currentGridPos;
        isNPCMove = false;

    }
    //网格坐标转换世界坐标
    private Vector3 CellPosToWorld(Vector3Int nextGridPos)
    {
        Vector3 worldPos = grid.CellToWorld(nextGridPos);
        return new Vector3(worldPos.x + 0.5f, worldPos.y + 0.5f);
    }
    //根据schedule构建路径
    private void BuildPaht(ScheduleDetails scheduleDetails)
    {
        movementSteps.Clear();
        currentSchedule = scheduleDetails;
        targetScene = scheduleDetails.targetScene;
        stopAnimationClip = scheduleDetails.animationClip;
        targetGridPos = (Vector3Int)scheduleDetails.targetGridPos;
        this.interactable = scheduleDetails.interactable;

        //同场景移动
        //print(currentScene);
        if (scheduleDetails.targetScene == currentScene)
        {
            Astar.Instance.BuildPath(scheduleDetails.targetScene, (Vector2Int)currentGridPos, scheduleDetails.targetGridPos, movementSteps);

        }
        //跨场景移动
        else if (scheduleDetails.targetScene != currentScene)
        {

            SceneRoute sceneRoute = NPCManager.Instance.FindRoute(currentScene, scheduleDetails.targetScene);
            if (sceneRoute != null)
            {
                for (int i = 0; i < sceneRoute.scenePathList.Count; i++)
                {
                    Vector2Int fromPos;
                    Vector2Int toPos;
                    ScenePath currentPath = sceneRoute.scenePathList[i];
                    if (currentPath.fromPos.x >= 9999)
                        fromPos = (Vector2Int)currentGridPos;
                    else
                        fromPos = currentPath.fromPos;
                    if (currentPath.toPos.x >= 9999)
                        toPos = scheduleDetails.targetGridPos;
                    else
                        toPos = currentPath.toPos;
                    Astar.Instance.BuildPath(currentPath.sceneName, fromPos, toPos, movementSteps);
                }
            }

        }
        if (movementSteps.Count > 1)
        {
            //更新每一步时间戳
            UpdateTimeOnPath();
        }
    }
    private void UpdateTimeOnPath()
    {
        MovementStep previousStep = null;
        TimeSpan currentTime = timeSpan;
        foreach (MovementStep step in movementSteps)
        {
            if (previousStep == null)
                previousStep = step;
            step.hour = currentTime.Hours;
            step.minute = currentTime.Minutes;
            step.second = currentTime.Seconds;

            TimeSpan gridMoveNextTime;
            if (DirOrInclined(step, previousStep))
            {
                gridMoveNextTime = new TimeSpan(0, 0, (int)(1.41 / speed / Settings.secondThreshold));
            }
            else
            {
                gridMoveNextTime = new TimeSpan(0, 0, (int)(1 / speed / Settings.secondThreshold));
            }
            //累加获得下一步的时间戳
            currentTime = currentTime.Add(gridMoveNextTime);
            //循环下一步
            previousStep = step;
        }


    }
    //判断是否走斜方向
    private bool DirOrInclined(MovementStep nextMove, MovementStep previousMove)
    {
        return (nextMove.gridPos.x != previousMove.gridPos.x && nextMove.gridPos.y != previousMove.gridPos.y);
    }
    //npc显示情况
    private void SetActiveInSCene()
    {
        spriteRenderer.enabled = true;
        coll.enabled = true;
    }
    private void SetFalseInSCene()
    {
        spriteRenderer.enabled = false;
        coll.enabled = false;
    }
    //人物动画切换
    private void SwitchAnimation()
    {
        isMoving = transform.position != CellPosToWorld(targetGridPos);

        ani.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            ani.SetBool("Exit", true);
            ani.SetFloat("DirX", dir.x);
            ani.SetFloat("DirY", dir.y);
        }
        else
        {
            ani.SetBool("Exit", false);
        }
    }
    private IEnumerator SetStopAnimation()
    {
        ani.SetFloat("DirX", 0);
        ani.SetFloat("DirY", -1);
        playStopAnimationTime = 5f;
        if (stopAnimationClip != null)
        {
            aniOver[blankAnimationClip] = stopAnimationClip;
            ani.SetBool("EventAnimation", true);
            yield return null;
            ani.SetBool("EventAnimation", false);
        }
        else
        {
            aniOver[stopAnimationClip] = blankAnimationClip;
            ani.SetBool("EventAnimation", false);
        }

    }

    public SaveGameC2SMsg SaveGame()
    {
        SaveGameC2SMsg saveData = new();
        saveData.CharacterPos.Add("targetGridPos", new Vector3Msg { X = targetGridPos.x, Y = targetGridPos.y, Z = targetGridPos.z });
        saveData.CharacterPos.Add("currentGridPos", new Vector3Msg { X = transform.position.x, Y = transform.position.y, Z = transform.position.z });
        saveData.DataSceneName = currentScene;
        // saveData.targetScene = this.targetScene;
        saveData.TiemDic.Add("currentSeason", (int)curSeason);
        if (stopAnimationClip != null)
        {
            saveData.AnimState = stopAnimationClip.GetInstanceID();
        }
        return saveData;
    }

    public void LoadGame(SaveGameC2SMsg saveData)
    {
        isFristLoad = false;
        isInit = true;
        currentScene = saveData.DataSceneName;
        //targetScene = saveData.targetScene;
        Vector3 pos = ProtoHelper.Vector3Msg2Vector3(saveData.CharacterPos[this.name]);
        Vector3Int gridPos = (Vector3Int)ProtoHelper.Vector3Msg2Vector2Int(saveData.CharacterPos["targetGridPos"]);
        transform.position = pos;
        targetGridPos = gridPos;

        if (saveData.AnimState != 0)
        {
            this.stopAnimationClip = Resources.InstanceIDToObject(saveData.AnimState) as AnimationClip;
        }
        // this.interactable = saveData.interactalbe;
        isInit = true;
        this.curSeason = (Season)saveData.TiemDic["currentSeason"];
    }
}
