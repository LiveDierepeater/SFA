using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core.LevelLogic
{
    public enum SwitcherType
    {
        Level,
        Scene
    }
    
    public class LevelSwitcher : MonoBehaviour, IInteractable
    {
        private bool m_interactable = true;
    
        [SerializeField] private SwitcherType m_switcherType;
    
        [ConditionalHide("m_switcherType", (int)SwitcherType.Level)]
        [SerializeField] private Level m_nextLevel;
        [ConditionalHide("m_switcherType", (int)SwitcherType.Scene)]
        [SerializeField] private string m_nextSceneName;

        private void Start() => InitializeLevelSwitchingLogic();
        private void InitializeLevelSwitchingLogic()
        {
            m_nextLevel.OnLevelSwitchingAccepted += DisableInteractability;
            m_nextLevel.OnLevelSwitchingFinished += EnableInteractability;
        }
        
        public void OnInteract()
        {
            if (!m_interactable) return;
        
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
        
        private void DisableInteractability() =>  m_interactable = false;
        private void EnableInteractability() =>  m_interactable = true;
    }
}
