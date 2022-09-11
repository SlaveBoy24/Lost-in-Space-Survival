using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StorageParser : MonoBehaviour
{
    [SerializeField] private Transform _cellsList;
    [SerializeField] private List<CellContainer> _items;
    [SerializeField] private List<Transform> _cells;
    [SerializeField] private int _storageCapacity;

    private void OnEnable()
    {
        if (_cells.Count == 0 || _cells == null)
        {
            for (int i = 0; i < _cellsList.childCount; i++)
            {
                _cells.Add(_cellsList.GetChild(i));
                _items.Add(_cellsList.GetChild(i).GetComponent<CellContainer>());
            }
        }

        ParseStorage();
    }

    public void IncreaseStorageCapsity() // улучшение сундука
    { 
    
    }

    public void ParseStorage()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (i < _storageCapacity)
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
                
                _items[i].Closed = false;
            }
            else
            {
                _cells[i].gameObject.SetActive(false);

                _items[i].Closed = true;
            }
        }
    }
}
