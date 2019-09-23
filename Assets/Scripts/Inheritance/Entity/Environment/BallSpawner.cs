using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Otumn.Juggling
{
    public class BallSpawner : Environment
    {
        [SerializeField] private GameObject ballPrefab; //TODO : get that from save
        [SerializeField] private float startImpulseForce = 5f;
        [SerializeField] private float timeBeforeSpawn = 5f;
        [SerializeField] private Text timerText;

        private float timerInc = 0f;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Update()
        {
            base.Update();
            Timer();
        }

        private void Timer()
        {
            timerInc += Time.deltaTime;
            timerText.text = Mathf.Round(timeBeforeSpawn - timerInc).ToString("F0");
            if(timerInc > timeBeforeSpawn)
            {
                GameObject ball = GameObject.Instantiate(ballPrefab, transform.position, Quaternion.identity);
                Vector3 dir = (new Vector3(0, 12, 0) - transform.position).normalized;
                ball.GetComponent<BallBehaviour>().Impulse(dir, startImpulseForce);
                GameObject.Destroy(gameObject);
            }
        }

    }
}
