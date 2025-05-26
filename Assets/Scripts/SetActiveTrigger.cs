using UnityEngine;
using Zenject;

namespace SkillBoxFinal
{
    public class SetActiveTrigger : MonoBehaviour
    {
        private GameController _gameController;

        private void Start()
        {
            _gameController = FindFirstObjectByType<GameController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IPlayer>(out var player))
            {
                player.Active = true;
                _gameController.UpdateActivePlayers();
            }
        }
    }
}
