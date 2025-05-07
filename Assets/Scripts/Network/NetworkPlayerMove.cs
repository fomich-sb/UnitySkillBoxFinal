using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkPlayerMove : NetworkBehaviour
    {
        private NetworkCharacterController _networkCharacterController;
        private Player player;

        public override void Spawned()
        {
            _networkCharacterController = GetComponent<NetworkCharacterController>();
            player = GetComponent<Player>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                transform.rotation = Quaternion.Euler(0, data.lookRotateY, 0);

                Vector3 direction = transform.rotation * new Vector3(data.moveDirection.x, 0, data.moveDirection.y);

                _networkCharacterController.Move(direction * Runner.DeltaTime);

                if(direction==Vector3.zero)
                {
                    player.animator.SetFloat("left", 0);
                    player.animator.SetFloat("forward", 0);
                }
                else if(Mathf.Abs(direction.z) >= Mathf.Abs(direction.x))
                {
                    player.animator.SetFloat("left", 0);
                    player.animator.SetFloat("forward", direction.z > 0 ? -1 : 1);
                }
                else
                {
                    player.animator.SetFloat("left", direction.x > 0 ? -1 : 1);
                    player.animator.SetFloat("forward", 0);
                }
            }
        }
    }
}
