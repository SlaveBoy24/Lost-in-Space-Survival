using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _buttonSprites;

    [SerializeField] private Inventory _inventory;
    [SerializeField] private Transform _recipesList;
    [SerializeField] private TextMeshProUGUI _craftingItemName;
    [SerializeField] private Image _craftingItemIcon;
    [SerializeField] private GameObject _craftButton;
    [SerializeField] private Transform _requiredItemsList;

    private CraftRecipes _recipe;
    private bool _enableToCraft;

    private void OnEnable()
    {
        ShowCraft(_recipesList.GetChild(0).GetComponent<CraftingCell>().CraftItem);
    }

    private void OnDisable()
    {
        ClearCraft();
    }

    public void ShowCraft(CraftRecipes craftingItem)
    {
        ClearCraft();

        _recipe = craftingItem;
        _enableToCraft = true;

        _craftingItemIcon.sprite = _recipe.CraftedItem.Item.Icon;
        _craftingItemName.text = _recipe.CraftedItem.Item.Name.ToUpper();

        for (int i = 0; i < _recipe.RequiredItems.Count; i++)
        {
            int itemAmountInventory = _inventory.ItemCount(_recipe.RequiredItems[i].Item);

            if (_recipe.RequiredItems[i].Amount > itemAmountInventory)
                _enableToCraft = false;

            _requiredItemsList.GetChild(i).transform.GetChild(1).GetComponent<Image>().sprite = _recipe.RequiredItems[i].Item.Icon;
            _requiredItemsList.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = itemAmountInventory.ToString() + "/" + _recipe.RequiredItems[i].Amount.ToString();
            _requiredItemsList.GetChild(i).gameObject.SetActive(true);
        }

        if (_enableToCraft)
        {
            _craftButton.GetComponent<Image>().sprite = _buttonSprites[0];
            _craftButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(58, 253, 66, 255);
        }
        else
        {
            _craftButton.GetComponent<Image>().sprite = _buttonSprites[1];
            _craftButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        }
    }

    public void Craft()
    {
        if (_enableToCraft)
        {
            for (int i = 0; i < _recipe.RequiredItems.Count; i++)
            {
                _inventory.RemoveItem(_recipe.RequiredItems[i].Item, _recipe.RequiredItems[i].Amount);
            }

            _inventory.AddItem(_recipe.CraftedItem.Item, _recipe.CraftedItem.Amount);
        }

        ShowCraft(_recipe);
    }

    private void ClearCraft()
    {
        _craftingItemIcon.sprite = null;
        _craftingItemName.text = "Craft";

        for (int i = 0; i < 5; i++)
        { 
            _requiredItemsList.GetChild(i).gameObject.SetActive(false);
        }
    }
}
