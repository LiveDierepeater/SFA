using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera m_CameraTransform;
        [SerializeField] private InputActionReference m_PrimaryActionReference;
        [SerializeField] private InputActionReference m_GasActionReference;
        [SerializeField] private InputActionReference m_GearActionReference;
        [SerializeField] private CarController m_CarController;

        private Player m_Player;
        
        private void Awake()
        {
            InitializeInput();
            m_Player = GetComponent<Player>();
        }

        private void InitializeInput()
        {
            m_PrimaryActionReference.action.performed += HandlePrimaryAction;
            m_GasActionReference.action.performed += HandleGasAction;
            m_GasActionReference.action.canceled += ReleaseGasAction;
            m_GearActionReference.action.performed += HandleGearAction;
            m_GearActionReference.action.canceled += ReleaseGearAction;
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

        private void HandleGearAction(InputAction.CallbackContext ctx)
        {
            if (!m_Player.IsAbleToDrive()) return;
            
            m_CarController.HandleGearInput(ctx.ReadValue<float>());
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
