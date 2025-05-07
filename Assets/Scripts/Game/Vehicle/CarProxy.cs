using Game.Core;
using UnityEngine;

public class CarProxy : MonoBehaviour
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
    
    //private void Start() => UpdateCarProxy();

    public void UpdateCarProxy()
    {
        DestroyAllProxies();
        
        var tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Bonnet.position, Quaternion.identity, m_Slot_Bonnet);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetBonnet(),
            this, false, true);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Chassis.position, Quaternion.identity, m_Slot_Chassis);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetChassis(),
            this, false, true);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Engine.position, Quaternion.identity, m_Slot_Engine);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetEngine(),
            this, false, true);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_FuelTank.position, Quaternion.identity, m_Slot_FuelTank);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetFuelTank(),
            this, false, true);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Trunk.position, Quaternion.identity, m_Slot_Trunk);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTrunk(),
            this, false, true);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Wheel_LF.position, Quaternion.identity, m_Slot_Wheel_LF);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
            this, false, true);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Wheel_RF.position, Quaternion.identity, m_Slot_Wheel_RF);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
            this, false, true);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Wheel_LB.position, Quaternion.identity, m_Slot_Wheel_LB);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
            this, false, true);
        
        tmp = Instantiate(m_CarComponentProxyPrefab, m_Slot_Wheel_RB.position, Quaternion.identity, m_Slot_Wheel_RB);
        tmp.GetComponent<CarComponentProxy>().InitializeCarComponentProxy(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires(),
            this, false, true);
    }
    
    public void RemoveProxy(CarComponentProxy carComponentProxy)
    {
        if (carComponentProxy == null) return;
        
        switch (carComponentProxy.GetCarComponent())
        {
            case Bonnet:
                if (m_Slot_Bonnet.childCount > 0 && m_Slot_Bonnet.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_Bonnet.DetachChildren();
                }
                break;
            case Chassis:
                if (m_Slot_Chassis.childCount > 0 && m_Slot_Chassis.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_Chassis.DetachChildren();
                }
                break;
            case Engine:
                if (m_Slot_Engine.childCount > 0 && m_Slot_Engine.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_Engine.DetachChildren();
                }
                break;
            case FuelTank:
                if (m_Slot_FuelTank.childCount > 0 && m_Slot_FuelTank.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_FuelTank.DetachChildren();
                }
                break;
            case Trunk:
                if (m_Slot_Trunk.childCount > 0 && m_Slot_Trunk.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_Trunk.DetachChildren();
                }
                break;
            case Tires:
                if (m_Slot_Wheel_LF.childCount > 0 && m_Slot_Wheel_LF.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_Wheel_LF.DetachChildren();
                    
                    Destroy(m_Slot_Wheel_RF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_LB.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RB.GetChild(0).gameObject);
                }
                else if (m_Slot_Wheel_RF.childCount > 0 && m_Slot_Wheel_RF.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_Wheel_RF.DetachChildren();
                    
                    Destroy(m_Slot_Wheel_LF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_LB.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RB.GetChild(0).gameObject);
                }
                else if (m_Slot_Wheel_LB.childCount > 0 && m_Slot_Wheel_LB.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_Wheel_LB.DetachChildren();
                    
                    Destroy(m_Slot_Wheel_LF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RB.GetChild(0).gameObject);
                }
                else if (m_Slot_Wheel_RB.childCount > 0 && m_Slot_Wheel_RB.GetChild(0).GetComponent<CarComponentProxy>() == carComponentProxy)
                {
                    m_Slot_Wheel_RB.DetachChildren();
                    
                    Destroy(m_Slot_Wheel_LF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_RF.GetChild(0).gameObject);
                    Destroy(m_Slot_Wheel_LB.GetChild(0).gameObject);
                }
                break;
        }
    }

    private void DestroyAllProxies()
    {
        if (m_Slot_Bonnet.childCount > 0)
            Destroy(m_Slot_Bonnet.GetChild(0));
        
        if (m_Slot_Chassis.childCount > 0)
            Destroy(m_Slot_Chassis.GetChild(0));
        
        if (m_Slot_Engine.childCount > 0)
            Destroy(m_Slot_Engine.GetChild(0));
        
        if (m_Slot_FuelTank.childCount > 0)
            Destroy(m_Slot_FuelTank.GetChild(0));
        
        if (m_Slot_Trunk.childCount > 0)
            Destroy(m_Slot_Trunk.GetChild(0));
        
        if (m_Slot_Wheel_LF.childCount > 0)
            Destroy(m_Slot_Wheel_LF.GetChild(0));
        
        if (m_Slot_Wheel_RF.childCount > 0)
            Destroy(m_Slot_Wheel_RF.GetChild(0));
        
        if (m_Slot_Wheel_LB.childCount > 0)
            Destroy(m_Slot_Wheel_LB.GetChild(0));
        
        if (m_Slot_Wheel_RB.childCount > 0)
            Destroy(m_Slot_Wheel_RB.GetChild(0));
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
}
