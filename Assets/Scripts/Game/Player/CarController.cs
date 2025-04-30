using System;
using UnityEngine;

namespace Game.Player
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private Transform m_CenterOfMass;
        
        [SerializeField] private Transform m_WheelTransform_LF;
        [SerializeField] private Transform m_WheelTransform_RF;
        [SerializeField] private Transform m_WheelTransform_LB;
        [SerializeField] private Transform m_WheelTransform_RB;
        
        [SerializeField] private WheelCollider m_Wheel_LF;
        [SerializeField] private WheelCollider m_Wheel_RF;
        [SerializeField] private WheelCollider m_Wheel_LB;
        [SerializeField] private WheelCollider m_Wheel_RB;

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
            m_Wheel_LB.motorTorque = value * m_MotorTorque;
            m_Wheel_RB.motorTorque = value * m_MotorTorque;
        }
        public void HandleGearInput(float value)
        {
            m_Wheel_LF.steerAngle = value * m_MaxSteer;
            m_Wheel_RF.steerAngle = value * m_MaxSteer;
        }

        private void FixedUpdate()
        {
            Vector3 pos;
            Quaternion rot;
            
            m_Wheel_LF.GetWorldPose(out pos, out rot);
            m_WheelTransform_LF.position = pos;
            m_WheelTransform_LF.rotation = rot;
            
            m_Wheel_RF.GetWorldPose(out pos, out rot);
            m_WheelTransform_RF.position = pos;
            m_WheelTransform_RF.rotation = rot * Quaternion.Euler(0, 180, 0);
            
            m_Wheel_LB.GetWorldPose(out pos, out rot);
            m_WheelTransform_LB.position = pos;
            m_WheelTransform_LB.rotation = rot;
            
            m_Wheel_RB.GetWorldPose(out pos, out rot);
            m_WheelTransform_RB.position = pos;
            m_WheelTransform_RB.rotation = rot * Quaternion.Euler(0, 180, 0);
        }
    }
}
