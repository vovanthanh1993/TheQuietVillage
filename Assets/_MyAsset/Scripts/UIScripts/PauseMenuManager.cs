using System.Collections;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{

    private bool isPaused = false;
    private bool isSettingOpen = false;

    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingMenuUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private GameObject gameOverUI;

    public static PauseMenuManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Nếu đã có một Instance khác, hủy đối tượng này
        }
    }

    void Update()
    {
        if (LoaderManager.instance.isLoading) return;
        if (InputManager.Instance.IsPressEscape() && !gameOverUI.activeSelf) // Bấm ESC để mở/tắt menu
        {
            if(settingMenuUI.activeSelf)
            {
                ShowSettingsPanel(false);
            } else 
            {
                if (isPaused)
                    Resume();
                else
                    Pause();
            }
        }
    }

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
    }
    public void Resume()
    {
        InputManager.Instance.InputSystem.Player.Enable();
        InputManager.Instance.InputSystem.FindAction("UI/Interact").Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioManager.Instance.Resume();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game
        isPaused = false;
        GUIManager.Instance.ShowHUD(true);
    }

    public void Settings()
    {
        ShowSettingsPanel(true);
    }

    public void Pause()
    {
        InputManager.Instance.InputSystem.Player.Disable();
        InputManager.Instance.InputSystem.FindAction("UI/Interact").Disable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        AudioManager.Instance.Pause();
        pauseMenuUI.SetActive(true);
        GUIManager.Instance.ShowHUD(false);
        Time.timeScale = 0f; // Dừng game
        isPaused = true;
        
    }

    public void Restart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        SceneManager.LoadSceneAsync(GameConstants.SCENE_GAMEPLAY_LV1);
        GUIManager.Instance.Reset();
        InventoryManager.Instance.Reset();
        PlayerController.Instance.ResetPlayer();
    }

    public void ReturnToMenu()
    { 
        Time.timeScale = 1f;
        AudioManager.Instance.StopSound();
        Instance = null;
        Destroy(GUIManager.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
        Destroy(CameraManager.Instance.gameObject);
        SceneManager.LoadScene(GameConstants.MAIN_MENU);
    }

    public void ShowGameOverPanel(bool isShow)
    {
        AudioManager.Instance.StopSound();
        gameOverUI.SetActive(isShow);
        Time.timeScale = 0f;
    }

    public void ShowSettingsPanel(bool isShow)
    {
        settingMenuUI.SetActive(isShow);
        pauseMenuUI.SetActive(!isShow);
    }
}
