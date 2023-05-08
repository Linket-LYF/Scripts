using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBtn : MonoBehaviour
{
    public GameObject loadPanel;
    public InputField inputfield;
    public void LoadGame()
    {
        loadPanel.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
