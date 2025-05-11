using System;
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
        [Header("Settings")]
        [SerializeField] private float m_SpeedReduction = 10f;
    
        private List<Wheel> m_Wheels;
        private Rigidbody m_Rigidbody;
        
        private float m_CurrentFuelTankVolume;

        #region Debug

        [Header("Debug")] public ShowDebug DebugMode;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_MaxSpeed;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_MotorTorque;
        [Space(10)]
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentConsumption;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentAcceleration;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentSpeed;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentTraction;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentFuelTankVolume;
        [ConditionalHide("DebugMode", (int)ShowDebug.Show)]    public float DEBUG_CurrentMass;

        #endregion
    
        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.centerOfMass = m_CenterOfMass.localPosition;
        
            m_Wheels = GetComponentsInChildren<Wheel>().ToList();
        }

        private void Start()
        {
            m_CarStats.InitializeStats();
            UpdateMaxSpeed();
            UpdateRigidbodyMass();
            RefillFuelTank();
            UpdateWheelFrictionValues();
        }

        private void FixedUpdate() => HandleCarSlowDown();

        private void HandleCarSlowDown()
        {
            if (!Physics.Raycast(m_CenterOfMass.position, -transform.up, out RaycastHit hit, 5f)) return;
            if (!hit.transform.CompareTag("Ground")) return;
            
            m_Rigidbody.AddForce(-m_Rigidbody.linearVelocity.normalized * m_SpeedReduction, ForceMode.Acceleration);
        }

        private void Update() => UpdateDebugValues();

        public void HandleGasInput(float value)
        {
            if (!HasEnoughFuel())
            {
                foreach (var wheel in m_Wheels.Where(wheel => wheel.IsPoweredWheel()))
                {
                    wheel.ApplyTorque(0f);
                    wheel.ApplyBrakeTorque(3000f);
                }
                return;
            }
            
            foreach (var wheel in m_Wheels.Where(wheel => wheel.IsPoweredWheel()))
                BreakingAndAcceleration(value, wheel);
            
            LimitSpeed();
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
                wheel.ApplyTorque(value * m_CarStats.GetTorque());
                float speedRatio = (Mathf.Abs(m_Rigidbody.linearVelocity.magnitude) / m_Rigidbody.maxLinearVelocity);
                m_Rigidbody.AddForce(m_Rigidbody.transform.forward * (-direction * (m_CarStats.GetAcceleration() * speedRatio)), ForceMode.Acceleration);
                wheel.ApplyBrakeTorque(3000f);

                // Only Consume Fuel when accelerating
                //if (value > 0)
                //    HandleFuelConsumption(value);
            }
            else
            {
                wheel.ApplyTorque(value * m_CarStats.GetTorque());
                m_Rigidbody.AddForce(m_Rigidbody.transform.forward * (m_CarStats.GetAcceleration() * 0.2f * value), ForceMode.Acceleration);
                HandleFuelConsumption(value);
            }

            if (Mathf.Abs(value) < 0.01f)
                wheel.ApplyBrakeTorque(3000f);
            else
                wheel.ApplyBrakeTorque(0f);
        }

        /// <summary>
        /// If the car is going too fast, we want to limit the speed
        /// </summary>
        private void LimitSpeed()
        {
            if (m_Rigidbody.linearVelocity.magnitude > m_CarStats.GetMaxSpeed())
            {
                m_Rigidbody.maxLinearVelocity = m_CarStats.GetMaxSpeed();
                m_Rigidbody.linearVelocity = m_Rigidbody.linearVelocity.normalized * m_CarStats.GetMaxSpeed();
            }
        }

        private void HandleFuelConsumption(float value)
        {
            if (value == 0) return;

            m_CurrentFuelTankVolume =
                Mathf.Clamp(m_CurrentFuelTankVolume - m_CarStats.GetCurrentConsumption() * Time.fixedDeltaTime / 2f,
                    0f,
                    m_CarStats.GetFuelTank().m_Volume);
        }
    
        public Rigidbody GetRigidbody() => m_Rigidbody;
        public CarStats GetCarStats() => m_CarStats;
        public Wheel[] GetWheels() => m_Wheels.ToArray();

        public bool HasEnoughFuel() => m_CurrentFuelTankVolume > 0;
        public float GetCurrentFuelTankVolume() => m_CurrentFuelTankVolume;
        public void RefillFuelTank() => m_CurrentFuelTankVolume = m_CarStats.GetFuelTank().m_Volume;
        public void RefillFuelTank(float amount) => m_CurrentFuelTankVolume += Mathf.Clamp(m_CurrentFuelTankVolume + amount, 0f, m_CarStats.GetFuelTank().m_Volume);

        public void UpdateCarStats(CarComponents carComponents) => m_CarStats.UpdateCarStats(carComponents);
        public void UpdateRigidbodyMass() => m_Rigidbody.mass = m_CarStats.GetMass();
        public void UpdateMaxSpeed() => m_Rigidbody.maxLinearVelocity = m_CarStats.GetMaxSpeed();

        public void UpdateWheelFrictionValues() =>
            m_Wheels.ForEach(wheel =>
            {
                if (wheel.IsSteeringWheel())
                    wheel.UpdateStiffnessValues(m_CarStats.GetTires().m_FF_Front, m_CarStats.GetTires().m_SF_Front);
                else
                    wheel.UpdateStiffnessValues(m_CarStats.GetTires().m_FF_Back, m_CarStats.GetTires().m_SF_Back);
            });

        private void UpdateDebugValues()
        {
            DEBUG_MaxSpeed = m_CarStats.GetMaxSpeed();
            DEBUG_MotorTorque = m_CarStats.GetTorque();
            DEBUG_CurrentConsumption = m_CarStats.GetCurrentConsumption();
            DEBUG_CurrentAcceleration = m_CarStats.GetCurrentAcceleration();
            DEBUG_CurrentFuelTankVolume = m_CurrentFuelTankVolume;
            DEBUG_CurrentMass = m_CarStats.GetMass();
        }

        private void OnDrawGizmosSelected()
        {
            // Draw Ray from HandleCarSlowDown()
            Gizmos.color = Color.red;
            Gizmos.DrawLine(m_CenterOfMass.position, m_CenterOfMass.position + -transform.up * 5f);
        }
    }
}
