using System;
using Game.Core;
using Game.Core.LevelLogic;
using UnityEngine;

public enum ProxyState
{
    Free,
    Grabbed,
    Snapped,
    Installed
}

public class CarComponentProxy : MonoBehaviour, IInteractable 
{
    public ProxyState m_ProxyState = ProxyState.Free;

    [SerializeField] private LayerMask m_LayerToCast;

    private CarComponent m_CarComponent;
    private CarProxy m_CarProxy;
    private Vector3 m_SpawnPoint;
    
    public void InitializeCarComponentProxy(CarComponent carComponent, CarProxy carProxy = null, bool removeRigidbody = false, bool isKinematic = false, ProxyState proxyState = ProxyState.Free, Vector3 spawnPoint = default(Vector3), Garage garage = null)
    {
        if (!carComponent)
            throw new Exception("CarComponent is null!");
        
        m_CarComponent = carComponent;
        var newProxy = Instantiate(m_CarComponent.m_Mesh, transform);
        newProxy.layer = LayerMask.NameToLayer("Components");
        var _collider = newProxy.AddComponent<MeshCollider>();
        _collider.sharedMesh = carComponent.m_Collision;
        _collider.convex = true;
        
        m_CarProxy = carProxy;
        m_SpawnPoint = spawnPoint == default(Vector3) ? transform.position : spawnPoint;
        
        TryGetComponent(out Rigidbody _rigidbody);
        _rigidbody.isKinematic = isKinematic;
        
        m_ProxyState  = proxyState;
        
        if (garage) garage.AddCarComponentToDeleteLater(transform);
        
        // Remove Rigidbody if it is not needed
        if (!removeRigidbody) return;
        Destroy(_rigidbody);
        Destroy(_collider);
    }

    private void UpdatePosition()
    {
        if (!Physics.Raycast(GameManager.Instance.m_Player.GetCamera().ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 999.0f, m_LayerToCast)) return;
        var targetPosition = new Vector3(hit.point.x, hit.point.y, m_SpawnPoint.z);
        
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("InvisWall"))
        {
            if (!Physics.Raycast(GameManager.Instance.m_Player.GetCamera().ScreenPointToRay(Input.mousePosition), out RaycastHit secondHit, 999.0f, LayerMask.NameToLayer("Car"))) return;
            if (m_CarProxy is not null)
            {
                switch (m_CarComponent)
                {
                    case Bonnet:
                        if (!m_CarProxy.HasBonnet())
                        {
                            targetPosition = m_CarProxy.GetSlot_Bonnet().position;
                            m_ProxyState = ProxyState.Snapped;
                        }
                        break;

                    case Chassis:
                        if (!m_CarProxy.HasChassis())
                        {
                            targetPosition = m_CarProxy.GetSlot_Chassis().position;
                            m_ProxyState = ProxyState.Snapped;
                        }
                        break;

                    case Engine:
                        if (!m_CarProxy.HasEngine())
                        {
                            targetPosition = m_CarProxy.GetSlot_Engine().position;
                            m_ProxyState = ProxyState.Snapped;
                        }
                        break;

                    case FuelTank:
                        if (!m_CarProxy.HasFuelTank())
                        {
                            targetPosition = m_CarProxy.GetSlot_FuelTank().position;
                            m_ProxyState = ProxyState.Snapped;
                        }
                        break;

                    case Trunk:
                        if (!m_CarProxy.HasTrunk())
                        {
                            targetPosition = m_CarProxy.GetSlot_Trunk().position;
                            m_ProxyState = ProxyState.Snapped;
                        }
                        break;

                    case Tires:
                        if (!m_CarProxy.HasWheel_LF())
                        {
                            targetPosition = m_CarProxy.GetSlot_Wheel_LF().position;
                            m_ProxyState = ProxyState.Snapped;
                        }
                        break;
                }
            }
        }
        
        transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }

    private void SettleState()
    {
        GameManager.Instance.m_Player.GetPlayerController().OnPrimaryActionHold -= UpdatePosition;
        GameManager.Instance.m_Player.GetPlayerController().OnPrimaryActionReleased -= SettleState;

        if (m_ProxyState == ProxyState.Snapped)
        {
            m_ProxyState = ProxyState.Installed;
            m_CarProxy.InstallProxy(this);
            if (TryGetComponent(out Rigidbody _rigidbody)) _rigidbody.isKinematic = true;
        }
        else
        {
            m_ProxyState = ProxyState.Free;
            m_CarProxy.RemoveProxy(this);
            if (TryGetComponent(out Rigidbody _rigidbody)) _rigidbody.isKinematic = false;
        }
    }
    
    public void OnInteract()
    {
        m_ProxyState = ProxyState.Grabbed;
        if (TryGetComponent(out Rigidbody _rigidbody)) _rigidbody.isKinematic = true;
        GameManager.Instance.m_Player.GetPlayerController().OnPrimaryActionHold += UpdatePosition;
        GameManager.Instance.m_Player.GetPlayerController().OnPrimaryActionReleased += SettleState;
    }
    
    public CarComponent GetCarComponent() => m_CarComponent;
}
