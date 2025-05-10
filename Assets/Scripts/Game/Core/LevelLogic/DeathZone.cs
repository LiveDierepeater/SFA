using System.Collections;
using System.Collections.Generic;
using Game.Core.LevelLogic;
using UnityEngine;

public class DeathZone : LevelSwitcher
{
    [SerializeField] private List<LevelActor> m_ActorsToReset = new();
    
    public override void OnDriveThrough()
    {
        base.OnDriveThrough();
        StartCoroutine(ResetActorsAfterTime(1f));
    }
    
    private IEnumerator ResetActorsAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        
        foreach (var actor in m_ActorsToReset) actor.OnResetActor();
    }
}
