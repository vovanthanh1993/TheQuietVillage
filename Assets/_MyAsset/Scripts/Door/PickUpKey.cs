using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    [Header("References")]
    public string keyId;
    public AudioClip pickUpKeySound;

    private bool inReach = false;

    void Start()
    {
        inReach = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = true;
            GUIManager.Instance.ShowPickUpText(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            GUIManager.Instance.ShowPickUpText(false);
        }
    }

    void Update()
    {
        if (inReach && InputManager.Instance.IsInteract())
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        AudioManager.Instance.PlayEffect(pickUpKeySound);
        PlayerItem.Instance.AddKey(keyId);
        GUIManager.Instance.ShowPickUpText(false);
        GUIManager.Instance.ShowFoundAKey(true);
        Destroy(gameObject);
    }
}
