using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{ 
    Tool,
    Food,
    Medicament,
    Resource,
    Weapon,
    Chip,
    Material,
    Gas,
    Construction,
    BackPackChip
}

public enum ToolType
{
    None,
    Axe,
    Pickaxe
}

public enum ItemRarity
{ 
    Normal,
    Rare,
    Epic,
    Unique,
    Legendary
}
[CreateAssetMenu(menuName = "Game/New Item")]
public class ItemScriptableObj : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public GameObject ItemPrefab;

    [Header("Item Count")]
    public bool IsCountable;
    public int MaxAmount;

    [Header("Status")]
    public ItemType Type;
    public ToolType Tool;

    [Header("Backpack")]
    public int Capacity;

    [Header("Durability")]
    public bool IsBreakable;
    [Range(0, 100)] public float Durability;

    [Header("Rarity")]
    public ItemRarity Rarity;

    [Header("Economy")]
    public int Cost;

    [Header("Stats")]
    public float Damage;
    public float Healing;
    public float UsingRange;
}
