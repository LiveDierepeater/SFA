using UnityEngine;

namespace Game.Core.LevelLogic
{
    public class OpenWorld : Level
    {
        public override void CameraFade(bool fadeIn) {}

        public override void PlayerTransition(Transform targetPlayerTransform)
        {
            GameManager.Instance.m_Player.LevelTransition(targetPlayerTransform, true);
            OnLevelSwitchingFinished?.Invoke();
        }
    }
}
