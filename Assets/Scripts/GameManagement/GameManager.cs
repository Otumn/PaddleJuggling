using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Otumn.Juggling
{
    public class GameManager : MonoBehaviour
    {
        public static GameState state = new GameState();

        private void Start()
        {
            GameManager.state.LoadPlayerData();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < state.Entities.Count; i++)
                {
                    Debug.Log(state.Entities[i].name);
                }
            }
        }

        [ContextMenu("Delete save")]
        public void EraseSave()
        {
            state.ErasePlayerData();
        }
    }

    public class GameState
    {
        private PlayerData playerDatas;
        private List<Entity> entities = new List<Entity>();
        private List<VirtualCameraShakeTarget> shakeTargets = new List<VirtualCameraShakeTarget>();
        private List<VirtualCameraZoomTarget> zoomTargets = new List<VirtualCameraZoomTarget>();
        private float paddleVelocity = 0f;
        private float currentScore = 0f;
        private float timeLostOnBallDestroyed = 0f;
        private int ballsInGame = 0;
        private int ballsDestroyed = 0;

        #region Entity registration

        public void RegisterEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void UnregisterEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion

        #region Camera shake registration

        public void RegisterShakeTarget(VirtualCameraShakeTarget target)
        {
            shakeTargets.Add(target);
        }

        public void UnregisterShakeTarget(VirtualCameraShakeTarget target)
        {
            shakeTargets.Remove(target);
        }

        #endregion

        #region Camera zoom registration

        public void RegisterZoomTarget(VirtualCameraZoomTarget target)
        {
            zoomTargets.Add(target);
        }

        public void UnregisterZoomTarget(VirtualCameraZoomTarget target)
        {
            zoomTargets.Remove(target);
        }

        #endregion

        #region Entity callbacks

        public void CallOnBallJuggle()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallJuggle();
            }
        }

        public void CallOnScoreAdded(float addedTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBonusScoreAdded(addedTime);
            }
        }

        public void CallOnBallDestroyed()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallDestroyed();
            }
        }

        public void CallOnGameOver()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnGameOver();
            }
        }

        #endregion

        #region Saving

        public void SavePlayerData()
        {
            string path = Path.Combine(Application.persistentDataPath, "PlayerData.otjg");
            string jsonString = JsonUtility.ToJson(playerDatas);

            using (StreamWriter streamWriter = File.CreateText(path))
            {
                streamWriter.Write(jsonString);
            }

        }

        public void LoadPlayerData()
        {
            string path = Path.Combine(Application.persistentDataPath, "PlayerData.otjg");

            if (File.Exists(path))
            {
                using (StreamReader streamReader = File.OpenText(path))
                {
                    string jsonString = streamReader.ReadToEnd();
                    playerDatas = JsonUtility.FromJson<PlayerData>(jsonString);
                }
            }
            else
            {
                Debug.Log("No playerData found, creating one...");
                playerDatas = new PlayerData();
                SavePlayerData();
            }
        }

        public void ErasePlayerData()
        {
            string path = Path.Combine(Application.persistentDataPath, "PlayerData.otjg");

            if (File.Exists(path))
            {
                File.Delete(path);
                playerDatas = null;
            }
        }

        #endregion

        public List<VirtualCameraShakeTarget> ShakeTargets { get => shakeTargets; }
        public List<VirtualCameraZoomTarget> ZoomTargets { get => zoomTargets;  }
        public float CurrentScore { get => currentScore; set => currentScore = value; }
        public int BallsInGame { get => ballsInGame; set => ballsInGame = value; }
        public PlayerData PlayerDatas { get => playerDatas; }
        public float PaddleVelocity { get => paddleVelocity; set => paddleVelocity = value; }
        public int BallsDestroyed { get => ballsDestroyed; set => ballsDestroyed = value; }
        public float TimeLostOnBallDestroyed { get => timeLostOnBallDestroyed; set => timeLostOnBallDestroyed = value; }
        public List<Entity> Entities { get => entities; }
    }

    [System.Serializable]
    public class PlayerData
    {
        [SerializeField] private float playerHighScore = 0f;
        [SerializeField] private float playerGolds = 0f;

        [SerializeField] private string playerBallPrefab;
        [SerializeField] private string playerRacketPrefab;

        public float PlayerHighScore { get => playerHighScore; set => playerHighScore = value; }
        public float PlayerGolds { get => playerGolds; set => playerGolds = value; }
        public string PlayerBallPrefab { get => playerBallPrefab; set => playerBallPrefab = value; }
        public string PlayerRacketPrefab { get => playerRacketPrefab; set => playerRacketPrefab = value; }
    }
}
