using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFarm.Transition
{
    public class Teleport : MonoBehaviour
    {
        public string SceneTogo;
        public Vector3 positonTogo;

        /// <summary>
        /// Sent when another object enters a trigger collider attached to this
        /// object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(transform.gameObject.name + 1);
                EventHandler.CallTransitionEvent(SceneTogo, positonTogo);
            }
        }
    }
}

