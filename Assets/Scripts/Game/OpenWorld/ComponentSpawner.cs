using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

public class ComponentSpawnerManager
{
    public static ComponentSpawnerManager Instance;
    public ComponentSpawnerManager() => Instance ??= this;
    public static bool IsInitialized => Instance != null;
    
    private readonly List<CarComponent> m_RegisteredComponents = new();
    public void RegisterComponent(CarComponent component)
    {
        if (!m_RegisteredComponents.Contains(component))
            m_RegisteredComponents.Add(component);
        else
            Debug.LogWarning($"Component {component} already registered!");
    }
    public List<CarComponent> GetRegisteredComponents() => m_RegisteredComponents;
}

[RequireComponent(typeof(SphereCollider))]
public class ComponentSpawner : MonoBehaviour, IInteractable
{
    [Header("Internal")]
    [SerializeField] private float m_InteractionSphereRadius = 2f;
    [SerializeField] private float m_SpawnDelay = 1.0f;
    [Header("Spawning")]
    [SerializeField] private GameObject m_CarComponentProxyPrefab;
    [SerializeField] private CarComponent m_CarComponent;
    [SerializeField] private float m_ComponentScale = 3.0f;
    [Header("Audio")]
    [SerializeField] private AudioClip m_OnItemCollected;

    private CarComponentProxy m_SpawnedCarComponent;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(m_SpawnDelay);
        if (!ComponentSpawnerManager.IsInitialized) ComponentSpawnerManager.Instance = new ComponentSpawnerManager();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        if (GameManager.Instance.m_Player.GetComponent<Inventory>().IsComponentInInventory(m_CarComponent))
        {
            Debug.LogWarning($"Component {m_CarComponent} already in inventory!");
            Destroy(gameObject);
        }
        else if (ComponentSpawnerManager.Instance.GetRegisteredComponents().Contains(m_CarComponent))
        {
            Debug.LogWarning($"Component {m_CarComponent} already registered!");
            Destroy(gameObject);
        }
        else
        {
            m_SpawnedCarComponent = Instantiate(m_CarComponentProxyPrefab, transform.position, Quaternion.identity, transform).GetComponent<CarComponentProxy>();
            m_SpawnedCarComponent.transform.localScale = Vector3.one * m_ComponentScale;
            m_SpawnedCarComponent.InitializeCarComponentProxy(m_CarComponent, null, true, true);
            GetComponent<SphereCollider>().radius = m_InteractionSphereRadius;
            
            ComponentSpawnerManager.Instance.RegisterComponent(m_CarComponent);
        }
    }

    public void OnInteract()
    {
        GameManager.Instance.m_Player.GetComponent<Inventory>().AddCarComponentToInventory(m_CarComponent);
        HandleNPCVoiceLines();
        Destroy(gameObject);
    }
    
    private void HandleNPCVoiceLines()
    { if (m_OnItemCollected is not null) NPCManager.GetInstance().GetNPC(NPCType.Grandpa).ReactOnItemCollected(m_OnItemCollected); }

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
