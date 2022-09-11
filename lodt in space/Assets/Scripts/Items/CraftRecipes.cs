using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemAmount
{
    public ItemScriptableObj Item;
    public int Amount;
}

[CreateAssetMenu(menuName = "Game/New Craft Recipe")]
public class CraftRecipes : ScriptableObject
{
    public List<ItemAmount> RequiredItems;
    public ItemAmount CraftedItem;
}
