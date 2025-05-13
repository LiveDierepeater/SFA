using System.Collections;
using System.Collections.Generic;
using Game.Core.Interfaces;
using UnityEngine;

namespace Game.Core.LevelLogic
{
    public abstract class Level : MonoBehaviour, ILevel
    {
        [SerializeField] protected Transform m_targetTransform;
        [SerializeField] protected Transform m_resetTransform;
        
        [SerializeField] private float LoadingDuration;
        
        [Header("Audio")]
        [SerializeField] private AudioClip m_OnLevelLoadFirstTime;
        [SerializeField] private List<AudioClip> m_OnLevelLoad;

        public delegate void LevelSwitching();
        public LevelSwitching OnLevelSwitchingAccepted;
        public LevelSwitching OnLevelSwitchingFinished;
        
        private int m_InteractionsCount;

        public virtual void SwitchToLevel()
        {
            BeforeLevelLoad();
            StartCoroutine(nameof(LoadLevel), m_targetTransform);
        }

        public virtual void ResetToLevel() => PlayerTransition(m_resetTransform);

        protected virtual void BeforeLevelLoad()
        {
            OnLevelSwitchingAccepted?.Invoke();
            CameraFade(true);
        }

        public virtual IEnumerator LoadLevel(Transform targetPlayerTransform)
        {
            yield return new WaitForSeconds(LoadingDuration);
            HandleNPCVoiceLines();
            PlayerTransition(targetPlayerTransform);
            CameraFade(false);
            OnLevelSwitchingFinished?.Invoke();
        }

        public virtual void PlayerTransition(Transform targetPlayerTransform)
        {
            GameManager.Instance.m_Player.LevelTransition(targetPlayerTransform);
            OnLevelSwitchingFinished?.Invoke();
        }

        public virtual void CameraFade(bool fadeIn)
        {
            if (fadeIn) UIManager.Instance.FadeToBlack();
            else UIManager.Instance.FadeOut();
        }

        protected virtual void HandleNPCVoiceLines()
        {
            ++m_InteractionsCount;
            if (m_InteractionsCount == 1)
            {
                if (m_OnLevelLoadFirstTime is not null)
                    NPCManager.GetInstance().GetNPC(NPCType.Grandpa).ReactOnLevelLoadFirstTime(m_OnLevelLoadFirstTime);
            }
            else
            {
                if (m_OnLevelLoad is not null && m_OnLevelLoad.Count > 0)
                {
                    if (Random.Range(0, 2) == 0)
                        NPCManager.GetInstance().GetNPC(NPCType.Grandpa).ReactOnLevelLoad(m_OnLevelLoad[Random.Range(0, m_OnLevelLoad.Count)]);
                }
            }
        }
    }
}
