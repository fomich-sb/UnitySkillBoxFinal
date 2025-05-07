using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillBoxFinal
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private TextMeshPro HealthTextMesh;
        public ParticleSystem ShootHitAnimation;

        [HideInInspector] public Text MyHealthText;
        [HideInInspector] public Text MyArmorText;

        public void Display(float health, float armor)
        {
            if (MyHealthText)
            {
                MyHealthText.text = Mathf.Ceil(health).ToString();
                MyArmorText.text = Mathf.Ceil(armor).ToString();
            }
            else
            {
                HealthTextMesh.text = Mathf.Ceil(health).ToString() + (armor > 0 ? " + " + Mathf.Ceil(armor).ToString() : "");
            }

        }

    }
}
