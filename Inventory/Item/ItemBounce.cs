using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFarm.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;
        private BoxCollider2D coll;
        public float gravity = -3.5f;
        private bool isGround;
        private float distance;
        private Vector2 direction;

        private Vector3 targetPos;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            Bounce();
        }
        public void InitBounceItemm(Vector3 pos, Vector2 dir)
        {
            coll.enabled = false;
            direction = dir;
            targetPos = pos;
            //transform是阴影的坐标
            distance = Vector3.Distance(pos, transform.position);
            spriteTrans.position += Vector3.up * 1.32f;
        }
        private void Bounce()
        {
            //如果阴影位置和鼠标点击位置不一致移动阴影位置，如果不在地面上，y不断下降
            isGround = spriteTrans.position.y <= transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position += (Vector3)direction * distance * -gravity * Time.deltaTime;
            }
            if (!isGround)
            {
                spriteTrans.position += Vector3.up * gravity * Time.deltaTime;
            }
            else
            {
                spriteTrans.position = transform.position;
                coll.enabled = true;

            }
        }
    }
}

