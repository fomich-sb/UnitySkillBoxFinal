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
    public class GameController : MonoBehaviour
    {

        [SerializeField] private GameObject StartMenuCanvas;
        [SerializeField] private GameObject LoadingCanvas;
        [SerializeField] private GameObject GameOverCanvas;
        [SerializeField] private GameObject SettingsCanvas;
        [SerializeField] private GameObject SettingsPanelInGame;
        [SerializeField] private GameObject PlayCanvasRoot;
        [SerializeField] private CinemachineCamera PlayerCamera;

        [SerializeField] private Text MyHealthText;
        [SerializeField] private Text MyArmorText;
        [SerializeField] private Text MyLevelText; 
        [SerializeField] private Text MyScoreText;
        [SerializeField] private Text MyHighDamageBulletsText;

        public Text GameOverLevelText;
        public Text GameOverScoreText;
        public Text GameOverShootingAccuracyText;

        [HideInInspector] public List<NetworkPlayer> ActiveNetworkPlayers = new();
        private GameObject myPlayerGO;
        private GameStatus status = GameStatus.MainMenu;

        [Inject] private NetworkController _networkController;
        [Inject] private NetworkPlayerController _networkPlayerController;
        [Inject] private Settings _settings;


        private void Start()
        {
            _settings.LoadSettings();
            SetStatus(GameStatus.MainMenu);
        }

        public void SetPlayerGO(GameObject _playerGO)
        {
            myPlayerGO = _playerGO;
            if (myPlayerGO.TryGetComponent(out Player player))
            {
                player.HideMyName();
                player.MyLevelText = MyLevelText;
                player.MyScoreText = MyScoreText;
                player.MyHighDamageBulletsText = MyHighDamageBulletsText;
                player.MyPlayer = true;
                player._gameController = this;
            }

            if (myPlayerGO.TryGetComponent(out Health health))
            {
                health.MyHealthText = MyHealthText;
                health.MyArmorText = MyArmorText;
            }
            if (myPlayerGO.TryGetComponent(out NetworkHealth networkHealth))
            {
                networkHealth.Display();
            }

            PlayerCamera.Follow = myPlayerGO.transform;
            PlayerCamera.LookAt = myPlayerGO.transform;
            PlayerCamera.Priority = 100;
            PlayerCamera.transform.SetParent(myPlayerGO.transform, false);
            SetStatus(GameStatus.Playing);
        }

        public bool StartGame(int playerTypeNum, int locationTypeNum, string playerName)
        {
            SetStatus(GameStatus.Loading);
            _networkController.StartGame(playerTypeNum, locationTypeNum, playerName);
            return true;
            /*          {
                          return true;
                      }
                      SetStatus(GameStatus.MainMenu);
                      return false;*/
        }

        public void SetStatus(GameStatus gameStatus)
        {
            status = gameStatus;
            SettingsCanvas.SetActive(false);

            StartMenuCanvas.SetActive(status == GameStatus.MainMenu);
            LoadingCanvas.SetActive(status == GameStatus.Loading);
            SettingsPanelInGame.SetActive(status == GameStatus.Playing);
            GameOverCanvas.SetActive(status == GameStatus.GameOver);
            PlayCanvasRoot.SetActive(status == GameStatus.Playing);
            

            if (status == GameStatus.Playing)
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

        public void OpenSettings()
        {
            if (SettingsCanvas.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                SettingsCanvas.SetActive(true);
                if (status == GameStatus.Playing)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
        public void CloseSettings()
        {
            SettingsCanvas.SetActive(false);
            if (status == GameStatus.Playing)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void GameOver()
        {
            SetStatus(GameStatus.GameOver);
        }

        public void ExitToMainMenu()
        {

            PlayerCamera.Priority = 0;
            PlayerCamera.transform.SetParent(null, false);
            _networkController.ExitPlayer();
            SetStatus(GameStatus.MainMenu);
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif

            Debug.Log("Игра завершена");
        }

        public void UpdateActivePlayers()
        {
            ActiveNetworkPlayers = new List<NetworkPlayer>();
            foreach (KeyValuePair<PlayerRef, NetworkObject> playerItem in _networkPlayerController._spawnedCharacters)
            {
                Player pl = playerItem.Value.gameObject.GetComponent<Player>();
                if(pl && pl.active)
                    ActiveNetworkPlayers.Add(playerItem.Value.gameObject.GetComponent<NetworkPlayer>());
            }
        }

        public enum GameStatus
        {
            MainMenu,     // В главном меню
            Loading,      // Идет загрузка
            Playing,      // Игровой процесс
            GameOver,     // Поражение/конец игры
        }
    }
}
