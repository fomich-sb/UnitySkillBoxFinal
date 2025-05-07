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
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float baseDamage = 1;
        [SerializeField] private float baseInterval = 0.25f;
        [SerializeField] private float HighDamageBulletK = 2f;
        [SerializeField] private ParticleSystem ShootAnimation;
        [SerializeField] private AK.Wwise.Event wwiseEvent;

        [HideInInspector] public Player player;
        [HideInInspector] public bool attack;
        [HideInInspector] public Vector3 attackDirection;
        [HideInInspector] public Vector3 attackPosition;
        private float lastShootTime = 0;
        private int layerMask;
        private Player _player;
        private NetworkPlayer _networkPlayer;
        private NetworkPlayerAttack _networkPlayerAttack;

        [HideInInspector] public int ShootCnt = 0;
        [HideInInspector] public int ShootGoodCnt = 0;
        [HideInInspector] public bool IsServer = false;

        private InputController inputController;

        private void Start()
        {
            layerMask = ~(1 << LayerMask.NameToLayer("Players")) &
                ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
            _player = GetComponent<Player>();
            _networkPlayer = GetComponent<NetworkPlayer>();
            _networkPlayerAttack = GetComponent<NetworkPlayerAttack>();
            inputController = FindFirstObjectByType<InputController>();
        }

        private void Update()
        {
            if (_player.active && attack && Time.time - lastShootTime > GetCurrentInterval())
            {
                if(_player.MyPlayer)
                    DetectTarget();
                Shoot();
            }
        }

        private bool DetectTarget()
        {
            Ray shootRay = new Ray(
                inputController.attackPosition,
                inputController.attackDirection
            );
            NetworkObject hitNetworkObject = null;
            if (Physics.Raycast(shootRay, out RaycastHit hit, 100, layerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out NetworkHealth networkHealth))
                {
                    if (hit.collider.gameObject.TryGetComponent(out Health health))
                    {
                        health.ShootHitAnimation.transform.position = hit.point;
                        health.ShootHitAnimation.Play();
                    }
                    hitNetworkObject = hit.collider.gameObject.GetComponent<NetworkObject>();
                }
            }

            if (_networkPlayerAttack.HitNetworkObject != hitNetworkObject)
            {
                _networkPlayerAttack.RPC_SetHitObject(hitNetworkObject);
                return true;
            }
            return false;
        }

        public void Shoot()
        {
            if (wwiseEvent != null)
                wwiseEvent.Post(gameObject);
            ShootAnimation.Play();
            if (IsServer)
            {

                ShootCnt++;
                if (_networkPlayerAttack.HitNetworkObject && _networkPlayerAttack.HitNetworkObject.TryGetComponent(out NetworkHealth networkHealth))
                {
                    ShootGoodCnt++;
                    if (networkHealth.Damage(GetCurrentDamage()))
                    {
                        if (_networkPlayerAttack.HitNetworkObject.TryGetComponent(out Enemy enemy))
                        {
                            _networkPlayer.Score += enemy.Score;
                            if (enemy.IsBoss)
                                _networkPlayer.AddLevel();
                        }
                    }
                }

                if (_networkPlayer.HighDamageBullets > 0)
                    _networkPlayer.HighDamageBullets--;
            }

            lastShootTime = Time.time;
        }

        public float GetCurrentInterval()
        {
            return baseInterval / (0.5f + 0.5f / _networkPlayer.Level);
        }

        public float GetCurrentDamage()
        {
            float damage = baseDamage * (1 + 0.1f * _networkPlayer.Level);
            if (_networkPlayer.HighDamageBullets > 0)
                damage *= HighDamageBulletK;
            return damage;
        }
    }
}
