using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Camera cam;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
