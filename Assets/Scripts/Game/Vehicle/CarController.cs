﻿using UnityEngine;

namespace Game.Vehicle
{
    [RequireComponent(typeof(Car))]
    public class CarController : MonoBehaviour
    {
        private Car m_Car;
        
        private void Awake() => m_Car = GetComponent<Car>();

        public void HandleGasInput(float value) => m_Car.HandleGasInput(value);
        public void HandleGearInput(float value) => m_Car.HandleGearInput(value);

        public Car GetCar() => m_Car;
    }
}
