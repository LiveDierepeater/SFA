using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_WheelTransform;
    
    [Header("Stats")]
    [SerializeField] private bool m_Steer;
    [SerializeField] private bool m_Power;
    
    private WheelCollider m_WheelCollider;

    private void Awake() => m_WheelCollider = GetComponent<WheelCollider>();

    public void ApplyTorque(float torque) => m_WheelCollider.motorTorque = torque;
    public void ApplySteerAngle(float angle) => m_WheelCollider.steerAngle = angle;

    private void FixedUpdate() => UpdateMeshTransform();

    private void UpdateMeshTransform()
    {
        m_WheelCollider.GetWorldPose(out var pos, out var rot);
        m_WheelTransform.position = pos;
        m_WheelTransform.rotation = rot;
    }

    public bool IsSteeringWheel() => m_Steer;
    public bool IsPoweredWheel() => m_Power;
}
