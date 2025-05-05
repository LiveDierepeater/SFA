using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void InventoryEvents();
    public InventoryEvents OnInventoryAddedItem;

    [SerializeField] private List<CarComponent> m_CarComponents = new();

    public void AddCarComponentToInventory(CarComponent carComponent)
    {
        if (IsComponentInInventory(carComponent))
            throw new WarningException($"Component {carComponent} is already in inventory!");

        m_CarComponents.Add(carComponent);
        OnInventoryAddedItem?.Invoke();
    }
    
    public bool IsComponentInInventory(CarComponent component) => m_CarComponents.Any(carComponent => carComponent == component);
    public List<CarComponent> GetCarComponents() => m_CarComponents;
}
