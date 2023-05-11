using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyFarm.Save;
namespace MyFarm.Transition
{
    public class TransitionManager : MonoBehaviour, Isavealbe
    {
        [SceneName]
        public string startSceneName = string.Empty;
        public static string currentSceneName = "";
        private CanvasGroup fadeCanvasGroup;

        private bool isFade;

        public string GUID => GetComponent<DataGUID>().guid;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }


        private void Start()
        {
            Isavealbe save = this;
            save.RegisterSaveble();
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            //yield return LoadSceneSetActive(startSceneName);
            //.CallAfterLoadSceneEvent();
        }

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
            EventHandler.StartNewGame += OnStartNewGame;
            EventHandler.EndGameEvent += OnEndGameEvent;
        }

        private void OnEndGameEvent()
        {
            StartCoroutine(UnloadScene());
        }

        private void OnStartNewGame()
        {
            StartCoroutine(LoadNewSaveDataScene(startSceneName));
        }

        private void OnTransitionEvent(string sceneName, Vector3 pos)
        {
            if (!isFade)
                StartCoroutine(Transition(sceneName, pos));
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
            EventHandler.StartNewGame -= OnStartNewGame;
            EventHandler.EndGameEvent -= OnEndGameEvent;
        }
        //加载场景并激活
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
        }
        //转换场景
        private IEnumerator Transition(string sceneName, Vector3 targetPositon)
        {
            //Debug.Log($"当前场景{SceneManager.GetActiveScene().name},要去的场景{sceneName}");
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);

            yield return LoadSceneSetActive(sceneName);

            EventHandler.CallMoveToPositon((targetPositon));

            EventHandler.CallAfterLoadSceneEvent();
            yield return Fade(0);
        }
        //淡入淡出场景
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
            isFade = false;
            fadeCanvasGroup.blocksRaycasts = false;
        }
        private IEnumerator LoadNewSaveDataScene(string sceneName)
        {
            //TODO 重点改造对象
            yield return Fade(1f);
            if (SceneManager.GetActiveScene().name != "MainScene")//游戏过程中加载另一个场景
            {
                EventHandler.CallBeforeSceneUnloadEvent();
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
            yield return LoadSceneSetActive(sceneName);
            currentSceneName = sceneName;
            EventHandler.CallCollectMapInfo();
            EventHandler.CallAfterLoadSceneEvent();
            yield return Fade(0f);
            //将新的存档发送给服务器
            SaveLoadManager.Instance.Save();
        }
        private IEnumerator LoadSaveDataScene(string sceneName)
        {
            //TODO 重点改造对象
            yield return Fade(1f);
            if (SceneManager.GetActiveScene().name != "MainScene")//游戏过程中加载另一个场景
            {
                EventHandler.CallBeforeSceneUnloadEvent();
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
            yield return LoadSceneSetActive(sceneName);
            currentSceneName = sceneName;
            EventHandler.CallAfterLoadSceneEvent();
            yield return Fade(0f);
        }
        private IEnumerator UnloadScene()
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1f);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            yield return Fade(0f);
        }
        public SaveGameC2SMsg SaveGame()
        {
            SaveGameC2SMsg saveData = new();
            saveData.DataSceneName = SceneManager.GetActiveScene().name;
            return saveData;
        }

        public void LoadGame(SaveGameC2SMsg saveData)
        {
            //加载游戏进度场景
            StartCoroutine(LoadSaveDataScene(saveData.DataSceneName));
        }
    }
}

