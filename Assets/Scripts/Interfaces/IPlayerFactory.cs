
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IPlayerFactory
    {
        NetworkObject CreatePlayer(GameObject prefab, Vector3 position, PlayerRef playerRef, String playerName);

        void RecyclePlayer(NetworkObject player);
    }
}
