using System;
using Game.Core;
using Game.Core.LevelLogic;
using UnityEngine;

public class CarProxy : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform m_Slot_Bonnet;
    [SerializeField] private Transform m_Slot_Chassis;
    [SerializeField] private Transform m_Slot_Engine;
    [SerializeField] private Transform m_Slot_FuelTank;
    [SerializeField] private Transform m_Slot_Trunk;
    [SerializeField] private Transform m_Slot_Wheel_LF;
    [SerializeField] private Transform m_Slot_Wheel_RF;
    [SerializeField] private Transform m_Slot_Wheel_LB;
    [SerializeField] private Transform m_Slot_Wheel_RB;
    
    [SerializeField] private GameObject m_CarComponentProxyPrefab;
    [SerializeField] private Transform m_SpawnPointTransform;
    
    [SerializeField] private Garage m_Garage;

    public void UpdateCarProxy()
    {
        DestroyAllProxies();
        
        var tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Bonnet.position, m_Slot_Bonnet.rotation, m_Slot_Bonnet).GetComponent<CarComponentProxy>();
        tmp.InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetBonnet(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Chassis.position, m_Slot_Chassis.rotation, m_Slot_Chassis).GetComponent<CarComponentProxy>();
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetChassis(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Engine.position, m_Slot_Engine.rotation, m_Slot_Engine).GetComponent<CarComponentProxy>();
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetEngine(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_FuelTank.position, m_Slot_FuelTank.rotation, m_Slot_FuelTank).GetComponent<CarComponentProxy>();
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetFuelTank(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Trunk.position, m_Slot_Trunk.rotation, m_Slot_Trunk).GetComponent<CarComponentProxy>();
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTrunk(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Wheel_LF.position, m_Slot_Wheel_LF.rotation, m_Slot_Wheel_LF).GetComponent<CarComponentProxy>();
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Wheel_RF.position, m_Slot_Wheel_RF.rotation, m_Slot_Wheel_RF).GetComponent<CarComponentProxy>();
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Wheel_LB.position, m_Slot_Wheel_LB.rotation, m_Slot_Wheel_LB).GetComponent<CarComponentProxy>();
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Wheel_RB.position, m_Slot_Wheel_RB.rotation, m_Slot_Wheel_RB).GetComponent<CarComponentProxy>();
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
            this, false, true, ProxyState.Installed, m_SpawnPointTransform.position, m_Garage);
    }
    
    public void RemoveProxy(CarComponentProxy carComponentProxy)
    {
        if (carComponentProxy == null) return;
        
        switch (carComponentProxy.GetCarComponent())
        {
            case Bonnet:
                if (m_Slot_Bonnet.childCount > 0 && m_Slot_Bonnet.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_Bonnet.GetChild(0));
                    m_Slot_Bonnet.DetachChildren();
                }
                break;
            case Chassis:
                if (m_Slot_Chassis.childCount > 0 && m_Slot_Chassis.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_Chassis.GetChild(0));
                    m_Slot_Chassis.DetachChildren();
                }
                break;
            case Engine:
                if (m_Slot_Engine.childCount > 0 && m_Slot_Engine.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_Engine.GetChild(0));
                    m_Slot_Engine.DetachChildren();
                }
                break;
            case FuelTank:
                if (m_Slot_FuelTank.childCount > 0 && m_Slot_FuelTank.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_FuelTank.GetChild(0));
                    m_Slot_FuelTank.DetachChildren();
                }
                break;
            case Trunk:
                if (m_Slot_Trunk.childCount > 0 && m_Slot_Trunk.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_Trunk.GetChild(0));
                    m_Slot_Trunk.DetachChildren();
                }
                break;
            case Tires:
                if (m_Slot_Wheel_LF.childCount > 0 && m_Slot_Wheel_LF.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_Wheel_LF.GetChild(0));
                    m_Slot_Wheel_LF.DetachChildren();
                    
                    Destroy(m_Slot_Wheel_RF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_LB.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RB.GetChild(0).gameObject);
                }
                else if (m_Slot_Wheel_RF.childCount > 0 && m_Slot_Wheel_RF.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_Wheel_RF.GetChild(0));
                    m_Slot_Wheel_RF.DetachChildren();
                    
                    Destroy(m_Slot_Wheel_LF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_LB.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RB.GetChild(0).gameObject);
                }
                else if (m_Slot_Wheel_LB.childCount > 0 && m_Slot_Wheel_LB.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_Wheel_LB.GetChild(0));
                    m_Slot_Wheel_LB.DetachChildren();
                    
                    Destroy(m_Slot_Wheel_LF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RB.GetChild(0).gameObject);
                }
                else if (m_Slot_Wheel_RB.childCount > 0 && m_Slot_Wheel_RB.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Garage.AddCarComponentToDeleteLater(m_Slot_Wheel_RB.GetChild(0));
                    m_Slot_Wheel_RB.DetachChildren();
                    
                    Destroy(m_Slot_Wheel_LF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_LB.GetChild(0).gameObject);
                }
                break;
        }
    }
    public void InstallProxy(CarComponentProxy carComponentProxy)
    {
        switch (carComponentProxy.GetCarComponent())
        {
            case Bonnet:
                carComponentProxy.transform.SetParent(m_Slot_Bonnet.transform);
                break;
            case Chassis:
                carComponentProxy.transform.SetParent(m_Slot_Chassis.transform);
                break;
            case Engine:
                carComponentProxy.transform.SetParent(m_Slot_Engine.transform);
                break;
            case FuelTank:
                carComponentProxy.transform.SetParent(m_Slot_FuelTank.transform);
                break;
            case Tires:
                carComponentProxy.transform.SetParent(m_Slot_Wheel_LF.transform);
                
                var tmp = Instantiate(carComponentProxy, m_Slot_Wheel_RF.transform, false);
                tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(
                    GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
                    this, false, true, ProxyState.Installed, tmp.transform.position);
                tmp = Instantiate(carComponentProxy, m_Slot_Wheel_LB.transform, false);
                tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(
                    GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
                    this, false, true, ProxyState.Installed, tmp.transform.position);
                tmp = Instantiate(carComponentProxy, m_Slot_Wheel_RB.transform, false);
                tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(
                    GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
                    this, false, true, ProxyState.Installed, tmp.transform.position);
                break;
            case Trunk:
                carComponentProxy.transform.SetParent(m_Slot_Trunk.transform);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DestroyAllProxies()
    {
        if (m_Slot_Bonnet.childCount > 0)
            Destroy(m_Slot_Bonnet.GetChild(0).gameObject);
        
        if (m_Slot_Chassis.childCount > 0)
            Destroy(m_Slot_Chassis.GetChild(0).gameObject);
        
        if (m_Slot_Engine.childCount > 0)
            Destroy(m_Slot_Engine.GetChild(0).gameObject);
        
        if (m_Slot_FuelTank.childCount > 0)
            Destroy(m_Slot_FuelTank.GetChild(0).gameObject);
        
        if (m_Slot_Trunk.childCount > 0)
            Destroy(m_Slot_Trunk.GetChild(0).gameObject);
        
        if (m_Slot_Wheel_LF.childCount > 0)
            Destroy(m_Slot_Wheel_LF.GetChild(0).gameObject);
        
        if (m_Slot_Wheel_RF.childCount > 0)
            Destroy(m_Slot_Wheel_RF.GetChild(0).gameObject);
        
        if (m_Slot_Wheel_LB.childCount > 0)
            Destroy(m_Slot_Wheel_LB.GetChild(0).gameObject);
        
        if (m_Slot_Wheel_RB.childCount > 0)
            Destroy(m_Slot_Wheel_RB.GetChild(0).gameObject);
    }
    
    public bool HasBonnet() => m_Slot_Bonnet.childCount > 0;
    public bool HasChassis() => m_Slot_Chassis.childCount > 0;
    public bool HasEngine() => m_Slot_Engine.childCount > 0;
    public bool HasFuelTank() => m_Slot_FuelTank.childCount > 0;
    public bool HasTrunk() => m_Slot_Trunk.childCount > 0;
    public bool HasWheel_LF() => m_Slot_Wheel_LF.childCount > 0;
    
    public Transform GetSlot_Bonnet() => m_Slot_Bonnet;
    public Transform GetSlot_Chassis() => m_Slot_Chassis;
    public Transform GetSlot_Engine() => m_Slot_Engine;
    public Transform GetSlot_FuelTank() => m_Slot_FuelTank;
    public Transform GetSlot_Trunk() => m_Slot_Trunk;
    public Transform GetSlot_Wheel_LF() => m_Slot_Wheel_LF;
    public Transform GetSlot_Wheel_RF() => m_Slot_Wheel_RF;
    public Transform GetSlot_Wheel_LB() => m_Slot_Wheel_LB;
    public Transform GetSlot_Wheel_RB() => m_Slot_Wheel_RB;
    public void OnInteract() { }
}
