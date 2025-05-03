using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType
{
    FadeIn,
    FadeOut
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private Image m_Fader;
    [SerializeField] private float m_FadeTime;

    private IEnumerator m_FadeToBlack_Coroutine;
    private IEnumerator m_FadeOut_Coroutine;
    private IEnumerator m_LevelTransition_Coroutine;

    private void Awake() => Initialize();
    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            m_FadeToBlack_Coroutine = Fade_Coroutine(FadeType.FadeIn);
            m_FadeOut_Coroutine = Fade_Coroutine(FadeType.FadeOut);
            m_LevelTransition_Coroutine = LevelTransition_Coroutine();
        }
        else
        {
            Debug.LogWarning("TickSystem already initialized.");
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (m_Fader.color.a != 0)
            FadeOut_Coroutine();
    }

    public void FadeToBlack_Coroutine() => StartCoroutine(m_FadeToBlack_Coroutine);
    public void FadeOut_Coroutine() => StartCoroutine(m_FadeOut_Coroutine);
    public void FadeToNextLevel_Coroutine()
    {
        print("FadeToNextLevel_Coroutine");
        StartCoroutine(m_LevelTransition_Coroutine);
    }

    private IEnumerator Fade_Coroutine(FadeType fadeType)
    {
        switch (fadeType)
        {
            case FadeType.FadeIn:
                TickManager.Instance.TickSystem.OnTickBegin += FadeToBlack;
                yield return new WaitForSeconds(m_FadeTime);
                TickManager.Instance.TickSystem.OnTickBegin -= FadeToBlack;
                m_Fader.color = Color.black;
                break;
            
            case FadeType.FadeOut:
                TickManager.Instance.TickSystem.OnTickBegin += FadeOut;
                yield return new WaitForSeconds(m_FadeTime);
                TickManager.Instance.TickSystem.OnTickBegin -= FadeOut;
                m_Fader.color = Color.clear;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(fadeType), fadeType, null);
        }
    }
    private IEnumerator LevelTransition_Coroutine()
    {
        print("drinnen");
        FadeToBlack_Coroutine();
        yield return new WaitForSeconds(m_FadeTime + 0.1f);
        FadeOut_Coroutine();
        print("draußen");
    }
    
    private void FadeToBlack()
    {
        m_Fader.color = new Color(0, 0, 0,
            m_Fader.color.a + m_FadeTime / TickManager.Instance.TickSystem.GetTickDeltaTime() * Time.deltaTime);
        print("FadeToBlack: " + m_Fader.color.a);
    }

    private void FadeOut()
    {
        m_Fader.color = new Color(0, 0, 0,
            m_Fader.color.a - m_FadeTime / TickManager.Instance.TickSystem.GetTickDeltaTime() * Time.deltaTime);
        print("FadeOut: " + m_Fader.color.a);
    }
}
