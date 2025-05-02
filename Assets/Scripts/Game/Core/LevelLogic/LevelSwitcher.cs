using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game.Core.LevelLogic
{
    public enum SwitcherType
    {
        Level,
        Scene
    }
    
    public enum TriggerType
    {
        Mouse,
        Vehicle
    }
    
    public class LevelSwitcher : MonoBehaviour, IInteractable
    {
        private bool m_interactable = true;
    
        [SerializeField] private SwitcherType m_SwitcherType;
        [SerializeField] private TriggerType m_TriggerType;
    
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
        
        public virtual void OnInteract()
        {
            if (!m_interactable) return;

            switch (m_TriggerType)
            {
                case TriggerType.Mouse:
                    switch (m_SwitcherType)
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
                    break;
                
                case TriggerType.Vehicle:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void DisableInteractability() =>  m_interactable = false;
        private void EnableInteractability() =>  m_interactable = true;
    }
}
