using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _openCelsWithoutBackpack;
    [SerializeField] private int _backpackCapacity;
    [SerializeField] private Transform _cellsList;
    [SerializeField] private List<CellContainer> _items;
    [SerializeField] private List<Transform> _cells;

    [SerializeField] private CellContainer _backPack;
    [SerializeField] private CellContainer _weapon;
    [SerializeField] private List<CellContainer> _supItems;
    [SerializeField] private List<CellContainer> _chips; // чипы

    private void Start()
    {
        if (_cells.Count == 0 || _cells == null)
        {
            for (int i = 0; i < _cellsList.childCount; i++)
            {
                _cells.Add(_cellsList.GetChild(i));
                _items.Add(_cellsList.GetChild(i).GetComponent<CellContainer>());
            }
        }
    }

    public void ParseInventory() // парсинг слотов экипировки (правая часть в инвентаре)
    {
        ParseEquipment(new List<CellContainer>() { _backPack, _weapon });
        ParseEquipment(_supItems);
        ParseEquipment(_chips);
    }

    public void ParseEquipment(List<CellContainer> parseCell)
    {
        for (int i = 0; i < parseCell.Count; i++)
        {
            if (parseCell[i].ItemPrefab != null)
            {
                parseCell[i].transform.GetChild(1).GetComponent<Image>().sprite = parseCell[i].ItemPrefab.Icon;
                parseCell[i].transform.GetChild(1).gameObject.SetActive(true);
                parseCell[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                parseCell[i].ClearCell();
                parseCell[i].transform.GetChild(1).gameObject.SetActive(false);
                parseCell[i].transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void RebuildInventory(List<CellContainer> items)
    {
        for (int i = 0; i < _cellsList.childCount; i++)
        {
            _items[i].RebuildSlot(items[i]);
        }
    }

    public int GetBackpackCapasity()
    {
        if (_backPack.ItemPrefab != null)
            return _backPack.ItemPrefab.Capacity;
        else
            return 0;
    }

    public List<CellContainer> GetParsedInventory()
    {
        return _items;
    }

    public void UseTool(ToolType type)
    {
        for (int i = 0; i < _openCelsWithoutBackpack + _backpackCapacity; i++)
        {
            if (_items[i].ItemPrefab != null && _items[i].ItemPrefab.Type == ItemType.Tool && _items[i].ItemPrefab.Tool == type)
            {
                _items[i].Durability -= 1;

                if (_items[i].Durability <= 0)
                    _items[i].ClearCell();

                return;
            }
        }
    }

    public bool FindTool(ToolType type)
    {
        for (int i = 0; i < _openCelsWithoutBackpack + _backpackCapacity; i++)
        {
            if (_items[i].ItemPrefab != null && _items[i].ItemPrefab.Type == ItemType.Tool && _items[i].ItemPrefab.Tool == type)
                return true;
        }

        return false;
    }

    public int ItemCount(ItemScriptableObj item)
    {
        int Amount = 0;

        foreach (CellContainer cell in _items)
        {
            if (cell.ItemPrefab == item)
                Amount += cell.Amount;
        }

        return Amount;
    }

    public void RemoveItem(ItemScriptableObj item, int amount)
    {
        for (int i = 0; i < _openCelsWithoutBackpack + _backpackCapacity; i++)
        {

            // добавить проход по стакам
            if (_items[i].ItemPrefab == item)
            {
                _items[i].Amount -= amount;

                if (_items[i].ItemPrefab.IsCountable && _items[i].Amount == 0)
                    _items[i].ClearCell();

                return;
            }
        }
    }

    public void AddItem(ItemScriptableObj item, int amount, bool newStack = false)
    {
        int amountLess = 0;

        if (item.IsCountable)
        {
            for (int i = 0; i < _openCelsWithoutBackpack + _backpackCapacity; i++)
            {
                if (_items[i].ItemPrefab == item)
                {
                    _items[i].Amount += amount + amountLess;
                    amountLess = 0;
                    amountLess = _items[i].Amount - item.MaxAmount;

                    if (amountLess > 0)
                    {
                        amount = 0;
                        _items[i].Amount = item.MaxAmount;
                        continue;
                    }

                    return;
                }
            }
        }

        for (int i = 0; i < _openCelsWithoutBackpack + _backpackCapacity; i++)
        {
            if (_items[i].ItemPrefab == null || _items[i].ItemPrefab == item)
            {
                _items[i].ItemPrefab = item;

                if (item.IsCountable)
                    _items[i].Amount += amount + amountLess;
                else
                    _items[i].Amount = 1;

                // стак
                if (item.IsCountable)
                {
                    amountLess = 0;
                    amountLess = _items[i].Amount - item.MaxAmount;

                    if (amountLess > 0)
                    {
                        amount = 0;
                        _items[i].Amount = item.MaxAmount;
                        continue;
                    }
                }

                if (item.IsBreakable)
                    _items[i].Durability = item.Durability;

                return;
            }
        }
    }
}
