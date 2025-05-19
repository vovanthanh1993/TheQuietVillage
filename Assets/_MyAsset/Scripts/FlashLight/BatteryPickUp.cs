using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickUp : MonoBehaviour
{
    private bool inReach;

    void Start()
    {
        inReach = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            GUIManager.Instance.ShowPickUpText(true);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            GUIManager.Instance.ShowPickUpText(false);
        }
    }

    void Update()
    {
        if(InputManager.Instance.IsInteract() && inReach)
        {
            if (InventoryManager.Instance.IsInventoryHasSlot())
            {
                InventoryManager.Instance.AddItem(GetComponent<ItemController>().item);
                PlayerItem.Instance.flashlightAdvanced.GetComponent<FlashlightAdvanced>().AddBatteries(1);
                AudioManager.Instance.PickUpSound();
                inReach = false;
                GUIManager.Instance.ShowPickUpText(false);
                Destroy(gameObject);
            }
            else
            {
                GUIManager.Instance.ShowCantPickUpText(true);
            }

        }
    }
}
