using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/New Processing Recipe (struct)")]
public class ProcessingStructRecipe : ScriptableObject
{
    public List<ItemAmount> RequiredItem;
    public ItemAmount ProcessedItem;
    public int TimeToCraft;
}
