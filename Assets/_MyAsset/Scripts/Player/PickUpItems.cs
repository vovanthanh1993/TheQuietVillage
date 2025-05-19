using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItems : MonoBehaviour
{

    private bool inReach;

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
            GUIManager.Instance.ShowCantPickUpText(false);
        }
    }


    void Update()
    {
        if(inReach && InputManager.Instance.IsInteract())
        {
            GUIManager.Instance.ShowPickUpText(false);
            if (InventoryManager.Instance.IsInventoryHasSlot())
            {
                InventoryManager.Instance.AddItem(GetComponent<ItemController>().item);
                AudioManager.Instance.PickUpSound();
                Destroy(gameObject);
            } else
            {
                GUIManager.Instance.ShowCantPickUpText(true);
            }
        }
        
    }
}
