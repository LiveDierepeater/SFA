public enum NPCReactionType
{
    Accepted,
    Denied
}

public enum CarModificationType
{
    Modifying,
    OutOfFuel
}

public interface INPC : IInteractable
{
    void IInteractable.OnInteract() => ReactOnInteract();
    public new void ReactOnInteract();
    
    public void ReactOnLevelLoadFirstTime();
    public void ReactOnLevelLoad();
    public void ReactOnLevelSwitch(NPCReactionType reactionType);
    public void ReactOnObstacle(ObstacleType obstacleType);
    public void ReactOnItemCollected();
    public void ReactOnCarModification(CarModificationType carModificationType);
}
