using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool m_Interactable = true;

    private void OnTriggerEnter(Collider other) { if (m_Interactable && other.CompareTag("Player")) OpenDoor(); }
    private void OnTriggerExit(Collider other) { if (m_Interactable && other.CompareTag("Player")) CloseDoor(); }
    
    [Header("Components")]
    [SerializeField] private Transform m_Door_R;
    [SerializeField] private Transform m_Door_L;
    [Header("Settings")]
    [SerializeField] private Vector3 m_OpenRotation_R = new Vector3(0, -90, 0);
    [SerializeField] private Vector3 m_OpenRotation_L = new Vector3(0, 90, 0);
    [SerializeField] private float m_Duration = 1f;
    [SerializeField] private float m_DoorOffset = 0.08f;

    private void OpenDoor()
    {
        StopAllCoroutines();
        StartCoroutine(RotateDoor(Quaternion.Euler(m_OpenRotation_R), Quaternion.Euler(m_OpenRotation_L)));
    }
    private void CloseDoor()
    { 
        StopAllCoroutines();
        StartCoroutine(RotateDoor(Quaternion.identity, Quaternion.identity));
    }

    private IEnumerator RotateDoor(Quaternion targetRotation_R, Quaternion targetRotation_L)
    {
        Quaternion startRotation_R = m_Door_R.rotation;
        Quaternion startRotation_L = m_Door_L.rotation;
        float time = 0f;

        while (time < m_Duration)
        {
            time += Time.deltaTime;
            float t = time / m_Duration;
            m_Door_R.rotation = Quaternion.Slerp(startRotation_R, targetRotation_R, t);

            // Small Offset for Left door
            t = Mathf.Clamp(t - m_DoorOffset, 0, m_Duration);
            m_Door_L.rotation = Quaternion.Slerp(startRotation_L, targetRotation_L, t);
            yield return null;
        }

        m_Door_R.rotation = targetRotation_R; // Genauigkeit am Ende
        m_Door_L.rotation = targetRotation_L; // Genauigkeit am Ende
    }
}
