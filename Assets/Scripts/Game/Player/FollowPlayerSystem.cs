using System.Collections;
using Game.Core;
using UnityEngine;

namespace Game.Player
{
    public class FollowPlayerSystem : MonoBehaviour
    {
        [SerializeField] private Transform m_StandardRotation_ForRef;
        [SerializeField] private float m_CameraSpeed = 2f;
        
        private Transform m_TargetTransform;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            Initialize();
        }
        private void Initialize() => m_TargetTransform = GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().transform;

        private void Update() => UpdateCameraRotation();

        private void UpdateCameraRotation()
        {
            if (!IsActive()) return;
            
            Vector3 directionToPlayer = (m_TargetTransform.position - transform.position);
            
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_CameraSpeed);
        }

        private static bool IsActive() => UIManager.Instance.IsOpenWorldUIActive();
    }
}
