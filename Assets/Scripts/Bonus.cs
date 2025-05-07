using UnityEngine;

namespace SkillBoxFinal
{
    public class Bonus : MonoBehaviour
    {
        public float Chance = 0.1f;
        [HideInInspector] public bool IsServer = false;
        private NetworkBonus networkBonus;
        private Camera _mainCamera;


        private void Start()
        {
            _mainCamera = Camera.main;
            networkBonus = GetComponent<NetworkBonus>();
        }

        private void Update()
        {
            transform.rotation = _mainCamera.transform.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (networkBonus && other)
                networkBonus.Action(other.gameObject);
                
        }
    }
}
