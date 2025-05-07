using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using Fusion.Addons.SimpleKCC;
using Unity.Cinemachine;
using Zenject;

namespace SkillBoxFinal
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera PlayerCamera;

        [HideInInspector] public Vector2 _moveInput;
        [HideInInspector] public Vector2 _lookInput;
        [HideInInspector] public bool _attackInput;
        [HideInInspector] public float _lookRotateY = 0;
        [HideInInspector] public float _lookRotateX = 0;
        [HideInInspector] public Vector3 attackDirection;
        [HideInInspector] public Vector3 attackPosition;
        private InputSystem_Actions _inputActions;

        [Inject] private Settings _settings;
        [Inject] private GameController gameController;

        private void Awake()
        {
            _inputActions = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            _inputActions.Player.Enable();

            _inputActions.Player.Move.performed += OnMove;
            _inputActions.Player.Move.canceled += OnMove;
            _inputActions.Player.Look.performed += OnLook;
            _inputActions.Player.Look.canceled += OnLook;
            _inputActions.Player.Attack.performed += OnAttack;
            _inputActions.Player.Attack.canceled += OnAttackCanceled;

            _inputActions.Player.Exit.performed += OnExit;
        }

        private void OnDisable()
        {
            _inputActions.Player.Move.performed -= OnMove;
            _inputActions.Player.Move.canceled -= OnMove;
            _inputActions.Player.Look.performed -= OnLook;
            _inputActions.Player.Look.canceled -= OnLook;
            _inputActions.Player.Attack.performed -= OnAttack;
            _inputActions.Player.Attack.canceled -= OnAttackCanceled;

            _inputActions.Player.Exit.performed -= OnExit;

            _inputActions.Player.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>() * _settings.mouseSensitivity;
            _lookRotateY += _lookInput.x;
            _lookRotateX -= _lookInput.y;
            _lookRotateX = Mathf.Min(30, Mathf.Max(-30, _lookRotateX));
            PlayerCamera.transform.localRotation = Quaternion.Euler(_lookRotateX, 0, 0);
            if (_attackInput)
            {
                attackDirection = PlayerCamera.transform.forward;
                attackPosition = PlayerCamera.transform.position;
            }
            else
            {
                attackDirection = Vector2.zero;
                attackPosition = Vector3.zero;
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            _attackInput = true;
        }

        private void OnAttackCanceled(InputAction.CallbackContext context)
        {
            _attackInput = false;
        }

        private void OnExit(InputAction.CallbackContext context)
        {
            gameController.OpenSettings();
        }
    }
}