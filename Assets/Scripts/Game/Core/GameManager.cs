using UnityEngine;

namespace Game.Core
{
    using Player;
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Player Player;
        
        public delegate void PlayerCommunication(Player player);
        public PlayerCommunication OnPlayerInitialize;

        private void Awake() => Initialize();
        private void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("TickSystem already initialized.");
                Destroy(gameObject);
            }
        }

        private void Start() => OnPlayerInitialize += SetPlayerRef;
        private void SetPlayerRef(Player player) => Player = player ?? Player;
    }
}
