using Fusion;
using System.Collections.Generic;
using Unity.Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SkillBoxFinal
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera PlayerCamera;

        [HideInInspector] public List<INetworkPlayer> ActiveNetworkPlayers = new();
        private GameObject myPlayerGO;
        public GameStatus status { get; private set; } = GameStatus.MainMenu;

        [Inject] private UIController uIController;
        [Inject] private NetworkController _networkController;
        [Inject] private NetworkPlayerController _networkPlayerController;
        [Inject] private Settings _settings;

        public event Action<GameStatus> OnChangeGameStatus;


        private void Start()
        {
            _settings.LoadSettings();
            SetStatus(GameStatus.MainMenu);

            Player.OnDeadAny += UpdateActivePlayers;
            _networkPlayerController.OnSpawn += UpdateActivePlayers;
            _networkPlayerController.OnDespawn += UpdateActivePlayers;
        }

        private void OnDestroy()
        {
            Player.OnDeadAny -= UpdateActivePlayers;
            _networkPlayerController.OnSpawn -= UpdateActivePlayers;
            _networkPlayerController.OnDespawn -= UpdateActivePlayers;
        }

        public void SetPlayerGO(GameObject _playerGO)
        {
            myPlayerGO = _playerGO;
            if (myPlayerGO.TryGetComponent(out IPlayer player))
            {
                player.MyPlayer = true;
                player.OnDead += GameOver;
                uIController.SetMyPlayer(player);
            }
            if (myPlayerGO.TryGetComponent(out IPlayerInfoDisplay playerInfoDisplay))
            {
                playerInfoDisplay.HideMyName();
            }

            if (myPlayerGO.TryGetComponent(out IHealthArmorDisplay healthArmorDisplay))
                healthArmorDisplay.MyPlayer = true;


            PlayerCamera.Follow = myPlayerGO.transform;
            PlayerCamera.LookAt = myPlayerGO.transform;
            PlayerCamera.Priority = 100;
            PlayerCamera.transform.SetParent(myPlayerGO.transform, false);
            SetStatus(GameStatus.Playing);
        }

        public void SetStatus(GameStatus gameStatus)
        {
            status = gameStatus;
            OnChangeGameStatus?.Invoke(status);
        }

        public bool StartGame(int playerTypeNum, int locationTypeNum, string playerName)
        {
            SetStatus(GameStatus.Loading);
            _networkController.StartGame(playerTypeNum, locationTypeNum, playerName);
            return true;
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
            ActiveNetworkPlayers = new List<INetworkPlayer>();
            foreach (KeyValuePair<PlayerRef, NetworkObject> playerItem in _networkPlayerController._spawnedCharacters)
            {
                IPlayer pl = playerItem.Value.gameObject.GetComponent<IPlayer>();
                if(pl is not null && pl.Active)
                    ActiveNetworkPlayers.Add(playerItem.Value.gameObject.GetComponent<INetworkPlayer>());
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
