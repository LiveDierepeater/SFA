using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Image m_fader;
    [SerializeField]
    private float m_fadeTime;

    private IEnumerator fadeCoroutine;

    private void Awake() => fadeCoroutine = FadeOut();
    
    public void StartGame() => TransferToOpenWorld();
    public void QuitGame() => Application.Quit();

    private void TransferToOpenWorld() => StartCoroutine(fadeCoroutine);

    private IEnumerator FadeOut()
    {
        TickManager.Instance.TickSystem.OnTickBegin += FadeToBlack;
        yield return new WaitForSeconds(m_fadeTime);
        TickManager.Instance.TickSystem.OnTickBegin -= FadeToBlack;
        LoadLevel();
    }
    private void LoadLevel() => SceneManager.LoadScene("OpenWorld");

    private void FadeToBlack() => m_fader.color = new Color(0, 0, 0, m_fader.color.a + (m_fadeTime - 0.1f) / TickManager.Instance.TickSystem.GetTickDeltaTime() * Time.deltaTime);
}
