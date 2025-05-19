using UnityEngine;

public class Note : ScriptableObject
{
    private string noteName;
    private string content;

    public string NoteName
    {
        get { return noteName; }
        set { noteName = value; }
    }

    public string Content
    {
        get { return content; }
        set { content = value; }
    }

    // Constructor (dùng cho khi khởi tạo từ script, không từ Unity Editor)
    public Note(string noteName, string content)
    {
        this.noteName = noteName;
        this.content = content;
    }
}
