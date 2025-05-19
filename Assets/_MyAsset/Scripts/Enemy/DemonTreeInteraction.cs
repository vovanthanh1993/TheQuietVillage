using Unity.VisualScripting;
using UnityEngine;

public class DemonTreeInteraction : MonoBehaviour
{
    private bool inReach;
    [SerializeField] private GameObject demon;
    [SerializeField] private GameObject demonChild;
    [SerializeField] private GameObject flame;
    [SerializeField] private AudioClip demonDieSound;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform pos;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            GUIManager.Instance.ShowBurnText(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            GUIManager.Instance.ShowBurnText(false);
            GUIManager.Instance.ShowNeedFuelText(false);
        }
    }


    void Update()
    {
        if (inReach && InputManager.Instance.IsInteract())
        {
            GUIManager.Instance.ShowBurnText(false);
            if (InventoryManager.Instance.HasGasItem())
            {
                inReach = false;
                Destroy(GetComponent<Collider>());
                flame.SetActive(true);
                demon.SetActive(false);
                demonChild.SetActive(false);
                AudioManager.Instance.PlayEffect(demonDieSound);
                InventoryManager.Instance.RemoveItem(Item.ItemType.Fuel);
                Instantiate(enemy, pos.position, pos.rotation);
            } else
            {
                GUIManager.Instance.ShowNeedFuelText(true);
            }
            
        }
    }
}
