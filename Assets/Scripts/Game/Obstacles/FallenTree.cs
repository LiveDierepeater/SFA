using System;
using Game.Core;
using UnityEngine;

public sealed class FallenTree : Obstacle
{
    [Header("Components")]
    [SerializeField] private Collider m_Collider;
    [Header("Settings")]
    [SerializeField] private float m_TimeToMasterObstacle = 0.5f;
    [SerializeField] private float m_TorqueRequired = 2f;
    [SerializeField] private float m_MaxMassPointsAllowed = 12f;
    
    private float m_CurrentTimeToMasterObstacle;
    private bool m_IsPlayerInside;

    private void Awake() => Initialize();

    private void Initialize() => m_CurrentTimeToMasterObstacle = m_TimeToMasterObstacle;

    protected override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        m_IsPlayerInside = true;
    }

    protected override void TriggerExit(Collider other)
    {
        base.TriggerExit(other);
        m_IsPlayerInside = false;
    }

    private void FixedUpdate()
    {
        if (m_IsPlayerInside)
        {
            m_CurrentTimeToMasterObstacle = Mathf.Clamp(m_CurrentTimeToMasterObstacle - Time.fixedDeltaTime, 0, m_TimeToMasterObstacle);
            
            if (m_CurrentTimeToMasterObstacle == 0)
                if (IsAbleToMaster())
                    UnblockObstacle();
        }
        else
        {
            m_CurrentTimeToMasterObstacle = Mathf.Clamp(m_CurrentTimeToMasterObstacle + Time.fixedDeltaTime, 0, m_TimeToMasterObstacle);
            
            if (Math.Abs(m_CurrentTimeToMasterObstacle - m_TimeToMasterObstacle) < 0.05f)
                BlockObstacle();
        }
    }
    
    protected override bool IsAbleToMaster()
    {
        base.IsAbleToMaster();
        
        if (GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetEngine().m_Torque >= m_TorqueRequired)
            if (GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetAllMassPoints() <= m_MaxMassPointsAllowed)
                return true;
        
        return false;
    }    

    private void BlockObstacle() => m_Collider.enabled = true;
    private void UnblockObstacle() => m_Collider.enabled = false;
}
