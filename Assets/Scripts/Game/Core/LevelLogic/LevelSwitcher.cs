using System;
using Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game.Core.LevelLogic
{
    public enum SwitcherType
    {
        Level,
        Scene
    }
    
    public enum TriggerType
    {
        Mouse,
        Vehicle
    }

    public enum InputState
    {
        None,
        Disabled,
        Enabled,
    }
    
    public sealed class LevelSwitcher : MonoBehaviour, IInteractable
    {
        private bool m_interactable = true;
    
        [SerializeField] private TriggerType m_TriggerType;
        [SerializeField] private SwitcherType m_SwitcherType;
    
        [ConditionalHide("m_SwitcherType", (int)SwitcherType.Level)]
        [SerializeField] private Level m_NextLevel;
        [ConditionalHide("m_SwitcherType", (int)SwitcherType.Scene)]
        [SerializeField] private string m_NextSceneName;

        [Header("Respawn Vehicle System")]
        [SerializeField] private Transform m_VehicleSpawnPoint;
        [SerializeField] private bool m_RespawnVehicle;
        [SerializeField] private bool m_NeutralizeVehicleVelocity;
        [SerializeField] private InputState m_CarControllerState;
        
        private void Start() => InitializeLevelSwitchingLogic();
        private void InitializeLevelSwitchingLogic()
        {
            m_NextLevel.OnLevelSwitchingAccepted += DisableInteractability;
            m_NextLevel.OnLevelSwitchingFinished += EnableInteractability;
        }
        
        public void OnInteract()
        {
            if (!m_interactable) return;

            switch (m_TriggerType)
            {
                case TriggerType.Mouse:
                    EvaluateSwitcherType();
                    break;
                
                case TriggerType.Vehicle:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnTriggerEnter(Collider other) { if (m_interactable && other.CompareTag("Player")) OnDriveThrough(); }
        private void OnDriveThrough() => EvaluateSwitcherType();
        
        private void EvaluateSwitcherType()
        {
            switch (m_SwitcherType)
            {
                case SwitcherType.Level:
                    if (m_RespawnVehicle)
                    {
                        Car car = GameManager.Instance.Player.GetComponent<PlayerController>().GetCarController().GetCar();
                        if (car is not null)
                        {
                            car.transform.position = m_VehicleSpawnPoint.position;
                            car.transform.rotation = m_VehicleSpawnPoint.rotation;

                            if (m_NeutralizeVehicleVelocity)
                            {
                                car.GetRigidbody().linearVelocity = Vector3.zero;
                                car.GetRigidbody().angularVelocity = Vector3.zero;
                                
                            }
                        }
                    }
                    
                    switch (m_CarControllerState)
                    {
                        case InputState.Disabled:
                            GameManager.Instance.Player.GetComponent<PlayerController>().DisableCarInput();
                            break;
                        case InputState.Enabled:
                            GameManager.Instance.Player.GetComponent<PlayerController>().EnableCarInput();
                            break;
                        
                        case InputState.None:
                            break;
                        
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    m_NextLevel.SwitchToLevel();
                    break;
            
                case SwitcherType.Scene:
                    SceneManager.LoadScene(m_NextSceneName);
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void DisableInteractability() =>  m_interactable = false;
        private void EnableInteractability() =>  m_interactable = true;
    }
}
