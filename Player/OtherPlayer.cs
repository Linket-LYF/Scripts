using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.Save;
public class OtherPlayer : MonoBehaviour, Isavealbe
{
    public int playerId = -1;
    private Rigidbody2D rb;
    public float speed;
    public float inputX;
    public float inputY;
    private Vector2 movementInput;
    private Animator ani;
    private bool isMoving;
    public bool isSwitch;
    private float mouseX;
    private float mouseY;
    private bool isUseTool;

    public string GUID => GetComponent<DataGUID>().guid.ToString();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        Isavealbe saveable = this;
        saveable.RegisterSaveble();
    }
    private void OnEnable()
    {
        EventHandler.MoveToPositon += OnMoveToPositon;
        EventHandler.MouseClickEvent += OnMouseClickEvent;
    }

    private void Update()
    {
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnDisable()
    {
        EventHandler.MoveToPositon -= OnMoveToPositon;
    }

    //人物鼠标点击事件
    private void OnMouseClickEvent(Vector3 mousePos, ItemDetails itemDetails)
    {
        //TODO:执行动画
        if (itemDetails.itemType == ItemType.HoeTool)
        {
            mouseX = mousePos.x - transform.position.x;
            mouseY = mousePos.y - (transform.position.y + 0.5f);
            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else
                mouseX = 0;
            StartCoroutine(UseDigToolRoutine(mousePos, itemDetails));
        }
        //浇水动画
        else if (itemDetails.itemType == ItemType.WaterTool)
        {
            mouseX = mousePos.x - transform.position.x;
            mouseY = mousePos.y - (transform.position.y + 0.5f);
            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else
                mouseX = 0;
            StartCoroutine(UseWaterToolRoutine(mousePos, itemDetails));
        }
        //收获动画
        else if (itemDetails.itemType == ItemType.CollectTool)
        {
            mouseX = mousePos.x - transform.position.x;
            mouseY = mousePos.y - (transform.position.y + 0.5f);
            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else
                mouseX = 0;
            StartCoroutine(UseCollectToolRoutine(mousePos, itemDetails));
        }
        //  砍树动画
        else if (itemDetails.itemType == ItemType.ChopTool)
        {
            mouseX = mousePos.x - transform.position.x;
            mouseY = mousePos.y - (transform.position.y + 0.5f);
            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else
                mouseX = 0;
            StartCoroutine(UseChopToolRoutine(mousePos, itemDetails));
        }
        //凿石头动画
        else if (itemDetails.itemType == ItemType.BreakTool)
        {
            mouseX = mousePos.x - transform.position.x;
            mouseY = mousePos.y - (transform.position.y + 0.5f);
            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else
                mouseX = 0;
            StartCoroutine(UseBreakToolRoutine(mousePos, itemDetails));
        }
        //收割动画
        else if (itemDetails.itemType == ItemType.ReapTool)
        {
            mouseX = mousePos.x - transform.position.x;
            mouseY = mousePos.y - (transform.position.y + 0.5f);
            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else
                mouseX = 0;
            StartCoroutine(UseReapToolRoutine(mousePos, itemDetails));
        }
        else
            EventHandler.CallExecuteActionAfteiAnimation(mousePos, itemDetails);
    }
    //挖坑动画协程
    private IEnumerator UseDigToolRoutine(Vector3 mousePos, ItemDetails itemDetails)
    {
        isUseTool = true;
        yield return null;
        ani.SetTrigger("Dig");
        //人物朝向
        ani.SetFloat("InputX", mouseX);
        ani.SetFloat("InputY", mouseY);
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfteiAnimation(mousePos, itemDetails);
        yield return new WaitForSeconds(0.55f);
        isUseTool = false;

    }
    //浇水动画协程
    private IEnumerator UseWaterToolRoutine(Vector3 mousePos, ItemDetails itemDetails)
    {
        isUseTool = true;
        yield return null;
        ani.SetTrigger("Water");
        //人物朝向
        ani.SetFloat("InputX", mouseX);
        ani.SetFloat("InputY", mouseY);
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfteiAnimation(mousePos, itemDetails);
        yield return new WaitForSeconds(0.55f);
        isUseTool = false;

    }
    //收割动画协程
    private IEnumerator UseCollectToolRoutine(Vector3 mousePos, ItemDetails itemDetails)
    {
        isUseTool = true;
        yield return null;
        ani.SetTrigger("Collect");
        //人物朝向
        ani.SetFloat("InputX", mouseX);
        ani.SetFloat("InputY", mouseY);
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfteiAnimation(mousePos, itemDetails);
        yield return new WaitForSeconds(0.55f);
        isUseTool = false;

    }
    //砍树动画协程
    private IEnumerator UseChopToolRoutine(Vector3 mousePos, ItemDetails itemDetails)
    {
        isUseTool = true;
        yield return null;
        ani.SetTrigger("Chop");
        //人物朝向
        ani.SetFloat("InputX", mouseX);
        ani.SetFloat("InputY", mouseY);
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfteiAnimation(mousePos, itemDetails);
        yield return new WaitForSeconds(0.55f);
        isUseTool = false;

    }
    //凿石头协程
    private IEnumerator UseBreakToolRoutine(Vector3 mousePos, ItemDetails itemDetails)
    {
        isUseTool = true;
        yield return null;
        ani.SetTrigger("Break");
        //人物朝向
        ani.SetFloat("InputX", mouseX);
        ani.SetFloat("InputY", mouseY);
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfteiAnimation(mousePos, itemDetails);
        yield return new WaitForSeconds(0.55f);
        isUseTool = false;

    }
    //收割协程
    private IEnumerator UseReapToolRoutine(Vector3 mousePos, ItemDetails itemDetails)
    {
        isUseTool = true;
        yield return null;
        ani.SetTrigger("Harvest");
        //人物朝向
        ani.SetFloat("InputX", mouseX);
        ani.SetFloat("InputY", mouseY);
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfteiAnimation(mousePos, itemDetails);
        yield return new WaitForSeconds(0.55f);
        isUseTool = false;

    }
    private void OnMoveToPositon(Vector3 pos)
    {
        transform.position = pos;
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.deltaTime);

    }
    //动画切换
    private void SwitchAnimation()
    {
        ani.SetBool("isMoving", isMoving);
        ani.SetFloat("MouseX", mouseX);
        ani.SetFloat("MouseY", mouseY);
        if (isMoving)
        {
            ani.SetFloat("InputX", inputX);
            ani.SetFloat("InputY", inputY);
        }
    }

    public SaveData SaveGame()
    {
        SaveData saveData = new SaveData();
        saveData.characterPos = new Dictionary<string, SerializableVector3>();
        saveData.characterPos.Add(this.name, new SerializableVector3(transform.position));
        return saveData;
    }

    public void LoadGame(SaveData saveData)
    {
        var charaPos = saveData.characterPos[this.name].ToVertor3();
        transform.position = charaPos;
    }
}
