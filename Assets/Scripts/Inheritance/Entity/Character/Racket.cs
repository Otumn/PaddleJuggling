using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class Racket : Entity
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float bounceForce = 3f;
        [SerializeField] private float velocityRatio = 3f;
        [SerializeField] private BoxCollider coll;
        [SerializeField] private AnimationCurve addedZRotCurve;
        [SerializeField] private float maxAddedZRot = 25f;

        private float vel = 0f;
        private Vector3 lastPos;

        protected override void Start()
        {
            base.Start();
            lastPos = transform.position;
        }

        protected override void Update()
        {
            base.Update();
            CalculateVelcocity();
            Debug.DrawLine(transform.position, CollUpMiddle, Color.blue);
            Debug.DrawLine(transform.position, transform.right * (coll.size.x * 0.5f) + transform.position, Color.yellow);
            Debug.DrawLine(transform.position, -transform.right * (coll.size.x * 0.5f) + transform.position, Color.green);
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            gameObject.SetActive(false);
        }



        private void CalculateVelcocity()
        {
            vel = Vector3.Distance(transform.position, lastPos);
            GameManager.state.PaddleVelocity = vel;
            lastPos = transform.position;
        }

        public float BounceForce { get => bounceForce; }
        public AnimationCurve AddedZRotCurve { get => addedZRotCurve; }
        public BoxCollider Coll { get => coll; }
        public float MaxAddedZRot { get => maxAddedZRot; set => maxAddedZRot = value; }

        public Vector3 CollUpMiddle
        {
            get
            {
                return (transform.up * (coll.size.y * 0.5f)) + transform.position;
            }
        }

        public float BounceVelocityForce
        {
            get
            {
                return bounceForce + vel * velocityRatio;
            }
        }

        public Rigidbody Body { get => body; }
    }
}
