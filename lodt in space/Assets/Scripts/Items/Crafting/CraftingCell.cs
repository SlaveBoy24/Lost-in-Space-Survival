using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CraftingCell : MonoBehaviour
{
    public CraftRecipes CraftItem;
    public int RequiredLevel;

    private void Start()
    {
        transform.GetChild(1).GetComponent<Image>().sprite = CraftItem.CraftedItem.Item.Icon;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ShowCraft(CraftingManager craftManager)
    { 
        craftManager.ShowCraft(CraftItem);
    }
}
