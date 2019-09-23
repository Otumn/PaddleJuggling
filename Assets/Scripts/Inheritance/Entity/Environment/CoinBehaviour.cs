using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class CoinBehaviour : Entity
    {
        [SerializeField] private float addedSeconds = 25f;
        [SerializeField] private float rotSpeed = 250f;

        private CoinSpawnPoint spawnPoint;

        protected override void Update()
        {
            base.Update();
            Rotate();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            Destroy(gameObject);
        }

        private void Rotate()
        {
            transform.rotation *= Quaternion.AngleAxis(rotSpeed * Time.deltaTime, Vector3.up);
        }

        public void AddScore()
        {
            spawnPoint.Available = true;
            GameManager.state.TimeLeft += addedSeconds;
            GameManager.state.CallOnTimeAdded(addedSeconds);
            GameObject.Destroy(gameObject);
        }


        public CoinSpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

    }
}
