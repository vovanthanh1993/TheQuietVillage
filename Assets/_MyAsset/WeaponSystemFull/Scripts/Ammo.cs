using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int ammoBoxAmount;
    public bool inreach;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inreach = true;
            GUIManager.Instance.ShowPickUpText(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inreach = false;
            GUIManager.Instance.ShowPickUpText(false);
        }
    }



    void Update()
    {
        if(inreach && InputManager.Instance.IsInteract())
        {
            

            if (InventoryManager.Instance.IsInventoryHasSlot())
            {
                InventoryManager.Instance.AddItem(GetComponent<ItemController>().item);
                PlayerItem.Instance.UpdatePistolAmmo(ammoBoxAmount);
                GUIManager.Instance.ShowPickUpText(false);
                AudioManager.Instance.PickUpSound();
                Destroy(gameObject);
            }
            else
            {
                GUIManager.Instance.ShowCantPickUpText(true);
            }

        }
    }

    private void Awake()
    {
        if(ammoBoxAmount == 0) ammoBoxAmount  = 10;
    }
}
