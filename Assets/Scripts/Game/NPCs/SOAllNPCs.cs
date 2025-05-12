using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC List", menuName = "Scriptable Objects/NPC List")]
public class SOAllNPCs : ScriptableObject
{
    [SerializeField] private GameObject m_Grandpa_Prefab;
    [SerializeField] private GameObject m_Racer_Prefab;
    [SerializeField] private GameObject m_Attendant_Prefab;
    
    public GameObject GetPrefabOfNPCType(NPCType npcType) =>
        npcType switch
        {
            NPCType.Grandpa => m_Grandpa_Prefab,
            NPCType.Attendant => m_Attendant_Prefab,
            NPCType.Racer => m_Racer_Prefab,
            _ => throw new ArgumentOutOfRangeException()
        };
}
