using Game.Core;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Inventory))]
    public class Player : MonoBehaviour
    {
        private Camera m_Camera;
        private PlayerController m_PlayerController;
        
        private void Awake()
        {
            m_Camera = GetComponentInChildren<Camera>();
            m_PlayerController = GetComponentInChildren<PlayerController>();
            GameManager.Instance.OnPlayerInitialize(this);
        }

        public void LevelTransition(Transform targetTransform)
        {
            // TODO: Is player able to transit to next level?
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }

        // TODO: Implement player driving state
        public bool IsAbleToDrive() => true;
        public Camera GetCamera() => m_Camera;
        public PlayerController GetPlayerController() => m_PlayerController;
    }
}
