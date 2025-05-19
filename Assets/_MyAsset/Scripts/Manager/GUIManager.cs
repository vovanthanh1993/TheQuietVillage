using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance { get; private set; }

    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject noteUI;
    [SerializeField] private GameObject openText;
    [SerializeField] private GameObject pickUpText;
    [SerializeField] private GameObject cantPickUpText;
    [SerializeField] private GameObject needAKey;
    [SerializeField] private GameObject foundAKey;
    [SerializeField] private GameObject burnText;
    [SerializeField] private GameObject needFuelText;

    [SerializeField] private GameObject flashLightGUI;
    [SerializeField] private GameObject flashLightOn;
    [SerializeField] private GameObject flashLightOff;
    [SerializeField] private GameObject screenHealing;
    [SerializeField] private GameObject screenTakeDmg;
    
    [SerializeField] private TMP_Text lifeTimeText;
    [SerializeField] private TMP_Text batteryNumText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private GameObject glockUI;
    [SerializeField] private GameObject knifeUI;
    [SerializeField] public GameObject crosshair;
    [SerializeField] public GameObject inventory;
    [SerializeField] public GameObject notesStore;

    [SerializeField] public GameObject quest1Text;
    [SerializeField] public GameObject quest2Text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowGlockUI(bool isShow)
    {
        if (glockUI != null)
            glockUI.SetActive(isShow);
    }

    public void ShowBurnText(bool isShow)
    {
        if (burnText != null)
            burnText.SetActive(isShow);
    }

    public void ShowQuest(int numQuest)
    {
        if(numQuest == 1)
        {
            quest1Text.SetActive(true);
            quest2Text.SetActive(false);
        } else
        {
            quest1Text.SetActive(false);
            quest2Text.SetActive(true);
        }
    }

    public void ResetQuest()
    {
        quest1Text.SetActive(false);
        quest2Text.SetActive(false);

    }

    public void ShowNeedFuelText(bool isShow)
    {
        if (needFuelText != null)
            needFuelText.SetActive(isShow);
    }

    public void ShowInventory(bool isShow)
    {
        if (inventory != null)
            inventory.SetActive(isShow);
        if (isShow) {
            InputManager.Instance.InputSystem.Disable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else
        {
            InputManager.Instance.InputSystem.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ShowNotes(bool isShow)
    {
        Time.timeScale = 1f;
        if (notesStore != null)
            notesStore.SetActive(isShow);
        if (isShow)
        {
            InputManager.Instance.InputSystem.Disable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            InputManager.Instance.InputSystem.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }


    }

    public void ShowKnifeUI(bool isShow)
    {
        if (knifeUI != null)
            knifeUI.SetActive(isShow);
    }

    public void ShowHUD(bool show)
    {
        if (hud != null)
            hud.SetActive(show);
    }

    public void ShowNoteUI(bool show, string content)
    {
        if (noteUI != null)
            noteUI.SetActive(show);
            Text text = noteUI.transform.Find("Note/Canvas/Content")?.GetComponent<Text>();
            if (text != null)
            {
                text.text = content;
            }
    }

    public void ShowPickUpText(bool show)
    {
        if (pickUpText != null)
            pickUpText.SetActive(show);
    }

    public void ShowFoundAKey(bool show)
    {
        if (foundAKey != null)
        {
            foundAKey.SetActive(show);
            StartCoroutine(TurnOffGameObject(foundAKey));
        }
            
    }

    public void ShowNeedAKeyText(bool show)
    {
        if (needAKey != null)
            needAKey.SetActive(show);
    }

    public void ShowOpenText(bool show)
    {
        if (openText != null)
            openText.SetActive(show);
    }

    public void ShowCantPickUpText(bool show)
    {
        if (cantPickUpText != null)
            cantPickUpText.SetActive(show);
    }

    public void ShowFlashLightOn(bool show)
    {
        ShowFlashLightGUI(true);
        if (flashLightOn != null)
            flashLightOn.SetActive(show);
        if (flashLightOff != null)
            flashLightOff.SetActive(!show);
    }

    public void ShowFlashLightGUI(bool show)
    {
        if (flashLightGUI != null)
            flashLightGUI.SetActive(show);
    }

    public void ShowScreenHealing()
    {
        if (screenHealing != null)
        {
            screenHealing.SetActive(true);
            StartCoroutine(TurnOffGameObject(screenHealing));
        }
    }
    public void ShowScreenTakeDmg()
    {
        if (screenTakeDmg != null)
        {
            screenTakeDmg.SetActive(true);
            StartCoroutine(TurnOffGameObject(screenTakeDmg));
        }     
    }

    IEnumerator TurnOffGameObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(1f);
        if (gameObject != null) gameObject.SetActive(false);
    }

    private void Start()
    {
        Reset();

    }

    public void FlashLightUIUpdate(string lifetime, string batteries)
    {
        if(lifeTimeText && batteryNumText)
        {
            lifeTimeText.text = lifetime;
            batteryNumText.text = batteries;
        }
        
    }

    public void AmmoTextUpdate(string ammo)
    {
        if(ammoText)
            ammoText.text = ammo;
    }

    public void Reset()
    {
        ShowInventory(false);
        ShowHUD(true);
        ShowPickUpText(false);
        ShowOpenText(false);
        ShowCantPickUpText(false);
        ShowNoteUI(false, "");
        ShowFlashLightGUI(false);
        ShowFlashLightOn(false);
        screenHealing.SetActive(false);
        screenTakeDmg.SetActive(false);
        ResetQuest();
    }
}
