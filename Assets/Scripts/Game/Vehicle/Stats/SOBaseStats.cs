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
    
    public float m_MaxSpeed = 12f;
    public float m_MotorTorque = 500f;
    public float m_Acceleration = 10f;
    public float m_MaxSteer = 20f;
}
