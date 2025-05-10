using UnityEngine;

[CreateAssetMenu(fileName = "SOBaseStats", menuName = "Scriptable Objects/SOBaseStats")]
public class SOBaseStats : ScriptableObject
{
    public SOEngine m_Engine;
    public SOFuelTank m_FuelTank;
    public SOTires m_Tires;
    public SOTrunk m_Trunk;
    public SOBonnet m_Bonnet;
    public SOChassis m_Chassis;
    
    public float m_MaxSpeed = 7f;
    public float m_MotorTorque = 250f;
    public float m_Acceleration = 4f;
    public float m_MaxSteer = 20f;
    public float m_BaseMass = 50f;
    public float m_BaseConsumption = 0.1f;

    public float GetMaxPossibleMass() => 18f * m_BaseMass;
    public float GetMinPossibleMass() => 6f * m_BaseMass;
    public float GetStandardMass() => (GetMinPossibleMass() + GetMaxPossibleMass()) * 0.5f;
    public float GetMaxPossibleAerodynamics() => 12f;
    public float GetMinPossibleAerodynamics() => 0f;
    public float GetStandardAerodynamics() => (GetMinPossibleAerodynamics() + GetMaxPossibleAerodynamics()) * 0.5f;
}
