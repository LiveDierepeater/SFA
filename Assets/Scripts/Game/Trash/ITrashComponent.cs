using UnityEngine;

public interface ITrashComponent : IInteractable
{
    void IInteractable.OnInteract() => OnInteract();

    public new void OnInteract();
    
    public void Initialize(CarComponent carComponent = null, Material material = null, TrashSpawner spawner = null);
    public CarComponent GetCarComponent();
}
