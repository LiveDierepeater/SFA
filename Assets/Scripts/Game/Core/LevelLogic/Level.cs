using Game.Core.Interfaces;
using UnityEngine;

namespace Game.Core.LevelLogic
{
    public abstract class Level : MonoBehaviour, ILevel
    {
        [SerializeField] private Transform m_targetTransform;
        [SerializeField] private Transform m_resetTransform;

        public delegate void LevelSwitching();
        public LevelSwitching OnLevelSwitchingAccepted;
        public LevelSwitching OnLevelSwitchingFinished;

        public virtual void SwitchToLevel() => MovePlayerToLevel(m_targetTransform);
        public virtual void ResetToLevel() => MovePlayerToLevel(m_resetTransform);
    
        public virtual void MovePlayerToLevel(Transform targetPlayerTransform)
        {
            OnLevelSwitchingAccepted?.Invoke();
            CameraFade();
            PlayerTransition(targetPlayerTransform);
        }

        public void PlayerTransition(Transform targetPlayerTransform)
        {
            GameManager.Instance.Player.LevelTransition(targetPlayerTransform);
            OnLevelSwitchingFinished?.Invoke();
        }

        // TODO: Implement camera fade through UI
        public void CameraFade() => UIManager.Instance.FadeToNextLevel_Coroutine();
    }
}
