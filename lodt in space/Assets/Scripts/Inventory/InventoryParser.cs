using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryParser : MonoBehaviour
{
    [SerializeField] private Transform _cellsList;
    [SerializeField] private List<CellContainer> _items;
    [SerializeField] private List<Transform> _cells;
    [SerializeField] private int _backpackCapacity;

    private void OnEnable()
    {
        Inventory inventory = GameObject.FindGameObjectWithTag("MainCharacter").GetComponent<Inventory>();
        inventory.ParseInventory();
        List<CellContainer>  supItemsList = inventory.GetParsedInventory();
        _backpackCapacity = inventory.GetBackpackCapasity();

        if (_cells.Count == 0 || _cells == null)
        {
            for (int i = 0; i < _cellsList.childCount; i++)
            {
                _cells.Add(_cellsList.GetChild(i));
                _items.Add(_cellsList.GetChild(i).GetComponent<CellContainer>());
            }
        }

        for (int i = 0; i < _cellsList.childCount; i++)
        {
            _items[i].GetComponent<CellContainer>().RebuildSlot(supItemsList[i]);
        }


        ParseInventory();
    }

    public void ParseInventory()
    {
        Inventory inventory = GameObject.FindGameObjectWithTag("MainCharacter").GetComponent<Inventory>();
        inventory.ParseInventory();
        List<CellContainer> supItemsList = inventory.GetParsedInventory();
        _backpackCapacity = inventory.GetBackpackCapasity();

        ParseEnabledCells();
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

    public void ParseEnabledCells()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (i < 15 + _backpackCapacity)
            {
                if (_items[i].ItemPrefab != null)
                {
                    _cells[i].GetChild(1).GetComponent<Image>().sprite = _items[i].ItemPrefab.Icon;
                    _cells[i].GetChild(1).gameObject.SetActive(true);

                    if (_items[i].ItemPrefab.IsCountable)
                    {
                        _cells[i].GetChild(3).GetComponent<TextMeshProUGUI>().text = _items[i].Amount.ToString();
                        _cells[i].GetChild(3).gameObject.SetActive(true);
                    }
                    else
                    {
                        _cells[i].GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
                        _cells[i].GetChild(3).gameObject.SetActive(false);
                    }
                }
                else
                {
                    _cells[i].GetChild(1).gameObject.SetActive(false);
                    _cells[i].GetChild(3).gameObject.SetActive(false);
                }

                _cells[i].GetChild(2).gameObject.SetActive(false);
                if (_items[i].isBackpackSlot)
                    _items[i].Closed = false;
            }
            else
            {
                _cells[i].GetChild(2).gameObject.SetActive(true);
                if (_items[i].isBackpackSlot)
                    _items[i].Closed = true;
            }
        }

        GameObject.FindGameObjectWithTag("MainCharacter").GetComponent<Inventory>().RebuildInventory(_items);
    }
}
