using TMPro;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public Note note;

    public void Read()
    {
        GameObject noteObj = GameObject.Find("NotesContent");
        noteObj.GetComponent<TextMeshProUGUI>().text = note.Content;
    }

    public void AddItem(Note newNote)
    {
        note = newNote;
    }
}
