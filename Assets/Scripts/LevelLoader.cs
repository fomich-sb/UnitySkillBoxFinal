using SkillBoxFinal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Transform PlayerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoints;

   /* [Inject] private readonly NetworkEnemyController networkEnemyController;
    [Inject] private readonly NetworkPlayerController networkPlayerController;*/

    private void Start()
    {
        FindFirstObjectByType<NetworkEnemyController>().SetEnemySpawnPoints(enemySpawnPoints);
        FindFirstObjectByType<NetworkPlayerController>().SetPlayerSpawnPoint(PlayerSpawnPoint);
    }
}
