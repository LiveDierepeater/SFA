using System.Collections.Generic;
using Game.Core;
using UnityEngine;

public class BrokenBridge : Obstacle
{
    [System.Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformData(Transform t)
        {
            position = t.position;
            rotation = t.rotation;
            scale = t.localScale;
        }

        public void ApplyTo(Transform t)
        {
            t.position = position;
            t.rotation = rotation;
            t.localScale = scale;
        }
    }
    
    [Header("Components")]
    [SerializeField] private List<Rigidbody> m_BridgeParts = new();
    
    private readonly Dictionary<Transform, TransformData> m_BridgePartsTransforms = new();

    private void Start()
    { foreach (var bridgePart in m_BridgeParts) m_BridgePartsTransforms[bridgePart.transform] = new TransformData(bridgePart.transform); }

    protected override void ResetObstacle()
    {
        base.ResetObstacle();
        RespawnBridge();
    }

    private void RespawnBridge()
    {
        foreach (var pair in m_BridgePartsTransforms)
            pair.Value.ApplyTo(pair.Key);
        
        foreach (var bridgePart in m_BridgeParts)
            bridgePart.isKinematic = true;
    }
}
