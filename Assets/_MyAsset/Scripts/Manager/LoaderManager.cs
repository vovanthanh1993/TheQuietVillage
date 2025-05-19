using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class LoaderManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private GameObject textPrompt;
    [SerializeField] private GameObject canvas;

    [Header("Loading State")]
    public bool isLoading = false;

    // Hàm này sẽ gọi khi bạn bắt đầu tải một scene
    private void Start()
    {
        Init();
    }
    public void LoadScene(string sceneToLoad)
    {
        isLoading = true;
        if (InputManager.Instance != null)  InputManager.Instance.InputSystem.Disable();
        canvas.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    // Coroutine để tải scene không đồng bộ
    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        AudioManager.Instance.Reset();
        // ✅ Bắt đầu: hiện thông báo chuyển cảnh
        if (loadingText != null)
        {
            loadingText.text = "Loading...";
        }

        yield return new WaitForSeconds(0.5f);

        // Bắt đầu tải scene không đồng bộ
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Đảm bảo màn hình loader không bị tắt khi scene đang tải
        asyncOperation.allowSceneActivation = false;

        // Hiển thị tỷ lệ phần trăm đang tải
        while (!asyncOperation.isDone)
        {
            // Cập nhật thanh tải hoặc text nếu muốn
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);  // Lấy tiến độ tải

            slider.value = progress;

            // Cập nhật Text loading nếu có
            if (loadingText != null)
            {
                loadingText.text = (progress * 100f).ToString("F0") + "%";
            }

            // Khi quá trình tải đạt 90%, cho phép chuyển đến scene
            if (asyncOperation.progress >= 0.9f)
            {

                loadingText.gameObject.SetActive(false);
                textPrompt.SetActive(true);

                // Đợi đến khi người chơi nhấn phím
                while (!Input.GetKeyDown(KeyCode.Space)) // hoặc dùng Input.GetKeyDown(KeyCode.Space) để cụ thể phím
                {
                    yield return null;
                }
                textPrompt.SetActive(false);
                slider.gameObject.SetActive(false);
                loadingText.gameObject.SetActive(false);
                yield return new WaitForSeconds(1f);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        InputManager.Instance.InputSystem.Enable();
        Init();
    }

    public static LoaderManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Đã có rồi thì huỷ bản mới
        }
    }

    public void Init()
    {
        if (InputManager.Instance != null) InputManager.Instance.InputSystem.Enable();
        isLoading = false;
        Time.timeScale = 1f; // Resume
        AudioListener.pause = false;
        canvas.SetActive(false);
        slider.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        textPrompt.SetActive(false);
        slider.value = 0f;
    }
}
