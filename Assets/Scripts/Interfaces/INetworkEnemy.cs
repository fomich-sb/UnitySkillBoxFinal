
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface INetworkEnemy
    {
        public GameObject Prefab { get; set; }
        public void Init(Vector3 pos, GameObject prefab);
        public void ReInit(Vector3 pos);
        public void Despawn();
    }
}
