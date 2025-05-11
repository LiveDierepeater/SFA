using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOAllComponents", menuName = "Scriptable Objects/SOAllComponents")]
public class SOAllComponents : ScriptableObject
{
    [SerializeField] private List<CarComponent> m_CarComponentsList;
    public List<CarComponent> GetCarComponentsList() => m_CarComponentsList;
}
