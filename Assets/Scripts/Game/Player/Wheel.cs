using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    [SerializeField] private Transform m_WheelTransform;
    [SerializeField] private bool m_IsLeftSidedWheel = true;
    
    private WheelCollider m_WheelCollider;

    private void Awake() => m_WheelCollider = GetComponent<WheelCollider>();

    public void ApplyTorque(float torque) => m_WheelCollider.motorTorque = torque;
    public void ApplySteerAngle(float angle) => m_WheelCollider.steerAngle = angle;

    private void FixedUpdate()
    {
        Vector3 pos;
        Quaternion rot;

        if (m_IsLeftSidedWheel)
        {
            m_WheelCollider.GetWorldPose(out pos, out rot);
            m_WheelTransform.position = pos;
            m_WheelTransform.rotation = rot;
        }
        else
        {
            m_WheelCollider.GetWorldPose(out pos, out rot);
            m_WheelTransform.position = pos;
            m_WheelTransform.rotation = rot * Quaternion.Euler(0, 180, 0);
        }
    }
}
