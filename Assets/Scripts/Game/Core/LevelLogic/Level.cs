using System.Collections;
using Game.Core.Interfaces;
using UnityEngine;

namespace Game.Core.LevelLogic
{
    public abstract class Level : MonoBehaviour, ILevel
    {
        [SerializeField] private Transform m_targetTransform;
        [SerializeField] private Transform m_resetTransform;
        
        [SerializeField] private float LoadingDuration = 1f;

        public delegate void LevelSwitching();
        public LevelSwitching OnLevelSwitchingAccepted;
        public LevelSwitching OnLevelSwitchingFinished;

        public virtual void SwitchToLevel()
        {
            BeforeLevelLoad();
            StartCoroutine(nameof(LoadLevel), m_targetTransform);
        }

        public virtual void ResetToLevel() => PlayerTransition(m_resetTransform);

        public virtual void BeforeLevelLoad()
        {
            OnLevelSwitchingAccepted?.Invoke();
            CameraFade(true);
        }

        public virtual IEnumerator LoadLevel(Transform targetPlayerTransform)
        {
            yield return new WaitForSeconds(LoadingDuration);
            
            // TODO: Implement level loading logic
            
            PlayerTransition(targetPlayerTransform);
            CameraFade(false);
            OnLevelSwitchingFinished?.Invoke();
        }

        public void PlayerTransition(Transform targetPlayerTransform)
        {
            GameManager.Instance.Player.LevelTransition(targetPlayerTransform);
            OnLevelSwitchingFinished?.Invoke();
        }

        public void CameraFade(bool fadeIn)
        {
            if (fadeIn) UIManager.Instance.FadeToBlack();
            else UIManager.Instance.FadeOut();
        }
    }
}
