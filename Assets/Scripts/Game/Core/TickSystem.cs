using UnityEngine;

[DefaultExecutionOrder(-1)]
public class TickSystem : MonoBehaviour
{
    public delegate void TickBeginDelegate();
    public TickBeginDelegate OnTickBegin;
    
    public delegate void TickDelegate();
    public TickDelegate OnTick;
    
    public delegate void TickEndDelegate();
    public TickEndDelegate OnTickEnd;

    [SerializeField]
    private float m_tickRate = 0.2f;
    
    // ReSharper disable once MemberCanBePrivate.Global
    public int ElapsedTicks { get; private set; }
    private float nextTick;

    private void Awake() => InitializeTickSystem();

    private void InitializeTickSystem()
    {
        if (TickManager.Instance.TickSystem != null)
        {
            Debug.LogWarning("TickSystem already initialized.");
            Destroy(gameObject);
        }
        else
        {
            TickManager.Instance.TickSystem = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update() => TickTimer();

    private void TickTimer()
    {
        if (Time.time >= nextTick)
        {
            OnTickEnd?.Invoke();
            OnTickBegin?.Invoke();
            OnTick?.Invoke();
            
            nextTick = Time.time + m_tickRate;
            ElapsedTicks++;
        }
    }

    public float GetTickDeltaTime() => m_tickRate;
}
