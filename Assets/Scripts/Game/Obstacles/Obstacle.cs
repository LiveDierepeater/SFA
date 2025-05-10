using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected virtual void TriggerEnter(Collider other) {}
    protected virtual void TriggerStay(Collider other) {}
    protected virtual void TriggerExit(Collider other) {}

    private void OnTriggerEnter(Collider other) => TriggerEnter(other);
    private void OnTriggerStay(Collider other) => TriggerStay(other);
    private void OnTriggerExit(Collider other) => TriggerExit(other);

    protected virtual bool IsAbleToMaster() => true;
}
