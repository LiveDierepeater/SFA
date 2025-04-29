using UnityEngine;

public abstract class Level : MonoBehaviour, ILevel
{
    public virtual void SwitchToLevel() => print("Switch to level...");
}
