using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReadNotes : MonoBehaviour
{
    private bool inReach = false;
    private bool isReading = false;

    [Header("Content")]
    [TextArea]
    public string noteName;
    [TextArea]
    public string content;

    void Start()
    {
        GUIManager.Instance.ShowNoteUI(false, content);
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
        if (InputManager.Instance.IsInteract() && isReading)
        {
            Time.timeScale = 1;
            GUIManager.Instance.ShowNoteUI(false, "");
            GUIManager.Instance.ShowHUD(true);
            isReading = false;
            PlayerManager.Instance.gameObject.SetActive(true);
            InputManager.Instance.InputSystem.Player.Enable();
            InputManager.Instance.InputSystem.FindAction("UI/Escape").Enable();
            if (TutorialManager.Instance != null)
                TutorialManager.Instance.ShowOpenNotes();
            Destroy(gameObject);
            
        }

        if (InputManager.Instance.IsInteract() && inReach)
        {
            GUIManager.Instance.ShowNoteUI(true, content);
            NotesManager.Instance.AddItem(new Note(noteName, content));
            AudioManager.Instance.OpenNote();
            GUIManager.Instance.ShowHUD(false);
            GUIManager.Instance.ShowPickUpText(false);
            PlayerManager.Instance.gameObject.SetActive(false);
            InputManager.Instance.InputSystem.Player.Disable();
            InputManager.Instance.InputSystem.FindAction("UI/Escape").Disable();
            Time.timeScale = 0;
            isReading = true;
            GetComponent<BoxCollider>().enabled = false;
            inReach = false;
        }
    }
}
