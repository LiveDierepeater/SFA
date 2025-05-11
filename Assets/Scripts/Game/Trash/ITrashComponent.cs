public interface ITrashComponent : IInteractable
{
    void IInteractable.OnInteract() => OnInteract();

    public new void OnInteract();
}
