using UnityEngine;

public class DevSpawner : MonoBehaviour
{
#if UNITY_STANDALONE
    private void Awake() => Destroy(gameObject);
#endif
}
