using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkPlayerController : NetworkBehaviour 
    {
        [SerializeField] private GameObject[] playerPrefabs;

        [HideInInspector] public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        private Vector3 PlayerSpawnPointPosition;

        [Inject] private IPlayerFactory _playerFactory;
        [Inject] private readonly GameController _gameController;

        public event Action OnSpawn;
        public event Action OnDespawn;

        public void SetPlayerSpawnPoint(Transform _playerSpawnPoint)
        {
            PlayerSpawnPointPosition = _playerSpawnPoint.position;
        }


        public bool Spawn(int playerTypeNum, string playerName)
        {
            RPC_Spawn(Runner.LocalPlayer, playerTypeNum, playerName);

            return true;
        }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Spawn(PlayerRef playerRef, int playerTypeNum, string playerName)
        {
            if (!playerPrefabs[playerTypeNum]) return;

            Vector3 spawnPosition = PlayerSpawnPointPosition + (new Vector3(-1 + playerRef.RawEncoded, 0, 0));


            NetworkObject playerObj = _playerFactory.CreatePlayer(playerPrefabs[playerTypeNum], spawnPosition, playerRef, playerName);

            _spawnedCharacters.Add(playerRef, playerObj);

            RPC_AssignPlayerToClient(playerRef, playerObj);

            OnSpawn?.Invoke();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_AssignPlayerToClient([RpcTarget] PlayerRef player, NetworkObject playerObj)
        {
            AssignPlayerToClient(player, playerObj);
        }

        public void AssignPlayerToClient(PlayerRef player, NetworkObject playerObj)
        {
            _gameController.SetPlayerGO(playerObj.gameObject);
        }

        public void Despawn(PlayerRef player)
        {
            if (_spawnedCharacters.ContainsKey(player))
            {
                _playerFactory.RecyclePlayer(_spawnedCharacters[player]);
                _spawnedCharacters.Remove(player);
            }
            OnDespawn?.Invoke();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Despawn(PlayerRef player)
        {
            Despawn(player);
        }

        public void Exit()
        {
            RPC_Despawn(Runner.LocalPlayer);
        }
    }
}
