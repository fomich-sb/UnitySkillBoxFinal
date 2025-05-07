using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkController : NetworkBehaviour, INetworkRunnerCallbacks 
    {

        [SerializeField] private NetworkRunner _runn;

        [Inject] private InputController inputController;
        [Inject] private NetworkPlayerController _networkPlayerController;

        [HideInInspector] public delegate void OnConnectedContainer(bool isServer);
        [HideInInspector] public event OnConnectedContainer OnConnected;
        private string _sessionName = "Room1";


        private void Awake()
        {
            InitGame(GameMode.AutoHostOrClient);
        }

        async void InitGame(GameMode mode)
        {
            _runn.AddCallbacks(this);
            _runn.ProvideInput = true;

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            // Start or join (depends on gamemode) a session with a specific name
            var result = await _runn.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = _sessionName,
                Scene = scene,
                SceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>()
            });

            if (result.Ok)
            {
                OnConnected(_runn.IsServer);
            }
            else
            {
            Debug.Log("Runner Ошибка запуска!");
            }
        }


        public bool StartGame(int playerTypeNum, string playerName)
        {
            return _networkPlayerController.Spawn(playerTypeNum, playerName);
        }

        public void ExitPlayer()
        {
            _networkPlayerController.RPC_Despawn(_runn.LocalPlayer);
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            data.moveDirection = inputController._moveInput;
            data.lookRotateY = inputController._lookRotateY;
            data.attack = inputController._attackInput;
            input.Set(data);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                _networkPlayerController.RPC_Despawn(player);
            }
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
        {
        }
    }
}
