using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    public ItemType itemType;
    public enum ItemType
    {
        Potion,
        Energy,
        Battery,
        Ammo,
        Fuel
    }
}
