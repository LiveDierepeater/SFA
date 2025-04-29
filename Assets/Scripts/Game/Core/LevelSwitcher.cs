using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour, IInteractable
{
    private bool m_interacted = false;
    
    [SerializeField] private SwitcherType m_switcherType;
    
    [ConditionalHide("m_switcherType", (int)SwitcherType.Level)]
    [SerializeField] private Level m_nextLevel;
    [ConditionalHide("m_switcherType", (int)SwitcherType.Scene)]
    [SerializeField] private string m_nextSceneName;
    
    public void OnInteract()
    {
        if (m_interacted) return;
        
        m_interacted = true;
        
        // TODO: Switch to the next level
        switch (m_switcherType)
        {
            case SwitcherType.Level:
                m_nextLevel.SwitchToLevel();
                break;
            
            case SwitcherType.Scene:
                SceneManager.LoadScene(m_nextSceneName);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum SwitcherType
{
    Level,
    Scene
}
