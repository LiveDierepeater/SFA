﻿using System;
using System.Collections.Generic;
using Game.Player;
using Game.Vehicle;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public enum UIHandling
    {
        None,
        Disable,
        Enable,
    }
    public enum UITypes
    {
        None,
        OpenWorldUI,
        JunkyardUI,
        GarageUI,
        GarbageUI,
        FuelStationUI,
        RacingUI
    }
    public enum AudioType
    {
        None,
        UseAudio
    }
    
    public class LevelSwitcher : MonoBehaviour, IInteractable
    {
        private bool m_interactable = true;
    
        [SerializeField] protected TriggerType m_TriggerType;
        [SerializeField] protected SwitcherType m_SwitcherType;
        [SerializeField] protected AudioType m_AudioType;
        [SerializeField] protected bool m_DisableCarCollision;
        [SerializeField] protected bool m_ActivateCarCollision;
    
        [ConditionalHide("m_SwitcherType", (int)SwitcherType.Level)]
        [SerializeField] private Level m_NextLevel;
        [ConditionalHide("m_SwitcherType", (int)SwitcherType.Scene)]
        [SerializeField] private string m_NextSceneName;

        [Header("Respawn Vehicle System")]
        [SerializeField] private Transform m_VehicleSpawnPoint;
        [SerializeField] private bool m_RespawnVehicle;
        [SerializeField] private bool m_NeutralizeVehicleVelocity;
        [SerializeField] private bool m_RefillFuelTank = false;
        [SerializeField] private InputState m_CarControllerState;
        
        [Header("UI Handling - OnInteract")]
        [SerializeField] private UIHandling m_UIHandling;
        [SerializeField] private List<UITypes> m_UIToProcess;
        
        [Header("Audio Clips")]
        [ConditionalHide("m_AudioType", (int)AudioType.UseAudio)]
        [SerializeField] private AudioClip m_OnLevelSwitch_Accepted;
        [ConditionalHide("m_AudioType", (int)AudioType.UseAudio)]
        [SerializeField] private AudioClip m_OnLevelSwitch_Denied;
        
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
        public virtual void OnDriveThrough() => EvaluateSwitcherType();
        
        private void EvaluateSwitcherType()
        {
            switch (m_SwitcherType)
            {
                case SwitcherType.Level:
                    HandleVehicle();
                    HandleUI();
                    HandleFuelRefill();
                    HandleCarCollision();
                    CallLevelSwitch();
                    break;
            
                case SwitcherType.Scene:
                    SceneManager.LoadScene(m_NextSceneName);
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void CallLevelSwitch() => m_NextLevel.SwitchToLevel();
        protected virtual void HandleFuelRefill()
        { if (m_RefillFuelTank) GameManager.Instance.m_Player.GetComponent<PlayerController>().GetCarController().GetCar().RefillFuelTank(); }
        
        protected virtual void HandleCarCollision()
        {
            if (m_DisableCarCollision)
            {
                Car car = GameManager.Instance.m_Player.GetComponent<PlayerController>().GetCarController().GetCar();
                car.gameObject.SetActive(false);
            }
            if (m_ActivateCarCollision)
            {
                Car car = GameManager.Instance.m_Player.GetComponent<PlayerController>().GetCarController().GetCar();
                car.gameObject.SetActive(true);
            }
        }

        private void HandleVehicle()
        {
            Car car = GameManager.Instance.m_Player.GetComponent<PlayerController>().GetCarController().GetCar();
                    
            if (m_RespawnVehicle)
                HandleVehicleRespawning(car);
                    
            if (m_NeutralizeVehicleVelocity)
                HandleVehicleVelocityNeutralization(car);

            HandleVehicleInputState();
        }
        private void HandleVehicleRespawning(Car car)
        {
            if (car is null) return;
            
            car.transform.position = m_VehicleSpawnPoint.position;
            car.transform.rotation = m_VehicleSpawnPoint.rotation;
        }
        private void HandleVehicleVelocityNeutralization(Car car)
        {
            if (car is null) return;
            
            car.GetRigidbody().linearVelocity = Vector3.zero;
            car.GetRigidbody().angularVelocity = Vector3.zero;
        }
        private void HandleVehicleInputState()
        {
            switch (m_CarControllerState)
            {
                case InputState.Disabled:
                    GameManager.Instance.m_Player.GetComponent<PlayerController>().DisableCarInput();
                    break;
                        
                case InputState.Enabled:
                    GameManager.Instance.m_Player.GetComponent<PlayerController>().EnableCarInput();
                    break;
                        
                case InputState.None:
                    break;
                        
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleUI()
        {
            switch (m_UIHandling)
            {
                case UIHandling.None:
                    break;
                
                case UIHandling.Disable:
                    foreach (var ui in m_UIToProcess)
                        EvaluateUIType(ui, false);
                    break;
                
                case UIHandling.Enable:
                    foreach (var ui in m_UIToProcess)
                        EvaluateUIType(ui, true);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void EvaluateUIType(UITypes uiType, bool active)
        {
            switch (uiType)
            {
                case UITypes.None:
                    break;
                
                case UITypes.OpenWorldUI:
                    if (active) UIManager.Instance.EnableOpenWorldUI();
                    else UIManager.Instance.DisableOpenWorldUI();
                    break;
                
                case UITypes.JunkyardUI:
                    if (active) UIManager.Instance.EnableJunkyardUI();
                    else UIManager.Instance.DisableJunkyardUI();
                    break;
                
                case UITypes.GarageUI:
                    if (active) UIManager.Instance.EnableGarageUI();
                    else UIManager.Instance.DisableGarageUI();
                    break;
                
                case UITypes.GarbageUI:
                    if (active) UIManager.Instance.EnableGarbageUI();
                    else UIManager.Instance.DisableGarbageUI();
                    break;
                
                case UITypes.FuelStationUI:
                    if (active) UIManager.Instance.EnableFuelStationUI();
                    else UIManager.Instance.DisableFuelStationUI();
                    break;
                
                case UITypes.RacingUI:
                    if (active) UIManager.Instance.EnableRacingUI();
                    else UIManager.Instance.DisableRacingUI();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected virtual void HandleAudio(NPCType npcType, NPC.ReactionType reactionType)
        {
            if (m_AudioType != AudioType.UseAudio) return;

            switch (reactionType)
            {
                case NPC.ReactionType.Accepted when m_OnLevelSwitch_Accepted is not null:
                    NPCManager.GetInstance().GetNPC(npcType).ReactOnLevelSwitch(m_OnLevelSwitch_Accepted);
                    break;
                
                case NPC.ReactionType.Denied when m_OnLevelSwitch_Denied is not null:
                    NPCManager.GetInstance().GetNPC(npcType).ReactOnLevelSwitch(m_OnLevelSwitch_Denied);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(reactionType), reactionType, null);
            }
        }
        
        private void DisableInteractability() =>  m_interactable = false;
        private void EnableInteractability() =>  m_interactable = true;
    }
}
