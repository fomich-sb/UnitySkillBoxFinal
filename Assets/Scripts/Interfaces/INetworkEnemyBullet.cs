
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface INetworkEnemyBullet
    {
        public GameObject Prefab { get; set; }
        public Vector3 TargetPosition { get; set; }
        public void Init(Vector3 pos, Vector3 targetPos, GameObject prefab=null);
        public void ReInit(Vector3 pos, Vector3 targetPos);
        public void Despawn();
    }
}
