using System;
using Game.Core;
using Game.Core.LevelLogic;
using UnityEngine;

public class GarageDoor : LevelSwitcher
{
    [SerializeField] private Garage m_Garage;

    protected override void CallLevelSwitch()
    {
        if (m_Garage == null)
            throw new Exception("Garage is null!");
        
        if (m_Garage.GetCarProxy() == null)
            throw new Exception("CarProxy is null!");

        if (m_Garage.GetCarProxy().HasAllComponentsBuildIn())
        {
            GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().UpdateCarStats(m_Garage.GetCarProxy().GetInstalledCarComponents());
            GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().RefillFuelTank();
            GameManager.Instance.m_Player.GetPlayerController().GetCarController().GetCar().UpdateWheelFrictionValues();
            base.CallLevelSwitch();
        }
    }
}
