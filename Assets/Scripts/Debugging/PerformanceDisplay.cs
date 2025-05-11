using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class PerformanceDisplay : MonoBehaviour
{
    private IAdaptivePerformance ap;

    [SerializeField] private TMP_Text displayText;
    
    private float deltaTime;

    void Start()
    {
        ap = Holder.Instance;
        if (ap == null)
            Debug.LogWarning("Adaptive Performance not available.");
    }

    void Update()
    {
        if (ap == null || !ap.Active)
        {
            //print("Adaptive Performance not available.");
            return;
        }

        var thermal = ap.ThermalStatus;
        var perf = ap.PerformanceStatus;
        
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        displayText.text =
            $"FPS: {fps:F1}\n" +
            $"Average FPS: {1.0f / perf.FrameTiming.AverageFrameTime:F1}\n" +
            $"CPU Time: {perf.FrameTiming.CurrentCpuFrameTime * 1000f:F2} ms\n" +
            $"GPU Time: {perf.FrameTiming.CurrentGpuFrameTime * 1000f:F2} ms\n" +
            $"Bottleneck: {perf.PerformanceMetrics.PerformanceBottleneck}\n" +
            $"CPU Level: {perf.PerformanceMetrics.CurrentCpuLevel}\n" +
            $"GPU Level: {perf.PerformanceMetrics.CurrentGpuLevel}\n" +
            $"Thermal Trend: {thermal.ThermalMetrics.TemperatureTrend:F2}\n" +
            $"Warning Level: {thermal.ThermalMetrics.WarningLevel}";
    }
}
