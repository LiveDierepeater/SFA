using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [SerializeField] private Transform m_CenterOfMass;
    
    [SerializeField] private Wheel m_Wheel_LF;
    [SerializeField] private Wheel m_Wheel_RF;
    [SerializeField] private Wheel m_Wheel_LB;
    [SerializeField] private Wheel m_Wheel_RB;
    
    [SerializeField] private float m_MotorTorque = 1500f;
    [SerializeField] private float m_MaxSteer = 20f;
    
    private Rigidbody m_Rigidbody;
    
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.centerOfMass = m_CenterOfMass.localPosition;
    }
    
    public void HandleGasInput(float value)
    {
        m_Wheel_LB.ApplyTorque(value * m_MotorTorque);
        m_Wheel_RB.ApplyTorque(value * m_MotorTorque);
    }
    public void HandleGearInput(float value)
    {
        m_Wheel_LF.ApplySteerAngle(value * m_MaxSteer);
        m_Wheel_RF.ApplySteerAngle(value * m_MaxSteer);
    }
}
