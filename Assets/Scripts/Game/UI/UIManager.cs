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
    
    [Header("Components")]
    [SerializeField] private Animator m_Fader;
    [SerializeField] private Transform m_OpenWorldUI;
    
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

    public void DisableOpenWorldUI() => DisableUI(m_OpenWorldUI);
    public void EnableOpenWorldUI() => EnableUI(m_OpenWorldUI);
    public void DisableJunkyardUI() => DisableUI(m_OpenWorldUI);
    public void EnableJunkyardUI() => EnableUI(m_OpenWorldUI);
    public void DisableGarageUI() => DisableUI(m_OpenWorldUI);
    public void EnableGarageUI() => EnableUI(m_OpenWorldUI);
    public void DisableGarbageUI() => DisableUI(m_OpenWorldUI);
    public void EnableGarbageUI() => EnableUI(m_OpenWorldUI);
    public void DisableFuelStationUI() => DisableUI(m_OpenWorldUI);
    public void EnableFuelStationUI() => EnableUI(m_OpenWorldUI);
    public void DisableRacingUI() => DisableUI(m_OpenWorldUI);
    public void EnableRacingUI() => EnableUI(m_OpenWorldUI);
    
    private static void DisableUI(Transform ui)
    {
        if (ui == null) return;
        ui.gameObject.SetActive(false);
    }
    private static void EnableUI(Transform ui)
    {
        if (ui == null) return;
        ui.gameObject.SetActive(true);
    }
    
#region Fader

    public void FadeToBlack()
    {
        m_Fader.SetInteger(FadeInput, 1);
        m_Fader.GetComponent<Image>().raycastTarget = true;
    }
    public void FadeOut()
    {
        m_Fader.SetInteger(FadeInput, -1);
        StartCoroutine(nameof(DisableFaderRaycastTargetAfterTime));
    }

    public void FadeInAndOut() => StartCoroutine(nameof(FadeToNextLevel_Coroutine));
    
    private IEnumerator FadeToNextLevel_Coroutine()
    {
        m_Fader.SetInteger(FadeInput, 1);
        yield return new WaitForSeconds(1f);
        m_Fader.SetInteger(FadeInput, -1);
    }
    private IEnumerator DisableFaderRaycastTargetAfterTime()
    {
        yield return new WaitForSeconds(m_FadeTime);
        m_Fader.GetComponent<Image>().raycastTarget = false;
    }

#endregion
}
