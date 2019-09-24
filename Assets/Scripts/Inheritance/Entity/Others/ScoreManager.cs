using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Otumn.Juggling
{
    public class ScoreManager : Entity
    {
        [Header("Timer display")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Animator timerAnim;
        [SerializeField] private Text addedScoreText;
        [SerializeField] private Text removedScoreText;

        protected override void Start()
        {
            base.Start();
            scoreText.text = "0";
        }

        public override void OnBallJuggle()
        {
            base.OnBallJuggle();
            UpdateScore(1);
        }

        public override void OnBonusScoreAdded(float addedScore)
        {
            base.OnBonusScoreAdded(addedScore);
            UpdateScore(addedScore);
        }

        private void UpdateScore(float addedScore)
        {
            addedScoreText.text = "+" + addedScore.ToString("F0");
            timerAnim.SetTrigger("added");
            GameManager.state.CurrentScore += addedScore;
            scoreText.text = GameManager.state.CurrentScore.ToString("F0");
        }

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            GameManager.state.BallsDestroyed++;
            GameManager.state.BallsInGame--;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            GameManager.state.PlayerDatas.PlayerHighScore = GameManager.state.CurrentScore;
            GameManager.state.SavePlayerData();
        }
    }
}
