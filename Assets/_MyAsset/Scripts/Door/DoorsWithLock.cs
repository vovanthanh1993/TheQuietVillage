using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DoorsWithLock : MonoBehaviour
{
    [Header("References")]
    public Animator door;
    public string requiredKeyID;

    [Header("Audio")]
    public AudioClip doorSound;
    public AudioClip doorLockSound;

    private bool isOpen = false;

    private bool inReach = false;
    private bool hasKey = false;
    private bool unlocked = false;
    private bool locked = true;

    void Start()
    {
        GUIManager.Instance.ShowOpenText(false);
        GUIManager.Instance.ShowNeedAKeyText(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = true;
            GUIManager.Instance.ShowOpenText(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            GUIManager.Instance.ShowOpenText(false);
            GUIManager.Instance.ShowNeedAKeyText(false);
        }
    }

    void Update()
    {
        

        if (inReach && InputManager.Instance.IsInteract())
        {
            CheckKey();
            if (unlocked)
            {
                if (!isOpen)
                {
                    DoorOpens();
                    isOpen = true;
                }
                else
                {
                    DoorCloses();
                    isOpen = false;
                }
            }
            else if (locked)
            {
                AudioManager.Instance.PlayEffect(doorLockSound);
                StartCoroutine(NeedAKeyRoutine());
            }
        }
    }

    private void CheckKey()
    {
        if (PlayerItem.Instance.HasKey(requiredKeyID))
        {
            hasKey = true;
            unlocked = true;
            locked = false;
            GUIManager.Instance.ShowNeedAKeyText(false);
        }
        else
        {
            hasKey = false;
            unlocked = false;
            locked = true;
        }
    }

    private IEnumerator NeedAKeyRoutine()
    {
        GUIManager.Instance.ShowNeedAKeyText(true);
        yield return new WaitForSeconds(2);
        GUIManager.Instance.ShowNeedAKeyText(false);
    }

    private void DoorOpens()
    {
        door.SetBool("Open", true);
        door.SetBool("Closed", false);
        AudioManager.Instance.PlayEffect(doorSound);
        transform.Find("Door").gameObject.GetComponent<NavMeshObstacle>().enabled = false;
    }

    private void DoorCloses()
    {
        door.SetBool("Open", false);
        door.SetBool("Closed", true);
        AudioManager.Instance.PlayEffect(doorSound);
        transform.Find("Door").gameObject.GetComponent<NavMeshObstacle>().enabled = true;
    }
}
