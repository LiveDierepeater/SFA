using System;
using System.Collections;
using UnityEngine;

public enum FadeType
{
    FadeIn,
    FadeOut
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private Animator m_Fader;
    
    private static readonly int FadeInput = Animator.StringToHash("FadeInput");
    private static readonly int FadeDuration = Animator.StringToHash("FadeDuration");
    private static readonly int Ready = Animator.StringToHash("Ready");

    private float m_FadeTime;

    private void Awake() => Initialize();
    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            m_FadeTime = m_Fader.GetFloat(FadeDuration);
        }
        else
        {
            Debug.LogWarning("TickSystem already initialized.");
            Destroy(gameObject);
        }
    }

    private void Start() => m_Fader.SetBool(Ready, true);

    public void FadeToBlack() => m_Fader.SetInteger(FadeInput, 1);
    public void FadeOut() => m_Fader.SetInteger(FadeInput, -1);
    public void FadeInAndOut() => StartCoroutine(nameof(FadeToNextLevel_Coroutine));
    
    private IEnumerator FadeToNextLevel_Coroutine()
    {
        m_Fader.SetInteger(FadeInput, 1);
        yield return new WaitForSeconds(1f);
        m_Fader.SetInteger(FadeInput, -1);
    }
}
