using UnityEngine;

public class DevSpawner : MonoBehaviour
{
#if UNITY_EDITOR
#else
    private void Awake() => Destroy(gameObject);
#endif
}
