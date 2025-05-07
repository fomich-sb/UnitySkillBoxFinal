using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillBoxFinal
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerInfoPanel;
        [SerializeField] private TextMeshPro PlayerNameTextMesh;
        [SerializeField] private TextMeshPro PlayerLevelTextMesh;
        [SerializeField] private AK.Wwise.Event wwiseEventDead;
        public Animator animator;

        [HideInInspector] public Text MyLevelText;
        [HideInInspector] public Text MyScoreText;
        [HideInInspector] public Text MyHighDamageBulletsText;
        [HideInInspector] public bool IsDead;

        [HideInInspector] public GameController _gameController;
        [HideInInspector] public bool active = false;
        [HideInInspector] public bool MyPlayer = false;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            if (TryGetComponent(out NetworkHealth networkHealth))
                networkHealth.OnDead += Dead;
        }

        private void Update()
        {
            if (_mainCamera != null && PlayerInfoPanel.activeSelf)
            {
                // Поворот к камере (билбординг)
                PlayerInfoPanel.transform.rotation = _mainCamera.transform.rotation;
            }
        }

        public void SetName(string Name)
        {
            PlayerNameTextMesh.text = Name;
        }

        public void HideMyName()
        {
            PlayerInfoPanel.SetActive(false);
        }

        public void DisplayLevel(int level)
        {
            if(MyLevelText)
                MyLevelText.text = level.ToString();
            else
                PlayerLevelTextMesh.text = "уровень " + level.ToString();

            float scale = 1 + (0.1f * (level - 1));
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void DisplayScore(int score)
        {
            if (MyScoreText)
                MyScoreText.text = score.ToString();
        }

        public void DisplayHighDamageBullets(int cnt)
        {
            if (MyHighDamageBulletsText)
                MyHighDamageBulletsText.text = cnt.ToString();
        }

        public void Dead()
        {
            IsDead = true;
            if (wwiseEventDead != null)
                wwiseEventDead.Post(gameObject);
            
            animator.SetBool("dead", true);
            active = false;
            if (_gameController)
                _gameController.UpdateActivePlayers();

            if (TryGetComponent(out NetworkPlayerMove networkPlayerMove))
                networkPlayerMove.enabled = false;
            if (TryGetComponent(out PlayerAttack pa))
                pa.enabled = false;


            if (MyPlayer)
            {
                NetworkPlayer np = GetComponent<NetworkPlayer>();
                _gameController.GameOverLevelText.text = "Уровень: " + np.Level.ToString();
                _gameController.GameOverScoreText.text = "Очки: " + np.Score.ToString();
                _gameController.GameOverShootingAccuracyText.text = "";
                np.RPC_RequestGameOverStat();
                _gameController.GameOver();
            }
        }
        public void UpdateGameOverStat(int ShootCnt, int ShootGoodCnt)
        {
            if(ShootCnt>0)
                _gameController.GameOverShootingAccuracyText.text = "Точность стрельбы " + Mathf.Round(100f * ShootGoodCnt / ShootCnt) + "%";
        }
    }
}
