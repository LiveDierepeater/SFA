using Game.Core;
using UnityEngine;

public enum TrashState
{
    Free,
    Grabbed
}

public class Trash : MonoBehaviour, ITrashComponent
{
    private CarComponent m_CarComponent;
    
    private TrashState m_TrashState = TrashState.Free;
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private MeshFilter m_MeshFilter;
    private MeshRenderer m_MeshRenderer;
    private TrashSpawner m_TrashSpawner;
    
    private Vector3 m_LastPosition;

    private void Awake() => AutoInitialize();
    private void AutoInitialize()
    {
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        m_Collider = GetComponentInChildren<Collider>();
        m_MeshFilter = GetComponentInChildren<MeshFilter>();
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_Rigidbody.mass = 20f;
    }
    public void Initialize(CarComponent carComponent = null, Material material = default(Material), TrashSpawner spawner = null)
    {
        m_CarComponent = carComponent;
        m_TrashSpawner = spawner;
        
        if (m_CarComponent is null) return;
        ComponentSpawnerManager.Instance.RegisterComponent(m_CarComponent);
        m_MeshRenderer.material = material;
        //m_MeshFilter.sharedMesh = m_CarComponent.m_Collision;
    }

    public CarComponent GetCarComponent() => m_CarComponent;
    public TrashState GetTrashState() => m_TrashState;
    private void SetTrashState(TrashState state)
    {
        m_TrashState = state;
        if (m_TrashState == TrashState.Grabbed)
        {
            m_Collider.enabled = false;
            m_Rigidbody.isKinematic = true;
        }
        else
        {
            m_Collider.enabled = true;
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
}
