using UnityEngine;

public interface INPC : IInteractable
{
    void IInteractable.OnInteract() => ReactOnInteract();
    public void ReactOnInteract();
    public void ReactOnLevelLoadFirstTime(AudioClip clip);
    public void ReactOnLevelLoad(AudioClip clip);
    public void ReactOnLevelSwitch(AudioClip clip);
    public void ReactOnObstacle(AudioClip clip);
    public void ReactOnItemCollected(AudioClip clip);
    public void ReactOnCarModification(AudioClip clip);
    public void ReactOnFuelState(AudioClip clip);
}
