using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProcStructParser : MonoBehaviour
{
    [SerializeField] private List<ProcessingStructRecipe> _recipes;
    [SerializeField] private List<CellContainer> _requiredItemCells; // из чего крафт
    [SerializeField] private CellContainer _processedItem; // что в итоге получается
    [SerializeField] private bool _isWorkingOnFuel; // нужно ли топливо
    [SerializeField] private CellContainer _fuelCell;

    [SerializeField] private bool _isWorking; // включена ли корутина
    [SerializeField] private ProcessingStructRecipe _recipeInWork; // для проверки создаваемого предмета

    private void OnEnable()
    {
        ParseStruct();
    }

    public void ParseStruct()
    {
        foreach (CellContainer cell in _requiredItemCells)
            ParseCell(cell);

        ParseCell(_processedItem);

        if (_isWorkingOnFuel)
            ParseCell(_fuelCell);

        ParseRecipes();
    }

    private void ParseRecipes()
    {
        ProcessingStructRecipe recipe = null;
        bool recipeAllowed = false; // правильно выложили вещи

        foreach (ProcessingStructRecipe rec in _recipes)
        {
            foreach (ItemAmount recipeItem in rec.RequiredItem)
            {
                foreach (CellContainer puttedItem in _requiredItemCells)
                {
                    if (recipeItem.Item == puttedItem.ItemPrefab)
                    {
                        recipe = rec;
                        _processedItem.transform.GetChild(1).GetComponent<Image>().sprite = rec.ProcessedItem.Item.Icon;
                        _processedItem.transform.GetChild(1).gameObject.SetActive(true);
                        recipeAllowed = true;
                        break;
                    }
                }
                if (recipeAllowed)
                    break;
            }
            if (recipeAllowed)
                break;
        }

        if (recipeAllowed)
        {
            _processedItem.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0.5f;

            if (_processedItem.ItemPrefab != null)
            {
                _processedItem.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
            }

            if (recipe != _recipeInWork)
            {
                _isWorking = true;
                StopAllCoroutines();
                _recipeInWork = recipe;

                //coroutine
            }
        }
        else
        {
            if (_processedItem.ItemPrefab == null)
                _processedItem.transform.GetChild(1).gameObject.SetActive(false);

            StopAllCoroutines();
            _isWorking = false;
            _recipeInWork = null;
        }
    }

    private void ParseCell(CellContainer cell)
    {
        if (cell.ItemPrefab != null)
        {
            cell.transform.GetChild(1).GetComponent<Image>().sprite = cell.ItemPrefab.Icon;
            cell.transform.GetChild(1).gameObject.SetActive(true);

            if (cell.ItemPrefab.IsCountable)
            {
                cell.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cell.Amount.ToString();
                cell.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                cell.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                cell.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        else
        {
            cell.transform.GetChild(1).gameObject.SetActive(false);
            cell.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
