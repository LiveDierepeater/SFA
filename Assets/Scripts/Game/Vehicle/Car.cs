using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_CenterOfMass;
    
    private List<Wheel> m_Wheels;
    
    //[Header("Stats")]
    //[SerializeField] [Range(1, 30)] private float m_MaxSpeed = 12f;
    //[SerializeField] private float m_MotorTorque = 500f;
    //[SerializeField] [Range(1, 100)] private float m_Acceleration = 10f;
    //[SerializeField] [Range(0, 60)] private float m_MaxSteer = 20f;
    [SerializeField] private CarStats m_CarStats;
    
    private Rigidbody m_Rigidbody;
    
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.centerOfMass = m_CenterOfMass.localPosition;
        
        m_Wheels = GetComponentsInChildren<Wheel>().ToList();
        
        m_CarStats.InitializeStats();
        m_Rigidbody.maxLinearVelocity = m_CarStats.GetMaxSpeed();
    }

    public void HandleGasInput(float value)
    {
        foreach (var wheel in m_Wheels)
        {
            // If the wheel is not powered, we don't want to apply torque to it
            if (!wheel.IsPoweredWheel()) continue;
            
            BreakingAndAcceleration(value, wheel);
            LimitSpeed();
        }
    }
    public void HandleGearInput(float value)
    {
        foreach (var wheel in m_Wheels.Where(wheel => wheel.IsSteeringWheel()))
            wheel.ApplySteerAngle(value * m_CarStats.GetMaxSteer());
    }

    /// <summary>
    /// Handles the breaking and acceleration of the car
    /// </summary>
    /// <param name="value"></param>
    /// <param name="wheel"></param>
    private void BreakingAndAcceleration(float value, Wheel wheel)
    {
        float direction = Vector3.Dot(transform.forward, m_Rigidbody.linearVelocity.normalized);
        
        // If the car rolls forwards but the input is backwards, we want to simulate braking the car OR
        // If the car rolls backwards but the input is forwards, we want to accelerate the car quicker
        if (direction > 0 && value < 0 || direction < 0 && value > 0)
        {
            //wheel.ApplyTorque(value * m_Acceleration * m_MotorTorque);
            wheel.ApplyTorque(value * m_CarStats.GetMotorTorque());
            float speedRatio = (Mathf.Abs(m_Rigidbody.linearVelocity.magnitude) / m_Rigidbody.maxLinearVelocity);
            m_Rigidbody.AddForce(m_Rigidbody.transform.forward * (-direction * (m_CarStats.GetAcceleration() * speedRatio)), ForceMode.Acceleration);
        }
        else
        {
            wheel.ApplyTorque(value * m_CarStats.GetMotorTorque());
            m_Rigidbody.AddForce(m_Rigidbody.transform.forward * (m_CarStats.GetAcceleration() * 0.2f * value), ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// If the car is going too fast, we want to limit the speed
    /// </summary>
    private void LimitSpeed()
    {
        if (m_Rigidbody.linearVelocity.magnitude > m_CarStats.GetMaxSpeed())
            m_Rigidbody.linearVelocity = m_Rigidbody.linearVelocity.normalized * m_CarStats.GetMaxSpeed();
    }
    
    public Rigidbody GetRigidbody() => m_Rigidbody;
}
