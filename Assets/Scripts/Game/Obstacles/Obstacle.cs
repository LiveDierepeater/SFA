using UnityEngine;

public enum ObstacleType
{
    BrokenBridge,
    DamagedBridge,
    FallenTree,
    MuddyRoad,
}

public abstract class Obstacle : LevelActor, ILevelActor
{
    [Header("Audio")]
    [SerializeField] private AudioClip m_OnObstacleNPCInfo;

    protected virtual void TriggerEnter(Collider other) {}

    protected virtual void TriggerStay(Collider other) {}
    protected virtual void TriggerExit(Collider other) {}

    private void OnTriggerEnter(Collider other) => TriggerEnter(other);
    private void OnTriggerStay(Collider other) => TriggerStay(other);
    private void OnTriggerExit(Collider other) => TriggerExit(other);

    protected virtual bool IsAbleToMaster() => true;
    public override void OnResetActor() => ResetObstacle();

    protected virtual void ResetObstacle() {}
    
    protected virtual void HandleAudio()
    { if (!IsAbleToMaster()) NPCManager.GetInstance().GetNPC(NPCType.Grandpa).ReactOnObstacle(m_OnObstacleNPCInfo); }
}
