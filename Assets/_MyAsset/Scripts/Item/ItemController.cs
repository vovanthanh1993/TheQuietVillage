using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public Item item;

    public void UseItem()
    {
        if (item == null) return;
        switch (item.itemType)
        {
            case Item.ItemType.Potion:
                PlayerHealth.Instance.AddHealth(item.value);
                RemoveItem();
                break;
            case Item.ItemType.Energy:
                PlayerHealth.Instance.AddHealth(item.value);
                RemoveItem();
                break;
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.RemoveItem(item);
        Image itemIcon = transform.Find("ItemIcon").GetComponent<Image>();
        itemIcon.enabled = false;
        item = null;
    }


}
