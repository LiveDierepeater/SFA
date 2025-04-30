using Game.Core;
using UnityEngine;

namespace Game.Player
{
    public class Player : MonoBehaviour
    {
        private void Start() => GameManager.Instance.OnPlayerInitialize(this);

        public void LevelTransition(Transform targetTransform)
        {
            // TODO: Is player able to transit to next level?
            transform.position = targetTransform.position;
        }
    }
}
