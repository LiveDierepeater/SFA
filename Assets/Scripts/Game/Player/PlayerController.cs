using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera m_CameraTransform;
        [Header("Input")]
        [SerializeField] private InputActionReference m_PrimaryActionReference;
        [SerializeField] private InputActionReference m_GasActionReference;
        [SerializeField] private InputActionReference m_GearActionReference;
        [SerializeField] private CarController m_CarController;

        private Player m_Player;
        
        private float m_CurrentGasInput;
        private float m_CurrentGearInput;
        
        private void Awake()
        {
            //InitializeInput();
            m_Player = GetComponent<Player>();
        }

        private void InitializeInput()
        {
            m_PrimaryActionReference.action.performed += HandlePrimaryAction;
            m_GasActionReference.action.performed += HandleGasAction;
            m_GearActionReference.action.performed += HandleGearAction;
        }

        private void FixedUpdate()
        {
            m_CurrentGasInput = m_GasActionReference.action.ReadValue<float>();
            m_CurrentGearInput = m_GearActionReference.action.ReadValue<float>();
            
            HandleGasAction(m_CurrentGasInput);
            HandleGearAction(m_CurrentGearInput);
        }

        private void HandlePrimaryAction(InputAction.CallbackContext ctx)
        {
            if (!Physics.Raycast(m_CameraTransform.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 9999.0f)) return;
            if (!hit.transform.TryGetComponent(out IInteractable interactable)) return;
            
            interactable.OnInteract();
        }

        private void HandleGasAction(InputAction.CallbackContext ctx)
        {
            if (!m_Player.IsAbleToDrive()) return;
            
            m_CarController.HandleGasInput(ctx.ReadValue<float>());
        }
        private void HandleGasAction(float value)
        {
            if (!m_Player.IsAbleToDrive()) return;
            m_CarController.HandleGasInput(value);
        }

        private void HandleGearAction(InputAction.CallbackContext ctx)
        {
            if (!m_Player.IsAbleToDrive()) return;
            m_CarController.HandleGearInput(ctx.ReadValue<float>());
        }
        private void HandleGearAction(float value)
        {
            if (!m_Player.IsAbleToDrive()) return;
            m_CarController.HandleGearInput(value);
        }
        
        private void ReleaseGasAction(InputAction.CallbackContext ctx)
        {
            if (!m_Player.IsAbleToDrive() || Mathf.Abs(ctx.ReadValue<float>()) >= 0.2f) return;
            
            m_CarController.HandleGasInput(0);
        }

        private void ReleaseGearAction(InputAction.CallbackContext ctx)
        {
            if (!m_Player.IsAbleToDrive() || Mathf.Abs(ctx.ReadValue<float>()) >= 0.2f) return;
            
            m_CarController.HandleGearInput(0);
        }
    }
}
