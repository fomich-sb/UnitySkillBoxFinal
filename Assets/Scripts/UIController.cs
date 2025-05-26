using Fusion;
using NanoSockets;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Zenject.Asteroids;
using Zenject.SpaceFighter;
using static SkillBoxFinal.GameController;

namespace SkillBoxFinal
{
    public class UIController : MonoBehaviour
    {

        [SerializeField] private GameObject StartMenuCanvas;
        [SerializeField] private GameObject LoadingCanvas;
        [SerializeField] private GameObject GameOverCanvas;
        [SerializeField] private GameObject SettingsCanvas;
        [SerializeField] private GameObject SettingsPanelInGame;
        [SerializeField] private GameObject PlayCanvasRoot;

        [SerializeField] private Text MyHealthText;
        [SerializeField] private Text MyArmorText;
        [SerializeField] private Text MyLevelText; 
        [SerializeField] private Text MyScoreText;
        [SerializeField] private Text MyHighDamageBulletsText;

        public Text GameOverLevelText;
        public Text GameOverScoreText;
        public Text GameOverShootingAccuracyText;

        private INetworkPlayer myNetworkPlayer;

        [Inject] private GameController _gameController;

        private void Start()
        {
            _gameController.OnChangeGameStatus += OnChangeGameStatus;
        }

        public void OnChangeGameStatus(GameController.GameStatus gameStatus)
        {
            SettingsCanvas.SetActive(false);

            StartMenuCanvas.SetActive(gameStatus == GameController.GameStatus.MainMenu);
            LoadingCanvas.SetActive(gameStatus == GameController.GameStatus.Loading);
            SettingsPanelInGame.SetActive(gameStatus == GameController.GameStatus.Playing);
            GameOverCanvas.SetActive(gameStatus == GameController.GameStatus.GameOver);
            PlayCanvasRoot.SetActive(gameStatus == GameController.GameStatus.Playing);
            
            if (gameStatus == GameController.GameStatus.Playing)
            {
                Cursor.lockState =  CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

        }
        public void SetMyPlayer(IPlayer player)
        {
            myNetworkPlayer = (player as MonoBehaviour).GetComponent<INetworkPlayer>();
            player.OnDead += OnMyPlayerDead;
            myNetworkPlayer.OnInfoChanged += DisplayMyMainInfo;
        }

        public void DisplayMyMainInfo()
        {
            MyLevelText.text = myNetworkPlayer.Level.ToString();
            MyScoreText.text = myNetworkPlayer.Score.ToString();
            MyHighDamageBulletsText.text = myNetworkPlayer.HighDamageBullets.ToString();
        }

        public void DisplayMyHealthArmor(float health=0, float armor=0)
        {
            MyHealthText.text = Mathf.Ceil(health).ToString();
            MyArmorText.text = Mathf.Ceil(armor).ToString();
        }

        public void OpenSettings()
        {
            if (SettingsCanvas.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                SettingsCanvas.SetActive(true);
                if (_gameController.status == GameController.GameStatus.Playing)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        public void CloseSettings()
        {
            SettingsCanvas.SetActive(false);
            if (_gameController.status == GameStatus.Playing)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnMyPlayerDead()
        {
            if (myNetworkPlayer is not null)
            {
                DisplayGameOverStat(myNetworkPlayer.Level, myNetworkPlayer.Score);
                myNetworkPlayer.RequestGameOverStat();
            }
        }

        public void DisplayGameOverStat(int level, int score)
        {
            GameOverLevelText.text = "Уровень: " + level.ToString();
            GameOverScoreText.text = "Очки: " + score.ToString();
            GameOverShootingAccuracyText.text = "";
        }

        public void UpdateGameOverStat(int ShootCnt, int ShootGoodCnt)
        {
            if (ShootCnt > 0)
                GameOverShootingAccuracyText.text = "Точность стрельбы " + Mathf.Round(100f * ShootGoodCnt / ShootCnt) + "%";
        }
    }
}
