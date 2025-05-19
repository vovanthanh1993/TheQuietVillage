using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using TMPro;


public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance { get; private set; }

    public List<Note> items = new List<Note>(); // Giả sử Item là ScriptableObject

    public Transform itemContent;
    public NoteController[] notesController;
    public GameObject itemPrefab;
    public GameObject noteBackground;

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

    public void AddItem(Note note)
    {
        items.Add(note);
    }

    public void RemoveItem(Note note)
    {
        items.Remove(note);
    }

    public void Reset()
    {
        //items = new List<Note>();
        foreach (Transform item in itemContent)
        {
            Destroy(item);
        }
    }

    public void ShowListItems()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        if (items.Count > 0)
        {
            noteBackground.SetActive(true);
            StartCoroutine(DelayedSetItems());
            GameObject noteObj = GameObject.Find("NotesContent");
            noteObj.GetComponent<TextMeshProUGUI>().text = items[0].Content;
        } else
        {
            noteBackground.SetActive(false);
        }
    }

    private void Update()
    {
        if (InputManager.Instance.OpenNotes())
        {
            GUIManager.Instance.ShowNotes(true);
            ShowListItems();
            Time.timeScale = 0f;
        }
    }

    private void SetItem()
    {
        notesController = itemContent.GetComponentsInChildren<NoteController>();
        if (notesController.Length > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                notesController[i].AddItem(items[i]);
            }
        }
    }

    private IEnumerator DelayedSetItems()
    {
        yield return null;
        foreach (Note item in items)
        {
            GameObject obj = Instantiate(itemPrefab, itemContent);
            TextMeshProUGUI text = obj.transform.Find("NotesName").GetComponent<TextMeshProUGUI>();
            text.text = item.NoteName;
        }
        SetItem();
    }
}
