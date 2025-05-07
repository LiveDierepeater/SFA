using System.Collections;
using Game.Core;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ComponentSpawner : MonoBehaviour, IInteractable
{
    [Header("Internal")]
    [SerializeField] private float m_InteractionSphereRadius = 2f;
    [SerializeField] private float m_SpawnDelay = 1.0f;
    [Header("Spawning")]
    [SerializeField] private GameObject m_CarComponentProxyPrefab;
    [SerializeField] private CarComponent m_CarComponent;

    private CarComponentProxy m_SpawnedCarComponent;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(m_SpawnDelay);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        if (GameManager.Instance.m_Player.GetComponent<Inventory>().IsComponentInInventory(m_CarComponent))
        {
            Debug.LogWarning($"Component {m_CarComponent} already in inventory!");
            Destroy(gameObject);
        }
        else
        {
            m_SpawnedCarComponent = Instantiate(m_CarComponentProxyPrefab, transform.position, Quaternion.identity, transform).GetComponent<CarComponentProxy>();
            m_SpawnedCarComponent.InitializeCarComponentProxy(m_CarComponent, null, true, true);
            GetComponent<SphereCollider>().radius = m_InteractionSphereRadius;
        }
    }

    public void OnInteract()
    {
        GameManager.Instance.m_Player.GetComponent<Inventory>().AddCarComponentToInventory(m_CarComponent);
        Destroy(gameObject);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_InteractionSphereRadius);
        if (m_SpawnedCarComponent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, m_SpawnedCarComponent.transform.position);
        }
    }
}
