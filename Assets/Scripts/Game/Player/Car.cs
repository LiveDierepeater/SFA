using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_CenterOfMass;
    
    private List<Wheel> m_Wheels;
    
    [Header("Stats")]
    [SerializeField] private float m_MotorTorque = 1500f;
    [SerializeField] private float m_MaxSteer = 20f;
    
    private Rigidbody m_Rigidbody;
    
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.centerOfMass = m_CenterOfMass.localPosition;
        
        m_Wheels = GetComponentsInChildren<Wheel>().ToList();
    }

    public void HandleGasInput(float value)
    {
        foreach (var wheel in m_Wheels.Where(wheel => wheel.IsPoweredWheel()))
            wheel.ApplyTorque(value * m_MotorTorque);
    }
    public void HandleGearInput(float value)
    {
        foreach (var wheel in m_Wheels.Where(wheel => wheel.IsSteeringWheel()))
            wheel.ApplySteerAngle(value * m_MaxSteer);
    }
}
