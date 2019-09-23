using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class TouchPlane : Entity, ITouchable
    {
        [SerializeField] private PlayerController player;

        public override void OnGameOver()
        {
            base.OnGameOver();
            gameObject.SetActive(false);
        }

        public void OnTouchBegin(Vector3 touchPos)
        {
            player.OnTouchBegin(touchPos);
        }

        public void OnTouchHeld(Vector3 touchPos)
        {
            player.OnTouchHeld(touchPos);
        }

        public void OnTouchReleased(Vector3 touchPos)
        {
            player.OnTouchReleased(touchPos);
        }


    }
}
