using System;
using Game.Core;
using UnityEngine;

namespace Game.Vehicle.Stats
{
    [Serializable]
    public class CarStats
    {
        private SOBonnet m_Bonnet;
        private SOChassis m_Chassis;
        private SOEngine m_Engine;
        private SOFuelTank m_FuelTank;
        private SOTires m_Tires;
        private SOTrunk m_Trunk;
    
        [SerializeField] private SOBaseStats m_BaseStats;

        public void InitializeStats()
        {
            if (m_BaseStats is null) 
                throw new Exception("Base stats are not set. Please set them in the inspector.");
        
            SetBonnet(m_BaseStats.m_Bonnet);
            SetChassis(m_BaseStats.m_Chassis);
            SetEngine(m_BaseStats.m_Engine);
            SetFuelTank(m_BaseStats.m_FuelTank);
            SetTires(m_BaseStats.m_Tires);
            SetTrunk(m_BaseStats.m_Trunk);

            AddComponentToInventory(m_Bonnet);
            AddComponentToInventory(m_Chassis);
            AddComponentToInventory(m_Engine);
            AddComponentToInventory(m_FuelTank);
            AddComponentToInventory(m_Tires);
            AddComponentToInventory(m_Trunk);
        }
    
        public float GetMaxSpeed() => m_BaseStats.m_MaxSpeed * m_Engine.m_Speed;
        public float GetTorque() => m_BaseStats.m_MotorTorque * m_Engine.m_Torque;
        public float GetAcceleration() => m_BaseStats.m_Acceleration * m_Engine.m_Acceleration;
        public float GetMaxSteer() => m_BaseStats.m_MaxSteer;
        public float GetMass() => (m_Engine.m_Mass + m_FuelTank.m_Mass + m_Tires.m_Mass + m_Trunk.m_Mass + m_Bonnet.m_Mass + m_Chassis.m_Mass) * m_BaseStats.m_BaseMass;
        public float GetAerodynamics() => m_Tires.m_Aerodynamics + m_Trunk.m_Aerodynamics + m_Bonnet.m_Aerodynamics + m_Chassis.m_Aerodynamics;
    
        public void SetBonnet(SOBonnet bonnet) => m_Bonnet = bonnet;
        public void SetChassis(SOChassis chassis) => m_Chassis = chassis;
        public void SetEngine(SOEngine engine) => m_Engine = engine;
        public void SetFuelTank(SOFuelTank fuelTank) => m_FuelTank = fuelTank;
        public void SetTires(SOTires tires) => m_Tires = tires;
        public void SetTrunk(SOTrunk trunk) => m_Trunk = trunk;
    
        public SOBonnet GetBonnet() => m_Bonnet;
        public SOChassis GetChassis() => m_Chassis;
        public SOEngine GetEngine() => m_Engine;
        public SOFuelTank GetFuelTank() => m_FuelTank;
        public SOTires GetTires() => m_Tires;
        public SOTrunk GetTrunk() => m_Trunk;
    
        public SOBaseStats GetBaseStats() => m_BaseStats;

        private void AddComponentToInventory(CarComponent carComponent) => 
            GameManager.Instance.m_Player.GetComponent<Inventory>().AddCarComponentToInventory(carComponent);
    }
}
