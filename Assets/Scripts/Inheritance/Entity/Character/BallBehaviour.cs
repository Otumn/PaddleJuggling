using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class BallBehaviour : Entity
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] private float timeLost = 15f;
        [SerializeField] private float downVelocityMultiplier = 1.2f;
        [SerializeField] private float upVelocityMultiplier = 0.95f;
        [SerializeField] private float maxVelocity = 25f;
        [Header("Feedbakcs")]
        [SerializeField] private FeedBack paddleBounceFeedback;
        [SerializeField] private FeedBack coinObtained;


        private Vector3 velocityBuffer;
        private bool hasBegunCollision = false;
        private bool canBeHit = true;
        private float angleSpacing;

        #region Monobehaviour Callbacks

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.state.BallsInGame++;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            VelocityManagement();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            velocityBuffer = body.velocity;
        }

        private void OnCollisionEnter(Collision coll)
        {
            if (coll.gameObject.GetComponent<Wall>() != null)
            {
                WallReflect(velocityBuffer, coll.contacts[0].normal);
            }
            if (coll.gameObject.GetComponent<Racket>() != null)
            {
                Debug.Log("Collision enter from " + gameObject.name);
                Debug.Break();
                Bounce(coll.gameObject.GetComponent<Racket>(), coll.contacts[0].normal, coll.contacts[0].point, coll.gameObject.GetComponent<Racket>().BounceForce);
            }
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.GetComponent<GameOverGround>() != null)
            {
                GameManager.state.BallsDestroyed++;
                GameManager.state.BallsInGame--;
                GameManager.state.TimeLeft -= timeLost * GameManager.state.BallsDestroyed;
                GameManager.state.CallOnTimeRemoved(timeLost * GameManager.state.BallsDestroyed);
                if (GameManager.state.BallsInGame <= 0)
                {
                    GameManager.state.CallOnGameOver();
                }
                GameObject.Destroy(this.gameObject);
            }

            if (coll.gameObject.GetComponent<CoinBehaviour>() != null)
            {
                coinObtained.transform.position = coll.transform.position;
                coinObtained.Play();
                coll.gameObject.GetComponent<CoinBehaviour>().AddScore();
            }
        }

        private void OnTriggerExit(Collider coll)
        {
            if(coll.GetComponent<Wall>() != null)
            {
                sphereCollider.isTrigger = false;
            }
        }

        #endregion

        #region Entity Callbacks

        public override void OnGameOver()
        {
            base.OnGameOver();
            Destroy(gameObject);
        }

        #endregion

        public void Impulse(Vector3 dir, float force)
        {
            body.AddForce(dir.normalized * force, ForceMode.Impulse);
        }

        private void Bounce(Racket racket, Vector3 direction, Vector3 contactPoint, float force)
        {
            transform.position += Vector3.up * 0.01f;
            body.velocity = Vector3.zero;
            float distFromContact = Vector3.Distance(racket.CollUpMiddle, contactPoint);
            float maxPossibleDist = Vector3.Distance(racket.CollUpMiddle, new Vector3(racket.Coll.bounds.max.x, racket.Coll.bounds.max.y, 0));
            float ratio = Mathf.InverseLerp(0, maxPossibleDist, distFromContact);
            if(Vector3.Distance(racket.transform.right * (racket.Coll.size.x * 0.5f) + racket.transform.position, contactPoint) < Vector3.Distance(-racket.transform.right * (racket.Coll.size.x * 0.5f) + racket.transform.position, contactPoint))
            {
                //to the right
                direction = (Quaternion.AngleAxis(racket.AddedZRotCurve.Evaluate(ratio) * -racket.MaxAddedZRot, Vector3.forward) * direction).normalized;
            }
            else
            {
                //to the left
                direction = (Quaternion.AngleAxis(racket.AddedZRotCurve.Evaluate(ratio) * racket.MaxAddedZRot, Vector3.forward) * direction).normalized;
            }

            body.AddForce((direction.normalized * force) + Vector3.up * Mathf.Abs(body.velocity.y * 0.2f), ForceMode.Impulse);
            paddleBounceFeedback.Play();
            GameManager.state.CallOnBallJuggle();
        }

        private void WallReflect(Vector3 entryVector, Vector3 normal)
        {
            body.velocity = Vector3.Reflect(entryVector, normal);
        }

        private void VelocityManagement()
        {
            if(body.velocity.y < 0)
            {
                body.velocity = new Vector3(body.velocity.x, body.velocity.y * downVelocityMultiplier, 0);
            }
            if(body.velocity.y > 0)
            {
                body.velocity = new Vector3(body.velocity.x, body.velocity.y * upVelocityMultiplier, 0);
            }

            if(body.velocity.magnitude > maxVelocity)
            {
                body.velocity = body.velocity.normalized * maxVelocity;
            }
        }

    }
}
