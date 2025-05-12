using UnityEngine;

namespace Game.Core.LevelLogic
{
    public class GarbageCollection : Level
    {
        [SerializeField] private TrashSpawner m_TrashSpawner;

        protected override void BeforeLevelLoad()
        {
            base.BeforeLevelLoad();
            m_TrashSpawner.DeleteTrash();
            m_TrashSpawner.SpawnTrashPile(1);
        }

        private void CalculateSpawnAmount()
        {
            
        }
    }
}
