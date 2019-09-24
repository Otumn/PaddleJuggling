using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class PlayerController : Character
    {
        [SerializeField] private float maxY = 7f;
        [SerializeField] private float maxX = 3f;
        [SerializeField] private Racket racket;
        [SerializeField] private float maxRot = 45f;
        [SerializeField] private AnimationCurve rotationCurve;
        [SerializeField] private float yOffSet = 0.5f;

        private Vector3 racketPosTarget;

        private void PositionRestriction()
        {
            if (racket.transform.position.y > maxY)
            {
                racket.transform.position = new Vector3(racket.transform.position.x, maxY, 0);
            }
        }

        private void RotationCalculation()
        {
            float xRatio = Mathf.InverseLerp(0, maxX, Mathf.Abs(racket.transform.position.x));
            float rot = rotationCurve.Evaluate(xRatio) * maxRot;
            if(racket.transform.position.x > 0)
            {
                //right
                racket.transform.rotation = Quaternion.Euler(0, 0, rot);
            }
            else
            {
                //left
                racket.transform.rotation = Quaternion.Euler(0, 0, -rot);
            }
        }

        public void OnTouchBegin(Vector3 pos)
        {
            //racket.gameObject.SetActive(true);
            racket.transform.position = pos + new Vector3(0, yOffSet, 0);
            PositionRestriction();
        }

        public void OnTouchHeld(Vector3 pos)
        {
            racket.transform.position = pos + new Vector3(0, yOffSet, 0);
            PositionRestriction();
        }

        public void OnTouchReleased(Vector3 pos)
        {
            //racket.gameObject.SetActive(false);
            racket.Body.velocity = Vector3.zero;
        }
    }
}
