using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Otumn.Juggling
{
    public class ScoreManager : Entity
    {
        [Header("Timer display")]
        [SerializeField] private float beginningTime = 180f;
        [SerializeField] private Text timerText;
        [SerializeField] private Animator timerAnim;
        [SerializeField] private Text addedTimeText;
        [SerializeField] private Text removedTimeText;

        private bool shouldCalculateTime = true;

        protected override void Start()
        {
            base.Start();
            GameManager.state.TimeLeft = beginningTime;
        }

        protected override void Update()
        {
            base.Update();
            CalculateTimer();
        }

        private void CalculateTimer()
        {
            if(shouldCalculateTime)
            {
                float timeLeft = GameManager.state.TimeLeft;
                string text;
                if (timeLeft < 10f)
                {
                    text = "0" + timeLeft.ToString("F0");
                }
                else
                {
                    text = timeLeft.ToString("F0");
                }

                timerText.text = text;
                timeLeft -= Time.deltaTime;
                GameManager.state.TimeLeft = timeLeft;
                if (GameManager.state.TimeLeft < 0)
                {
                    timerText.text = "00";
                    GameManager.state.CallOnGameOver();
                }
            }
        }

        public override void OnBallJuggle()
        {
            base.OnBallJuggle();
            GameManager.state.CurrentScore += 1;
        }

        public override void OnTimeAdded(float addedTime)
        {
            base.OnTimeAdded(addedTime);
            addedTimeText.text = "+" + addedTime.ToString("F0");
            timerAnim.SetTrigger("added");
        }

        public override void OnTimeRemoved(float removedTime)
        {
            base.OnTimeRemoved(removedTime);
            removedTimeText.text = "-" + removedTime.ToString("F0");
            timerAnim.SetTrigger("removed");
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            shouldCalculateTime = false;
            GameManager.state.PlayerDatas.PlayerHighScore = GameManager.state.CurrentScore;
            GameManager.state.SavePlayerData();
        }
    }
}
