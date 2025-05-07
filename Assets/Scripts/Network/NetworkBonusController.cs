using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkBonusController : NetworkBehaviour
    {
        [SerializeField] private GameObject[] bonusPrefabs;
        private float[] bonusChances;

        override public void Spawned()
        {
            bonusChances = new float[bonusPrefabs.Length];
            for (int i = 0; i < bonusPrefabs.Length; i++)
                bonusChances[i] = bonusPrefabs[i].GetComponent<Bonus>().Chance;

        }

        public void CheckNeedSpawn(Vector3 pos)
        {
            float bonusValue = Random.value;
            float chanceSum = 0;
            for (int i = 0; i < bonusChances.Length; i++)
            {
                if (bonusValue < chanceSum + bonusChances[i])
                {
                    Spawn(i, pos);
                    break;
                }
                chanceSum += bonusChances[i];
            }
        }

        private void Spawn(int i, Vector3 pos)
        {
            NetworkObject bonusObj = Runner.Spawn(
                bonusPrefabs[i],
                pos,
                Quaternion.identity,
                onBeforeSpawned: (runner, obj) =>
                {
                    obj.GetComponent<Bonus>().IsServer = true;
                    obj.GetComponent<NetworkBonus>().Init(pos);
                }
            );
        }
    }
}
