using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Otumn.Juggling
{
    public class UIManager : Entity
    {
        [SerializeField] private GameObject[] allUiCanvas;
        [SerializeField] private GameObject gameOver;

        public override void OnGameOver()
        {
            base.OnGameOver();
            gameOver.SetActive(true);
        }

        public void LoadScene(int index)
        {
            GameManager.state = new GameState();
            SceneManager.LoadScene(index);
        }

        public void ShowMenu(GameObject canvas)
        {
            for (int i = 0; i < allUiCanvas.Length; i++)
            {
                allUiCanvas[i].SetActive(false);
            }

            canvas.SetActive(true);
        }
    }
}
