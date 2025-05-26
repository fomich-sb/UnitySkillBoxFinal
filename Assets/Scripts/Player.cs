using Fusion;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillBoxFinal
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private GameObject PlayerInfoPanel;
        [SerializeField] private TextMeshPro PlayerNameTextMesh;
        [SerializeField] private TextMeshPro PlayerLevelTextMesh;
        [SerializeField] private AK.Wwise.Event wwiseEventDead;
        public event Action OnDead;
        public static event Action OnDeadAny;

        [HideInInspector] public Text MyLevelText;
        [HideInInspector] public Text MyScoreText;
        [HideInInspector] public Text MyHighDamageBulletsText;
        [HideInInspector] public bool IsDead { get; set; }

        [HideInInspector] public bool Active { get; set; } = false;
        [HideInInspector] public bool MyPlayer { get; set; } = false;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            if (TryGetComponent(out IDamageable damageable))
                damageable.OnDead += Dead;
        }

        private void Update()
        {
            if (_mainCamera != null && PlayerInfoPanel.activeSelf)
            {
                // Поворот к камере (билбординг)
                PlayerInfoPanel.transform.rotation = _mainCamera.transform.rotation;
            }
        }

        public void Dead()
        {
            IsDead = true;
            if (wwiseEventDead != null)
                wwiseEventDead.Post(gameObject);
            
            Active = false;

            if (TryGetComponent(out NetworkPlayerMove networkPlayerMove))
                networkPlayerMove.enabled = false;
            if (TryGetComponent(out PlayerAttack pa))
                pa.enabled = false;

            OnDead?.Invoke();
            OnDeadAny?.Invoke();
        }
    }
}
