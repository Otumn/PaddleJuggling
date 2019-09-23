using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class EventManager : Entity
    {
        // TODO : get those from saves
        [Header("Coins")]
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private CoinSpawnPoint[] coinsSpawns;
        [SerializeField] private float spawnCD = 10f;
        [Header("Balls")]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private float impulseForce = 5f;
        [SerializeField] private int maxBallsinGame = 5;
        [SerializeField] private float ballSpawnMinY = 6f;
        [SerializeField] private float ballSpawnMaxY = 8f;
        [SerializeField] private float ballSpawnMaxX = 2f;
        [SerializeField] private float scoreThreshHold = 100f;

        private float addedScore = 0f;
        private float lastScore = 0f;

        protected override void Start()
        {
            base.Start();
            SpawnNewBall(ballSpawnMaxX, ballSpawnMinY, ballSpawnMaxY);
            StartCoroutine(CoinSpawnTimer());
        }

        protected override void Update()
        {
            base.Update();
            
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            AddedScoreCalculation();
            lastScore = GameManager.state.CurrentScore;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            StopAllCoroutines();
        }

        private IEnumerator CoinSpawnTimer()
        {
            yield return new WaitForSeconds(spawnCD);
            for (int i = 0; i < coinsSpawns.Length; i++)
            {
                int c = Random.Range(0, coinsSpawns.Length);
                if (coinsSpawns[c].Available)
                {
                    coinsSpawns[c].Available = false;
                    GameObject coin = GameObject.Instantiate(coinPrefab, coinsSpawns[c].Point.position, Quaternion.identity);
                    coin.GetComponent<CoinBehaviour>().SpawnPoint = coinsSpawns[c];
                    break;
                }
            }
            StartCoroutine(CoinSpawnTimer());
        }

        private void AddedScoreCalculation()
        {
            if(GameManager.state.BallsInGame < maxBallsinGame)
            {
                addedScore += GameManager.state.CurrentScore - lastScore;
                //Debug.Log("last : " + lastScore + " added : " + addedScore);
            }
            if(addedScore > scoreThreshHold)
            {
                SpawnNewBall(ballSpawnMaxX, ballSpawnMinY, ballSpawnMaxY);
                addedScore = 0;
            }
            
        }

        private void SpawnNewBall(float maxX, float minY, float maxY)
        {
            int c = Random.Range(0, 2);
            float x = 0f;
            if(c == 0)
            {
                x = -maxX;
            }
            else
            {
                x = maxX;
            }

            Vector3 pos = new Vector3(x, Random.Range(minY, maxY));
            GameObject ball = GameObject.Instantiate(ballPrefab, pos, Quaternion.identity);
            Vector3 dir = (new Vector3(0, pos.y + 5f, 0) - pos).normalized;
            ball.GetComponent<BallBehaviour>().Impulse(dir, impulseForce);
        }

    }

    [System.Serializable]
    public class CoinSpawnPoint
    {
        [SerializeField] private string name;
        [SerializeField] private Transform point;
        private bool available = true;

        public bool Available { get => available; set => available = value; }
        public Transform Point { get => point; set => point = value; }
    }
}
