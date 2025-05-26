using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SkillBoxFinal
{
    public class HealthArmorDisplay : MonoBehaviour, IHealthArmorDisplay
    {
        [SerializeField] private TextMeshPro HealthTextMesh;
        public ParticleSystem ShootHitAnimation;
        [HideInInspector] public bool MyPlayer { get; set; } = false;

        private IHealthSystem healthSystem;
        private IArmorSystem armorSystem;

        [Inject] private UIController uIController;

        private void Start()
        {
            if (TryGetComponent(out healthSystem))
                healthSystem.OnChange += Display;
            if (TryGetComponent(out armorSystem))
                armorSystem.OnChange += Display;
            Display();
        }

        public void Display()
        {
            if (MyPlayer)
                uIController?.DisplayMyHealthArmor(healthSystem?.Value ?? 0f, armorSystem?.Value ?? 0f);
            else
                HealthTextMesh.text = Mathf.Ceil(healthSystem?.Value ?? 0f).ToString() + ((armorSystem?.Value ?? 0f) > 0 ? " + " + Mathf.Ceil(armorSystem.Value).ToString() : "");
        }

        public void PlayShootHitAnimation(Vector3 position)
        {
            ShootHitAnimation.transform.position = position;
            ShootHitAnimation.Play();
        }
    }
}
