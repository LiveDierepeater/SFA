using UnityEngine;

namespace Game.Core
{
    using Player;
    
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Player m_Player;
        
        public delegate void PlayerCommunication(Player player);
        public PlayerCommunication OnPlayerInitialize;

        private void Awake() => Initialize();
        private void Initialize()
        {
            OnPlayerInitialize += SetPlayerRef;
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("TickSystem already initialized!");
                Destroy(gameObject);
            }
        }

        private void SetPlayerRef(Player player) => m_Player = player ? player : m_Player;
    }
}
