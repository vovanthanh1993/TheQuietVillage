using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using static Item;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<Item> items = new List<Item>(); // Giả sử Item là ScriptableObject

    public Transform itemContent;
    public ItemController[] InventoryItems;
    public int itemMaxNum = 12;
    private bool isFisrtTime = true;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi load scene mới
        }
        else
        {
            Destroy(gameObject); // Nếu đã có instance khác, hủy cái mới
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        if(isFisrtTime && item.itemType== ItemType.Potion && TutorialManager.Instance!= null)
        {
            TutorialManager.Instance.ShowInventoryTutorial();
            isFisrtTime = false;
        }
        Debug.Log("Added item: " + item.name);
    }

    public bool IsInventoryHasSlot()
    {
        return (items.Count < itemMaxNum);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        Debug.Log("Removed item: " + item.name);
    }

    public void RemoveItem(ItemType itemType)
    {
        foreach (Item item in items)
        {
            if (item.itemType == itemType)
            {
                items.Remove(item);
                break;
            }
        }
    }

    public bool HasGasItem()
    {
        foreach (Item item in items)
        {
            if (item.itemType == ItemType.Fuel)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveAmmo(int num)
    {
        int ammoCount = 0;
        List<Item> itemsToRemove = new List<Item>();

        foreach (Item item in items)
        {
            if (item.itemType == ItemType.Ammo)
            {
                ammoCount++;
                if (ammoCount > num)
                {
                    itemsToRemove.Add(item);
                }
            }
        }

        foreach (Item item in itemsToRemove)
        {
            items.Remove(item);
        }
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public void Reset()
    {
        items = new List<Item>();
        foreach (Transform item in itemContent)
        {
            item.Find("ItemIcon").GetComponent<Image>().enabled = false;
            item.GetComponent<ItemController>().item = null;
        }
    }

    public void ShowListItems() {
        int count = 0;

        foreach (Transform item in itemContent)
        {
            //Destroy(item.gameObject);
            Image itemIcon = item.Find("ItemIcon").GetComponent<Image>();
            if (count < items.Count)
            {

                itemIcon.enabled = true;
                itemIcon.sprite = items[count].icon;
                count++;
            }
            else
            {
                itemIcon.enabled = false;
                item.GetComponent<ItemController>().item = null;
            }
        }
        //StartCoroutine(DelayedSetItems());
        SetItem();
    }

    private void Update()
    {
        if (InputManager.Instance.InputInventory())
        {
            GUIManager.Instance.ShowInventory(true);
            ShowListItems();
            
        }
    }

    private void SetItem()
    {
        InventoryItems = itemContent.GetComponentsInChildren<ItemController>();
        if(InventoryItems.Length > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                InventoryItems[i].AddItem(items[i]);
            }
        }
    }

    /*private IEnumerator DelayedSetItems()
    {
        yield return null; // chờ 1 frame
        foreach (Item item in items)
        {
            GameObject obj = Instantiate(itemPrefab, itemContent);
            Image itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            itemIcon.sprite = item.icon;
        }

        SetItem();
    }*/
}
