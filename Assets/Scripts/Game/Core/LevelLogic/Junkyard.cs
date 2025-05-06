using Game.Player;
using Game.Vehicle;
using UnityEngine;

namespace Game.Core.LevelLogic
{
    public class Junkyard : Level
    {
        [SerializeField] private Transform m_VehicleSpawnPoint;
        
        public void TeleportCarToJunkyard()
        {
            Car car = GameManager.Instance.m_Player.GetComponent<PlayerController>().GetCarController().GetCar();
            GameManager.Instance.m_Player.GetComponent<PlayerController>().DisableCarInput();
                
            HandleVehicleRespawning(car);
            HandleVehicleVelocityNeutralization(car);
        }
        private void HandleVehicleRespawning(Car car)
        {
            if (car is null) return;
        
            car.transform.position = m_VehicleSpawnPoint.position;
            car.transform.rotation = m_VehicleSpawnPoint.rotation;
        }
        private static void HandleVehicleVelocityNeutralization(Car car)
        {
            if (car is null) return;
        
            car.GetRigidbody().linearVelocity = Vector3.zero;
            car.GetRigidbody().angularVelocity = Vector3.zero;
        }
    }
}
