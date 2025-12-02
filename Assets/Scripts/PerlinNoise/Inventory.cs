using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Dictionary<BlockType, int> items = new();
    InventoryUi ui;

    private void Awake()
    {
        ui = FindAnyObjectByType<InventoryUi>();   
    }
    public void Add(BlockType type, int count = 1)
    {
        if (!items.ContainsKey(type)) items[type] = 0;
        items[type] += count;
        Debug.Log($"[Inventory] +{count} {type} (รั {items[type]})");

        ui.UpdateInventory(this);
    }

    public bool Consume(BlockType type, int count = 1)
    {
        if (!items.TryGetValue(type, out var have) || have < count) return false;
        items[type] = have - count;
        Debug.Log($"[Inventory] -{count} {type} (รั {items[type]})");
        if (items[type] == 0)
        {
            items.Remove(type);
            ui.selectedIndex = -1;
            ui.ResetSelection();
        }

        ui.UpdateInventory(this);
        return true;
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
