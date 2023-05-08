using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace MyFarm.Dialog
{
    [System.Serializable]
    public class DialogPiece
    {
        public Sprite faceSprite;
        public bool isShowSprite;
        [TextArea]
        public string dialogText;
        public bool hasToPause;
        public bool isDone;


    }
}
