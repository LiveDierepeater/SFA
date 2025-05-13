using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum NPCType
{
    Grandpa,
    Attendant,
    Racer
}

[RequireComponent(typeof(AudioSource))]
public abstract class NPC : MonoBehaviour, INPC
{
    [Header("Components")]
    [SerializeField] private SOAllNPCs m_NPC_List;
    [Header("Settings")]
    [SerializeField] private NPCType m_NPC_Type;
    
    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> m_OnInteract;

    private AudioSource m_AudioSource;
    
    private void Awake() => Initialize();
    private void Initialize()
    {
        NPCManager.GetInstance().RegisterNPC(this);
        m_AudioSource = GetComponentInChildren<AudioSource>();
        Instantiate(m_NPC_List.GetPrefabOfNPCType(m_NPC_Type), transform.position, transform.rotation, transform);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * 2f);
    }

    public void ReactOnInteract() => OnReactInteract();
    protected virtual void OnReactInteract() => PlayAudioClip(m_OnInteract[Random.Range(0, m_OnInteract.Count)]);

    public void ReactOnLevelLoadFirstTime(AudioClip clip) => OnLevelLoadFirstTime(clip);
    protected virtual void OnLevelLoadFirstTime(AudioClip clip) => PlayAudioClip(clip);

    public void ReactOnLevelLoad(AudioClip clip) => OnLevelLoad(clip);
    protected virtual void OnLevelLoad(AudioClip clip) => PlayAudioClip(clip);

    public void ReactOnLevelSwitch(AudioClip clip) => OnLevelSwitch(clip);
    protected virtual void OnLevelSwitch(AudioClip clip) => PlayAudioClip(clip);

    public void ReactOnObstacle(AudioClip clip) => OnObstacle(clip);
    protected virtual void OnObstacle(AudioClip clip) => PlayAudioClip(clip);

    public void ReactOnItemCollected(AudioClip clip) => OnItemCollected(clip);
    protected virtual void OnItemCollected(AudioClip clip) => PlayAudioClip(clip);

    public void ReactOnCarModification(AudioClip clip) => OnCarModification(clip);
    protected virtual void OnCarModification(AudioClip clip) => PlayAudioClip(clip);
    
    public void ReactOnFuelState(AudioClip clip) => OnFuelState(clip);
    protected virtual void OnFuelState(AudioClip clip) => PlayAudioClip(clip);

    protected void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip is null) return;
        
        //m_AudioSource.time = 0f;
        m_AudioSource.clip = audioClip;
        m_AudioSource.Play();
    }
    
    public NPCType GetNPCType() => m_NPC_Type;
    
    public enum ReactionType
    {
        Accepted,
        Denied
    }
}

public class NPCManager
{
    private static NPCManager Instance;
    public static NPCManager GetInstance()
    {
        if (Instance is null) Instance = new NPCManager();

        return Instance;
    }
    
    private readonly List<NPC> m_RegisteredNPCs = new List<NPC>();

    public void RegisterNPC(NPC npc)
    {
        if (m_RegisteredNPCs.Contains(npc)) return;
        m_RegisteredNPCs.Add(npc);
    }
    public NPC GetNPC(NPCType npcType) =>
        npcType switch
        {
            NPCType.Grandpa => m_RegisteredNPCs.FirstOrDefault(npc => npc.GetNPCType() == NPCType.Grandpa),
            NPCType.Attendant => m_RegisteredNPCs.FirstOrDefault(npc => npc.GetNPCType() == NPCType.Attendant),
            NPCType.Racer => m_RegisteredNPCs.FirstOrDefault(npc => npc.GetNPCType() == NPCType.Racer),
            _ => throw new System.ArgumentOutOfRangeException()
        };
}
