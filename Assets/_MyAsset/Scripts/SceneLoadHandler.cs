using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoadHandler : MonoBehaviour
{
    public static SceneLoadHandler Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Không huỷ khi load scene
    }
    public void LoadSceneAfterCleanup(string sceneName)
    {
        StartCoroutine(LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        yield return null;
        Destroy(PlayerManager.Instance.gameObject);
        Destroy(CameraManager.Instance.gameObject);
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneWithInit(string sceneName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoaderManager.instance.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(DelayedInit());
    }

    private IEnumerator DelayedInit()
    {
        yield return null; // Đợi 1 frame
        //AudioManager.Instance.Reset();
        PlayerManager.Instance.InitPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadSceneWithInit("GamePlayLv1");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadSceneWithInit("GamePlayLv2");
        }

    }
}
