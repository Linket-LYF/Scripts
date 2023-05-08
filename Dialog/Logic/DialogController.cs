using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyFarm.Dialog
{
    [RequireComponent(typeof(NPCMovement))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogController : MonoBehaviour
    {
        private NPCMovement nPCMovement => GetComponent<NPCMovement>();
        public UnityEvent OnFinishEvent;
        public List<DialogPiece> dialogPieces = new List<DialogPiece>();
        private Stack<DialogPiece> DialogST;
        private bool canTalk;
        private bool isTalking;
        private void Awake()
        {
            FullDialogST();
        }

        private void Update()
        {
            if (canTalk && Input.GetKeyDown(KeyCode.Space) && !isTalking)
            {
                StartCoroutine("DialogRoutine");
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {

                if (!nPCMovement.isNPCMove && nPCMovement.interactable)
                {
                    canTalk = true;
                    Debug.Log("cantalk" + canTalk);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!nPCMovement.isMoving && nPCMovement.interactable)
                {
                    Debug.Log("fffffffffffffff");
                    canTalk = false;
                }
            }
        }
        //构建对话堆栈
        private void FullDialogST()
        {
            DialogST = new Stack<DialogPiece>();
            for (int i = dialogPieces.Count - 1; i > -1; i--)
            {
                dialogPieces[i].isDone = false;
                DialogST.Push(dialogPieces[i]);
            }
        }
        //协程开启对话
        IEnumerator DialogRoutine()
        {
            isTalking = true;
            if (DialogST.TryPop(out DialogPiece result))
            {
                //传到UI显示对话
                EventHandler.CallShowDialogEvent(result);
                EventHandler.CallPauseGame(GameState.Pause);
                yield return new WaitUntil(() => result.isDone);
                isTalking = false;
            }
            else
            {
                EventHandler.CallPauseGame(GameState.GamePlay);
                EventHandler.CallShowDialogEvent(null);
                FullDialogST();
                isTalking = false;
                if (OnFinishEvent != null)
                {
                    OnFinishEvent.Invoke();
                    canTalk = false;
                }

            }
        }
    }


}
