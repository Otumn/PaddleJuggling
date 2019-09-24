using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class Entity : MonoBehaviour
    {
        #region MonoBehaviour callbacks

        protected virtual void Awake()
        {

        }

        protected virtual void OnEnable()
        {
            GameManager.state.RegisterEntity(this);
        }

        protected virtual void OnDisable()
        {
            GameManager.state.UnregisterEntity(this);
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        #endregion

        #region Entity callbacks

        public virtual void OnBallJuggle()
        {

        }

        public virtual void OnBonusScoreAdded(float addedTime)
        {

        }

        public virtual void OnBallDestroyed()
        {

        }

        public virtual void OnGameOver()
        {

        }

        #endregion
    }
}
