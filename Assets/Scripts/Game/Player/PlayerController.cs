using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera m_cameraTransform;
        [SerializeField] private InputActionReference m_actionReference;

        private void Awake() => InitializeInput();

        private void InitializeInput() => m_actionReference.action.performed += HandlePrimaryAction;

        private void HandlePrimaryAction(InputAction.CallbackContext ctx)
        {
            if (!Physics.Raycast(m_cameraTransform.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 9999.0f)) return;
            if (!hit.transform.TryGetComponent(out IInteractable interactable)) return;
            
            interactable.OnInteract();
        }
    }
}
