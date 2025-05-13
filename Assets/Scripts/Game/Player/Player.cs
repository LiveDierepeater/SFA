using Game.Core;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Game.Player
{
    [RequireComponent(typeof(Inventory))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float m_SmoothCameraSpeed = 1f;
        
        private Camera m_Camera;
        private PlayerController m_PlayerController;
        
        private Vector3 m_SmoothCameraPosition;
        private Quaternion m_SmoothCameraRotation;
        private float m_LastTimeStamp;

        private bool m_IsInTransition;
        
        private void Awake()
        {
            m_Camera = GetComponentInChildren<Camera>();
            m_PlayerController = GetComponentInChildren<PlayerController>();
            GameManager.Instance.OnPlayerInitialize(this);
            m_SmoothCameraPosition = transform.position;
            m_LastTimeStamp = Time.time;
        }

        public void LevelTransition(Transform targetTransform, bool smooth = false)
        {
            // TODO: Is player able to transit to next level?
            if (!smooth)
            {
                transform.position = targetTransform.position;
                transform.rotation = targetTransform.rotation;
                m_Camera.transform.localPosition = Vector3.zero;
                m_Camera.transform.localRotation = Quaternion.LookRotation(Vector3.zero);
                m_IsInTransition = false;
            }
            else
            {
                m_SmoothCameraPosition = targetTransform.position;
                m_SmoothCameraRotation = targetTransform.rotation;
                m_LastTimeStamp = Time.time;
                m_IsInTransition = true;
            }
        }
        
        private void Update()
        {
            if (m_IsInTransition) UpdateSmoothCameraPosition();
        }

        private void UpdateSmoothCameraPosition()
        {
            //if ((transform.position - m_SmoothCameraPosition).magnitude < 1f &&
            //    Quaternion.Angle(transform.rotation, m_SmoothCameraRotation) < 1f)
            //    m_IsInTransition = false;
            if ((transform.position - m_SmoothCameraPosition).magnitude < 1f)
                m_IsInTransition = false;

            transform.position = Vector3.Slerp(transform.position, m_SmoothCameraPosition,
                m_SmoothCameraSpeed * Time.fixedDeltaTime);
            //transform.rotation = Quaternion.Slerp(transform.rotation, m_SmoothCameraRotation,
            //    m_SmoothCameraSpeed * Time.fixedDeltaTime);
        }

        // TODO: Implement player driving state
        public bool IsAbleToDrive() => true;
        public Camera GetCamera() => m_Camera;
        public PlayerController GetPlayerController() => m_PlayerController;
    }
}
