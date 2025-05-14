using Game.Core;
using Game.Core.LevelLogic;
using UnityEngine;

public class GasPump : MonoBehaviour, IInteractable
{
    [SerializeField] private LevelSwitcher m_LevelSwitcherToTrigger;
    
    public void OnInteract()
    {
        GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().RefillFuelTank();
        m_LevelSwitcherToTrigger.OnInteract();
    }
}
