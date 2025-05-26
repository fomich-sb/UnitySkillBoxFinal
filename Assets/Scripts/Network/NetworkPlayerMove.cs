using Fusion;
using System.Data;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkPlayerMove : NetworkBehaviour
    {
        private NetworkCharacterController _networkCharacterController;
        private IPlayerAnimator playerAnimation;

        public override void Spawned()
        {
            _networkCharacterController = GetComponent<NetworkCharacterController>();
            playerAnimation = GetComponent<IPlayerAnimator>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                transform.rotation = Quaternion.Euler(0, data.lookRotateY, 0);

                Vector3 direction = transform.rotation * new Vector3(data.moveDirection.x, 0, data.moveDirection.y);

                _networkCharacterController.Move(direction * Runner.DeltaTime);

                playerAnimation?.UpdateStatus(direction);
            }
        }
    }
}
