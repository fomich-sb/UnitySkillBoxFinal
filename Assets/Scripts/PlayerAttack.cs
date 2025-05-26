using Fusion;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;

namespace SkillBoxFinal
{
    public class PlayerAttack : MonoBehaviour, IPlayerAttack
    {
        [SerializeField] private float baseDamage = 1;
        [SerializeField] private float baseInterval = 0.25f;
        [SerializeField] private float HighDamageBulletK = 2f;
        [SerializeField] private ParticleSystem ShootAnimation;
        [SerializeField] private AK.Wwise.Event wwiseEvent;

        [HideInInspector] public IPlayer player;
        [HideInInspector] public bool Attack { get; set; }
        [HideInInspector] public Vector3 attackDirection;
        [HideInInspector] public Vector3 attackPosition;
        private float lastShootTime = 0;
        private IPlayer _player;
        private INetworkPlayer _networkPlayer;
        private INetworkPlayerAttack _networkPlayerAttack;
        private IPlayerDetectTarget _playerDetectTarget;

        [HideInInspector] public int ShootCnt { get; set; } = 0;
        [HideInInspector] public int ShootGoodCnt { get; set; } = 0;
        [HideInInspector] public bool IsServer { get; set; } = false;


        private void Start()
        {
            _player = GetComponent<IPlayer>();
            _networkPlayer = GetComponent<INetworkPlayer>();
            _networkPlayerAttack = GetComponent<INetworkPlayerAttack>();
            _playerDetectTarget = GetComponent<IPlayerDetectTarget>();
        }

        private void Update()
        {
            if (_player.Active && Attack && Time.time - lastShootTime > GetCurrentInterval())
            {
                if(_player.MyPlayer)
                    _playerDetectTarget.DetectTarget();
                Shoot();
            }
        }

        public void Shoot()
        {
            if (wwiseEvent != null)
                wwiseEvent.Post(gameObject);
            ShootAnimation.Play();
            if (IsServer)
            {

                ShootCnt++;
                if (_networkPlayerAttack.HitNetworkObject && _networkPlayerAttack.HitNetworkObject.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log("KILL");
                    ShootGoodCnt++;
                    if (damageable.Damage(GetCurrentDamage()))
                    {
                        Debug.Log("KILL2");
                        if (_networkPlayerAttack.HitNetworkObject.TryGetComponent(out IEnemy enemy))
                        {
                            Debug.Log("KILL3");
                            _networkPlayer.Score += enemy.Score;
                            if (enemy.IsBoss)
                            {
                                Debug.Log("KILL4");
                                _networkPlayer.AddLevel();
                            }
                        }
                    }
                }

                if (_networkPlayer.HighDamageBullets > 0)
                    _networkPlayer.HighDamageBullets--;
            }

            lastShootTime = Time.time;
        }

        private float GetCurrentInterval()
        {
            return baseInterval / (0.5f + 0.5f / _networkPlayer.Level);
        }

        private float GetCurrentDamage()
        {
            float damage = baseDamage * (1 + 0.1f * _networkPlayer.Level);
            if (_networkPlayer.HighDamageBullets > 0)
                damage *= HighDamageBulletK;
            return damage;
        }
    }
}
