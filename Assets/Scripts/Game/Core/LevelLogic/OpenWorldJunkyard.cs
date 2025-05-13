using Game.Core;
using Game.Core.LevelLogic;
using UnityEngine;

public class OpenWorldJunkyard : Level
{
    public override void PlayerTransition(Transform targetPlayerTransform)
    {
        GameManager.Instance.m_Player.LevelTransition(targetPlayerTransform);
        OnLevelSwitchingFinished?.Invoke();
    }
}
