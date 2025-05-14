using System.Collections.Generic;
using Game.Core;
using UnityEngine;

public enum TrashState
{
    Free,
    Grabbed
}

public class Trash : MonoBehaviour, ITrashComponent
{
    [SerializeField] private List<GameObject> m_TrashPrefabs;
    
    private CarComponent m_CarComponent;
    private TrashSpawner m_TrashSpawner;
    
    private TrashState m_TrashState = TrashState.Free;
    private Rigidbody m_Rigidbody;
    //private MeshCollider m_MeshCollider;
    //private MeshFilter m_MeshFilter;
    //private MeshRenderer m_MeshRenderer;
    
    private Vector3 m_LastPosition;

    private void Awake() => AutoInitialize();
    private void AutoInitialize()
    {
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        m_Rigidbody.mass = 20f;
        
        UpdateMesh();
    }
    public void Initialize(CarComponent carComponent = null, Material material = default(Material), TrashSpawner spawner = null)
    {
        m_CarComponent = carComponent;
        m_TrashSpawner = spawner;
        
        if (m_CarComponent is null) return;
        ComponentSpawnerManager.Instance.RegisterComponent(m_CarComponent);

        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            var tmp = Instantiate(m_CarComponent.m_Mesh, transform.position, Quaternion.identity, transform);
        }
    }

    public CarComponent GetCarComponent() => m_CarComponent;
    public TrashState GetTrashState() => m_TrashState;
    private void SetTrashState(TrashState state)
    {
        m_TrashState = state;
        if (m_TrashState == TrashState.Grabbed)
        {
            m_Rigidbody.isKinematic = true;
        }
        else
        {
            m_Rigidbody.isKinematic = false;
        }
    }

    public void OnInteract()
    {
        SetTrashState(TrashState.Grabbed);
        GameManager.Instance.m_Player.GetPlayerController().OnPrimaryActionHold += UpdatePosition;
        GameManager.Instance.m_Player.GetPlayerController().OnPrimaryActionReleased += SettleState;
    }

    private void SettleState()
    {
        GameManager.Instance.m_Player.GetPlayerController().OnPrimaryActionHold -= UpdatePosition;
        GameManager.Instance.m_Player.GetPlayerController().OnPrimaryActionReleased -= SettleState;
        SetTrashState(TrashState.Free);
        m_Rigidbody.AddForce(transform.position - m_LastPosition, ForceMode.Impulse);

        if (m_CarComponent is not null) CollectComponent();
    }
    private void CollectComponent()
    { 
        if (!GameManager.Instance.m_Player.GetComponent<Inventory>().IsComponentInInventory(m_CarComponent))
            GameManager.Instance.m_Player.GetComponent<Inventory>().AddCarComponentToInventory(m_CarComponent);
        HandleAudio();
        Destroy(gameObject);
    }
    private void HandleAudio()
    {
        if (m_TrashSpawner is null) return;
        
        var clip = m_TrashSpawner.GetComponent<TrashSpawner>().GetOnItemCollectedAudioClip();
        if (clip is null) return;
        
        NPCManager.GetInstance().GetNPC(NPCType.Grandpa).ReactOnItemCollected(clip);
    }

    private void UpdatePosition()
    {
        if (!Physics.Raycast(GameManager.Instance.m_Player.GetCamera().ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 999.0f)) return;
        var targetPosition = hit.point;
        
        m_LastPosition = transform.position;
        transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }

    private void UpdateMesh() => Instantiate(m_TrashPrefabs[Random.Range(0, m_TrashPrefabs.Count)], transform.position, Quaternion.identity, transform);
}
