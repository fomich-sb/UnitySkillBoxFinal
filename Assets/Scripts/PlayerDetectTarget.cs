using Fusion;
using System;
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
    public class PlayerDetectTarget : MonoBehaviour, IPlayerDetectTarget
    {
        private INetworkPlayerAttack _networkPlayerAttack;
        private int layerMask;
        private InputController inputController;

        private void Start()
        {
            layerMask = ~(1 << LayerMask.NameToLayer("Players")) &
                ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
            _networkPlayerAttack = GetComponent<INetworkPlayerAttack>();
            inputController = FindFirstObjectByType<InputController>();
        }


        public bool DetectTarget()
        {
            Ray shootRay = new Ray(
                inputController.attackPosition,
                inputController.attackDirection
            );
            NetworkObject hitNetworkObject = null;
            if (Physics.Raycast(shootRay, out RaycastHit hit, 100, layerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    if (hit.collider.gameObject.TryGetComponent(out IHealthArmorDisplay healthArmorDisplay))
                    {
                        healthArmorDisplay.PlayShootHitAnimation(hit.point);
                        
                    }
                    hitNetworkObject = hit.collider.gameObject.GetComponent<NetworkObject>();
                }
            }

            if (_networkPlayerAttack.HitNetworkObject != hitNetworkObject)
            {
                _networkPlayerAttack.SetHitObject(hitNetworkObject);
                return true;
            }
            return false;
        }
    }
}
