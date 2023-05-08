using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManager : Singleton<MenuManager>
{
    // Start is called before the first frame update
    private GameObject menuCanvas;
    public GameObject menuPrefab;
    public Button pauseBtn;
    public GameObject pausePanel;
    public Slider musicSlider;
    public GameObject tipsPanel;
    public GameObject tipsPrefab;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        pauseBtn.onClick.AddListener(Pause);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
    }
    private void OnEnable()
    {
        EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
    }



    private void OnDisable()
    {
        EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
    }

    private void Start()
    {
        menuCanvas = GameObject.FindGameObjectWithTag("Menu");
        Instantiate(menuPrefab, menuCanvas.transform);

    }

    private void OnAfterLoadSceneEvent()
    {
        if (menuCanvas.transform.childCount > 0)
        {
            Destroy(menuCanvas.transform.GetChild(0).gameObject);

        }
    }
    public void Pause()
    {
        bool isOpen = pausePanel.activeInHierarchy;
        if (isOpen)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Close()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void ReturnMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(BackMenu());
    }
    private IEnumerator BackMenu()
    {
        pausePanel.SetActive(false);

        EventHandler.CallEndGameEvent();

        yield return new WaitForSeconds(1f);

        Instantiate(menuPrefab, menuCanvas.transform);

    }
    public void ShowTips(string text)
    {
        tipsPanel.SetActive(true);
        var tipsItem = Instantiate(tipsPrefab, tipsPanel.transform);
        tipsItem.GetComponent<TipsUI>().SetText(text);
    }
}
