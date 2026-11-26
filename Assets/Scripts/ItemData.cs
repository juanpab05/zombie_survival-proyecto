using UnityEngine;

public enum ItemType
{
    Consumable, // Curaciones
    Weapon      // Armas
}

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [Header("Si es Curación")]
    public float healAmount = 25f;

    [Header("Si es Arma")]
    public int weaponIndex = 0;
}