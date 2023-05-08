using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsUI : MonoBehaviour
{
    public TextMeshProUGUI tipsText;
    public void SetText(string text)
    {
        tipsText.text = text;
        Destroy(this, 1.5f);
    }
    private void OnDestroy()
    {
        //销毁自己后，如果父物体只剩下自己，那么父物体也销毁
        if (transform.parent.childCount == 1)
        {
            transform.parent.gameObject.SetActive(false);
        }
        Destroy(this.gameObject);
    }
}
