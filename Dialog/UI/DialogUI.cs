using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace MyFarm.Dialog
{
    public class DialogUI : MonoBehaviour
    {
        public GameObject ui;
        public Text text;
        public Image headImage;
        private void OnEnable()
        {
            //EventHandler.ShowDialogEvent += OnShowDialogEvent;
            EventHandler.ShowDialogEvent += OnShowDialogEvent;
        }

        private void OnDisable()
        {
            EventHandler.ShowDialogEvent -= OnShowDialogEvent;
        }

        private void OnShowDialogEvent(DialogPiece result)
        {
            StartCoroutine(ShowDialog(result));
            //StartCoroutine("ShowDialog", result);
        }
        IEnumerator ShowDialog(DialogPiece dialogPiece)
        {
            if (dialogPiece != null)
            {
                dialogPiece.isDone = false;
                ui.SetActive(true);
                text.text = string.Empty;
                if (dialogPiece.isShowSprite)
                {
                    headImage.sprite = dialogPiece.faceSprite;
                }
                yield return text.DOText(dialogPiece.dialogText, 1.5f).WaitForCompletion();
                dialogPiece.isDone = true;
            }
            else
            {
                ui.SetActive(false);
                yield break;
            }
        }

    }
}
