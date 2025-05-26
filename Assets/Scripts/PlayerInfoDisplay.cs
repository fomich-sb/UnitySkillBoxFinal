using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillBoxFinal
{
    public class PlayerInfoDisplay : MonoBehaviour, IPlayerInfoDisplay
    {
        [SerializeField] private GameObject PlayerInfoPanel;
        [SerializeField] private TextMeshPro PlayerNameTextMesh;
        [SerializeField] private TextMeshPro PlayerLevelTextMesh;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_mainCamera != null && PlayerInfoPanel.activeSelf)
                PlayerInfoPanel.transform.rotation = _mainCamera.transform.rotation;
        }

        public void DisplayName(string Name)
        {
            PlayerNameTextMesh.text = Name;
        }

        public void HideMyName()
        {
            PlayerInfoPanel.SetActive(false);
        }

        public void DisplayLevel(int level)
        {
            PlayerLevelTextMesh.text = "уровень " + level.ToString();

            float scale = 1 + (0.02f * (level - 1));
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void DisplayScore(int score)
        {
        }

        public void DisplayHighDamageBullets(int cnt)
        {
        }

    }
}
