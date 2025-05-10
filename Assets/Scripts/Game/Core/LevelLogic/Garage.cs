using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.LevelLogic
{
    public class Garage : Level
    {
        [SerializeField] private CarProxy m_CarProxy;
        [SerializeField] private Transform m_CarComponentsSpawnPoint;
        [SerializeField] private GameObject m_CarComponentProxyPrefab;
        [SerializeField] [Range(0, 10)] private float m_RandomSpawnRange = 2f;

        private readonly List<Transform> m_SpawnedCarComponents = new();

        protected override void BeforeLevelLoad()
        {
            base.BeforeLevelLoad();
            DestroyAllCarComponents();
            m_CarProxy.UpdateCarProxy();
            SpawnCarComponents();
        }
        
        private void DestroyAllCarComponents()
        {
            foreach (var component in m_SpawnedCarComponents.Where(component => component != null))
                Destroy(component.gameObject);
            m_SpawnedCarComponents.Clear();
        }
        private void SpawnCarComponents()
        {
            var carComponentsToSpawn = GetCarComponentsToSpawn();
            foreach (var component in carComponentsToSpawn)
            {
                var carComponentProxy = Instantiate(m_CarComponentProxyPrefab, m_CarComponentsSpawnPoint.position, Quaternion.identity);
                carComponentProxy.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(component, m_CarProxy);
                
                // Random Spawn Offset 
                carComponentProxy.transform.position += new Vector3(Random.Range(-m_RandomSpawnRange, m_RandomSpawnRange), 0f, 0f);
                
                m_SpawnedCarComponents.Add(carComponentProxy.transform);
            }
        }
        private static List<CarComponent> GetCarComponentsToSpawn()
        {
            var carComponentsToSpawn = GameManager.Instance.m_Player.GetComponent<Inventory>().GetCarComponents();
            foreach (var component in GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetCarComponents())
            {
                if (carComponentsToSpawn.Contains(component))
                    carComponentsToSpawn.Remove(component);
            }
            return carComponentsToSpawn;
        }

        public void AddCarComponentToDeleteLater(Transform carComponent)
        {
            if (m_SpawnedCarComponents.Contains(carComponent)) return;
            m_SpawnedCarComponents.Add(carComponent);
        }
        
        public CarProxy GetCarProxy() => m_CarProxy;
    }
}

// TODO: Start()->UpdateCarProxy()

// TODO: Spawn car components when player enters garage                     -> BeforeLevelLoad() -> SpawnCarComponents()
// TODO: Grabbing and moving car components
// TODO: Removing car components                                            -> UpdateCarProxy()
// TODO: Adding car components                                              -> UpdateCarProxy()
// TODO: Apply Car Proxy's stats to the car                                 -> UpdateCar()
// TODO: Recalculate Stats when it's not already automatically done         -> UpdateCarStats()
