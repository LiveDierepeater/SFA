using UnityEngine;

public enum NPCType
{
    Grandpa,
    Attendant,
    Racer
}

public class NPC : MonoBehaviour, INPC
{
    [Header("Components")]
    [SerializeField] private SOAllNPCs m_NPC_List;
    [Header("Settings")]
    [SerializeField] private NPCType m_NPC_Type;

    private void Awake() => Initialize();
    private void Initialize() => Instantiate(m_NPC_List.GetPrefabOfNPCType(m_NPC_Type), transform.position, transform.rotation, transform);
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * 2f);
    }

    public void ReactOnInteract() => OnInteract();
    public virtual void OnInteract() {}

    public void ReactOnLevelLoadFirstTime() => OnLevelLoadFirstTime();
    protected virtual void OnLevelLoadFirstTime() {}

    public void ReactOnLevelLoad() => OnLevelLoad();
    protected virtual void OnLevelLoad() {}

    public void ReactOnLevelSwitch(NPCReactionType reactionType) => OnLevelSwitch(reactionType);
    protected virtual void OnLevelSwitch(NPCReactionType reactionType) {}

    public void ReactOnObstacle(ObstacleType obstacleType) => OnObstacle(obstacleType);
    protected virtual void OnObstacle(ObstacleType obstacle) {}

    public void ReactOnItemCollected() => OnItemCollected();
    protected virtual void OnItemCollected() {}

    public void ReactOnCarModification(CarModificationType carModificationType) => OnCarModification(carModificationType);
    protected virtual void OnCarModification(CarModificationType carModificationType) {}
}
