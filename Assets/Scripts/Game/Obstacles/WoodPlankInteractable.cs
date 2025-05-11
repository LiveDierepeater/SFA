using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WoodPlankInteractable : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    
    private void Awake() => m_Rigidbody = GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.useGravity = true;
            m_Rigidbody.AddForce(new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1)), ForceMode.Impulse);
        }
    }
}
