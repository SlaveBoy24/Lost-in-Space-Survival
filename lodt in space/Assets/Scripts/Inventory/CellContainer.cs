using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class CellContainer : MonoBehaviour
{
    public ItemScriptableObj ItemPrefab;
    public int Amount;
    public float Durability;
    public List<ItemType> AllowedItems;
    public bool Closed;
    public bool isBackpackSlot;
 
    public CellContainer(ItemScriptableObj item, int amount, float durability, List<ItemType> allolwedItems)
    { 
        ItemPrefab = item;
        Amount = amount;
        Durability = durability;
        AllowedItems = allolwedItems;
    }

    public void ClearCell()
    {
        ItemPrefab = null;
        Amount = 0;
        Durability = 0;
    }

    public void RebuildSlot(CellContainer newItem)
    {
        ItemPrefab = newItem.ItemPrefab;
        Amount = newItem.Amount;
        Durability = newItem.Durability;
        AllowedItems = newItem.AllowedItems;
        Closed = newItem.Closed;
        isBackpackSlot = newItem.isBackpackSlot;
    }

    public void NewItemInCell(CellContainer newItem)
    {
        ItemPrefab = newItem.ItemPrefab;
        Amount = newItem.Amount;
        Durability = newItem.Durability;
    }
}
