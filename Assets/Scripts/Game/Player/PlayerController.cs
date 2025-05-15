using System;
using Game.Vehicle;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public enum PlayerInputType
    {
        KeyboardMouse,
        Mobile
    }
    
    public class PlayerController : MonoBehaviour
    {
        public delegate void PlayerControllerEvents();
        public PlayerControllerEvents OnPrimaryActionHold;
        public PlayerControllerEvents OnPrimaryActionReleased;
        
        [SerializeField] private Camera m_CameraTransform;
        [SerializeField] private LayerMask m_RaytracedLayers;
        [Header("Input")]
        [SerializeField] private InputActionReference m_PrimaryActionReference;
        [SerializeField] private InputActionReference m_GasActionReference;
        [SerializeField] private InputActionReference m_GearActionReference;
        [SerializeField] private CarController m_CarController;

        private Player m_Player;

        [SerializeField] private bool m_CarInputEnabled;
        [SerializeField] private PlayerInputType m_PlayerInputType = PlayerInputType.KeyboardMouse;
        [SerializeField] private FixedJoystick m_GasJoystick;
        [SerializeField] private FixedJoystick m_GearJoystick;
        private float m_CurrentGasInput;
        private float m_CurrentGearInput;
        
        private void Awake()
        {
            InitializeInput();
            m_Player = GetComponent<Player>();
            
            #if UNITY_STANDALONE_WIN
            m_PlayerInputType = PlayerInputType.KeyboardMouse;
            UIManager.Instance.DisableMobileControlsUI();
            #elif UNITY_ANDROID
            m_PlayerInputType = PlayerInputType.Mobile;
            #endif
        }

        private void InitializeInput()
        {
            m_PrimaryActionReference.action.performed += HandlePrimaryAction;
            m_PrimaryActionReference.action.canceled += HandlePrimaryActionReleased;
            m_PrimaryActionReference.action.Enable();
            m_GasActionReference.action.Enable();
            m_GearActionReference.action.Enable();
        }

        private void FixedUpdate()
        {
            if (m_CarInputEnabled)
                HandleCarInput();
        }

        private void Update()
        {
            if (m_PrimaryActionReference.action.IsPressed() && !m_PrimaryActionReference.action.WasPerformedThisFrame())
                OnPrimaryActionHold?.Invoke();
        }

        private void HandleCarInput()
        {
            switch (m_PlayerInputType)
            {
                case PlayerInputType.KeyboardMouse:
                    m_CurrentGasInput = m_GasActionReference.action.ReadValue<float>();
                    m_CurrentGearInput = m_GearActionReference.action.ReadValue<float>();
                    break;
                
                case PlayerInputType.Mobile:
                    m_CurrentGasInput = m_GasJoystick.Vertical;
                    m_CurrentGearInput = m_GearJoystick.Horizontal;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            HandleGasAction(m_CurrentGasInput);
            HandleGearAction(m_CurrentGearInput);
        }

        private void HandlePrimaryAction(InputAction.CallbackContext ctx)
        {
            if (!Physics.Raycast(m_CameraTransform.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 9999.0f, m_RaytracedLayers)) return;
            if (!hit.transform.TryGetComponent(out IInteractable interactable)) return;
            
            interactable?.OnInteract();
        }
        private void HandlePrimaryActionReleased(InputAction.CallbackContext ctx) => OnPrimaryActionReleased?.Invoke();

        private void HandleGasAction(float value)
        {
            if (!m_Player.IsAbleToDrive()) return;
            m_CarController.HandleGasInput(value);
        }
        private void HandleGearAction(float value)
        {
            if (!m_Player.IsAbleToDrive()) return;
            m_CarController.HandleGearInput(value);
        }

        public CarController GetCarController() => m_CarController;
        
        public void EnableCarInput() => m_CarInputEnabled = true;
        public void DisableCarInput() => m_CarInputEnabled = false;
        public bool IsCarInputEnabled() => m_CarInputEnabled;
    }
}
