using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkPlayerController : NetworkBehaviour 
    {
        [SerializeField] private GameObject[] playerPrefabs;
        [SerializeField] private Transform PlayerSpawnPoint;

        [HideInInspector] public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        [Inject] private readonly GameController _gameController;

      
        public bool Spawn(int playerTypeNum, string playerName)
        {
            RPC_Spawn(Runner.LocalPlayer, playerTypeNum, playerName);

            return true;
        }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Spawn(PlayerRef playerRef, int playerTypeNum, string playerName)
        {
            if (!playerPrefabs[playerTypeNum]) return;

            Vector3 spawnPosition = PlayerSpawnPoint.position + (new Vector3(-1 + playerRef.RawEncoded, 0, 0));


            NetworkObject playerObj = Runner.Spawn(
                playerPrefabs[playerTypeNum].GetComponent<NetworkObject>(),
                spawnPosition,
                Quaternion.identity,
                playerRef,
                onBeforeSpawned: (runner, obj) =>
                {
                    obj.GetComponent<NetworkHealth>().Init(100);
                    obj.GetComponent<NetworkPlayer>().Name = playerName;
                    obj.GetComponent<Player>()._gameController = _gameController;
                    obj.GetComponent<PlayerAttack>().IsServer = true;
                }
            );

            _spawnedCharacters.Add(playerRef, playerObj);

            RPC_AssignPlayerToClient(playerRef, playerObj);

            _gameController.UpdateActivePlayers();
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
                Runner.Despawn(_spawnedCharacters[player]);
                _spawnedCharacters.Remove(player);
                _gameController.UpdateActivePlayers();
            }
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
