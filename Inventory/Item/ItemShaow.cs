using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFarm.Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemShaow : MonoBehaviour
    {
        public SpriteRenderer itemSprite;
        private SpriteRenderer shaowSprite;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            shaowSprite = GetComponent<SpriteRenderer>();
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            shaowSprite.sprite = itemSprite.sprite;
            shaowSprite.color = new Color(0, 0, 0, 3f);
        }

    }
}

