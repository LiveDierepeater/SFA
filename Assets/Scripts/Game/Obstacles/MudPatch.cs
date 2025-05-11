using Game.Core;
using Game.Vehicle;
using UnityEngine;

public class MudPatch : Obstacle
{
    [Header("Components")]
    [SerializeField] private Collider m_Collider;
    [Header("Settings")]
    [SerializeField] private float m_BrakeForce = 25f;
    [SerializeField] private float m_MinTractionNecessary = 3f;
    [SerializeField] private float m_MaxMassPointsPossibleToMaster = 15f;
    
    private Car m_Car;

    protected override bool IsAbleToMaster() =>
        !(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetTires().m_Traction < m_MinTractionNecessary)
        &&
        !(GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetCarStats().GetAllMassPoints() > m_MaxMassPointsPossibleToMaster);

    protected override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if (!other.CompareTag("Player")) return;
        
        m_Car = GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar();

        if (!IsAbleToMaster()) return;

        m_Collider.enabled = false;
    }
    
    protected override void TriggerStay(Collider collider)
    {
        base.TriggerStay(collider);
        if (!collider.CompareTag("Player")) return;
        
        //if (IsAbleToMaster())
        //    foreach (var wheel in GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetWheels().Where(wheel => wheel.IsPoweredWheel()))
        //        wheel.ApplyBrakeTorque(m_BrakeForce);
        //else
        //    foreach (var wheel in GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().GetWheels().Where(wheel => wheel.IsPoweredWheel()))
        //        wheel.ApplyBrakeTorque(m_BrakeForce * 5f);
        
        if (m_Car is null) return;

        if (IsAbleToMaster())
            m_Car.GetRigidbody().AddForce(
                -m_Car.GetRigidbody().linearVelocity.normalized * m_BrakeForce * (m_Car.GetRigidbody().linearVelocity.magnitude / m_Car.GetCarStats().GetMaxSpeed()),
                ForceMode.Acceleration);
        else
            m_Car.GetRigidbody().AddForce(
                -m_Car.GetRigidbody().linearVelocity.normalized * m_BrakeForce,
                ForceMode.Acceleration);
    }

    protected override void TriggerExit(Collider other)
    {
        base.TriggerExit(other);
        if (!other.CompareTag("Player")) return;
        
        m_Collider.enabled = true;
    }
}
