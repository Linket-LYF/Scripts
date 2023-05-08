using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Resolution : MonoBehaviour
{
    public Dropdown dropdown;
    void Start()
    {
        dropdown.onValueChanged.AddListener(ChangeResolution);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ChangeResolution(int index)
    {
        switch (index)
        {
            case 0:

                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:

                Screen.SetResolution(2560, 1440, true);
                break;
            case 2:

                Screen.SetResolution(1366, 768, true);
                break;
            default:
                break;
        }
    }
}
