using UnityEngine;

namespace SkillBoxFinal
{
    public class Bonus : MonoBehaviour, IBonus
    {
        public float Chance { get; set; } = 0.1f;
        [HideInInspector] public bool IsServer { get; set; } = false;
        private INetworkBonus networkBonus;
        private Camera _mainCamera;


        private void Start()
        {
            _mainCamera = Camera.main;
            networkBonus = GetComponent<INetworkBonus>();
        }

        private void Update()
        {
            transform.rotation = _mainCamera.transform.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other)
                networkBonus.Action(other.gameObject);
        }
    }
}
