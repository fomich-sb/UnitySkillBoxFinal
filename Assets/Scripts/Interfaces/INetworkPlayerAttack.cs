
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface INetworkPlayerAttack 
    {
        public NetworkObject HitNetworkObject { get; set; }
        public void SetHitObject(NetworkObject hitNetworkObject);
    }
}
