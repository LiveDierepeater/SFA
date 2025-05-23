using UnityEngine;

namespace Game.Core.Interfaces
{
    public interface ILevel
    {
        public void SwitchToLevel();
        public void ResetToLevel();
        public void PlayerTransition(Transform targetPlayerTransform);
        public void CameraFade(bool fadeIn);
    }
}
