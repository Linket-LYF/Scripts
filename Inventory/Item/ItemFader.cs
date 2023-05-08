using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //逐渐恢复颜色
    public void FadeIn()
    {
        Color targetcolor = new Color(1, 1, 1, 1);
        spriteRenderer.DOColor(targetcolor, Settings.itemFadeDuration);
    }
    //逐渐半透明
    public void FadeOut()
    {
        Color targetcolor = new Color(1, 1, 1, Settings.targetAlpha);
        spriteRenderer.DOColor(targetcolor, Settings.itemFadeDuration);
    }
}
