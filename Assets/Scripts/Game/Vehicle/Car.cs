using System.Collections.Generic;
using System.Linq;
using Game.Vehicle.Stats;
using UnityEngine;

namespace Game.Vehicle
{
    public enum ShowDebug
    {
        Hide,
        Show
    }

    [RequireComponent(typeof(Rigidbody))]
    public class Car : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform m_CenterOfMass;
        [SerializeField] private CarStats m_CarStats;
    
        private List<Wheel> m_Wheels;
        private Rigidbody m_Rigidbody;

        #region Debug

        [Header("Debug")] public ShowDebug DebugMode;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_MaxSpeed;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_MotorTorque;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_Acceleration;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_MaxSteer;
        [Space(10)]
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentConsumption;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentAcceleration;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentSpeed;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentTraction;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentFuelTankVolume;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentMass;

        #endregion
    
        // more mass -> more consumption
        // more aerodynamics -> less consumption
        // more weight -> less acceleration
        // more aerodynamics -> more acceleration
    
        // a consumption of 1 is enough to cross 1 map tile
        // affected value = base value + adaption value * affection scale
    
        // aerodynamic has a lot of influence on the car
        // racing tires have more sideways friction
    
        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.centerOfMass = m_CenterOfMass.localPosition;
        
            m_Wheels = GetComponentsInChildren<Wheel>().ToList();
        
            m_CarStats.InitializeStats();
            m_Rigidbody.maxLinearVelocity = m_CarStats.GetMaxSpeed();
        }

        private void Update() => UpdateDebugValues();

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
                wheel.ApplyTorque(value * m_CarStats.GetTorque());
                float speedRatio = (Mathf.Abs(m_Rigidbody.linearVelocity.magnitude) / m_Rigidbody.maxLinearVelocity);
                m_Rigidbody.AddForce(m_Rigidbody.transform.forward * (-direction * (m_CarStats.GetAcceleration() * speedRatio)), ForceMode.Acceleration);
            }
            else
            {
                wheel.ApplyTorque(value * m_CarStats.GetTorque());
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
    
        private void UpdateDebugValues()
        {
            DEBUG_MaxSpeed = m_CarStats.GetMaxSpeed();
            DEBUG_MotorTorque = m_CarStats.GetTorque();
            DEBUG_Acceleration = m_CarStats.GetAcceleration();
            DEBUG_MaxSteer = m_CarStats.GetMaxSteer();

            DEBUG_CurrentConsumption =
                m_CarStats.GetEngine().m_Consumption
                * (1f + m_CarStats.GetMass() / 6f * m_CarStats.GetBaseStats().m_BaseMass)
                * (1f - m_CarStats.GetAerodynamics() / 12f);
        
            DEBUG_CurrentAcceleration =
                m_CarStats.GetTorque()
                * (1f + m_CarStats.GetAerodynamics() / 1.2f)
                / m_CarStats.GetMass();

            DEBUG_CurrentSpeed =
                m_CarStats.GetTorque() / m_CarStats.GetMass()
                * (1f + m_CarStats.GetAerodynamics() / 1.2f);

            DEBUG_CurrentTraction =
                m_CarStats.GetTires().m_Traction
                + m_CarStats.GetMass() / 6f * m_CarStats.GetBaseStats().m_BaseMass;
        
            DEBUG_CurrentFuelTankVolume =
                m_CarStats.GetFuelTank().m_Volume
                + m_CarStats.GetMass() / 6f * m_CarStats.GetBaseStats().m_BaseMass;
        
            DEBUG_CurrentMass = m_CarStats.GetMass();
        }
    }
}
