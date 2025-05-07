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
    
    public float m_MaxSpeed = 4f;
    public float m_MotorTorque = 250f;
    public float m_Acceleration = 4f;
    public float m_MaxSteer = 20f;
    public float m_BaseMass = 50f;
}
