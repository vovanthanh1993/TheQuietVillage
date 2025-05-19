using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{

    [Header("Menu References")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject mainMenu;

    [Header("Volume Sliders")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundfxVolumeSlider;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject keyPanel;
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown screenResolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown gameDifficultyDropdown;



    [SerializeField] private Slider mouseHorizontalSlider;
    [SerializeField] private Slider mouseVerticalSlider;
    [SerializeField] private float mouseHorizontal;
    [SerializeField] private float mouseVertical;
    [SerializeField] private float damageMultiplierSetting;


    public static SettingsMenuManager Instance { get; private set; }
    public float MouseHorizontal { get => mouseHorizontal; set => mouseHorizontal = value; }
    public float MouseVertical { get => mouseVertical; set => mouseVertical = value; }
    public float DamageMultiplierSetting { get => damageMultiplierSetting; set => damageMultiplierSetting = value; }

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

    public void ShowGamePanel()
    {
        gamePanel.SetActive(true);
        controlPanel.SetActive(false);
        videoPanel.SetActive(false);
        keyPanel.SetActive(false);
    }
    public void ShowControlPanel()
    {
        gamePanel.SetActive(false);
        controlPanel.SetActive(true);
        videoPanel.SetActive(false);
        keyPanel.SetActive(false);
    }
    public void ShowVideoPanel()
    {
        gamePanel.SetActive(false);
        controlPanel.SetActive(false);
        videoPanel.SetActive(true);
        keyPanel.SetActive(false);
    }

    public void ShowKeyPanel()
    {
        gamePanel.SetActive(false);
        controlPanel.SetActive(false);
        videoPanel.SetActive(false);
        keyPanel.SetActive(true);
    }
    IEnumerator Start()
    {
        // Chờ tới khi AudioManager.Instance sẵn sàng
        while (AudioManager.Instance == null)
            yield return null;
        ShowGamePanel();
        LoadSettings();
    }

    private void LoadSettings()
    {
       
        // Load chế độ đã lưu, mặc định là Exclusive Fullscreen (index 0)
        int savedMode = PlayerPrefs.GetInt("ScreenMode", 0);
        displayModeDropdown.value = savedMode;
        displayModeDropdown.RefreshShownValue(); // Cập nhật hiển thị UI
        OnDisplayModeDropdownValueChanged(savedMode);
        displayModeDropdown.onValueChanged.AddListener(OnDisplayModeDropdownValueChanged);

        
        // Load lựa chọn đã lưu từ PlayerPrefs (nếu có), mặc định là 1920x1080 (index = 1)
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);
        screenResolutionDropdown.value = savedIndex;
        screenResolutionDropdown.RefreshShownValue(); // Cập nhật UI
        OnResolutionFromDropdownChange(savedIndex); // Áp dụng độ phân giải đã lưu
        screenResolutionDropdown.onValueChanged.AddListener(OnResolutionFromDropdownChange);

        // Load lựa chọn đã lưu, mặc định là 1 (Medium)
        int savedQuality = PlayerPrefs.GetInt("QualitySetting", 1);
        qualityDropdown.value = savedQuality;
        qualityDropdown.RefreshShownValue();
        QualitySettings.SetQualityLevel(savedQuality);
        qualityDropdown.onValueChanged.AddListener(OnQualityDropdownValueChanged);

        // Music setting
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicVolumeSlider.value = musicVolume;
            AudioManager.Instance.SetMusicVolume(musicVolume);
        }
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        // sound fx setting
        if (PlayerPrefs.HasKey("SoundFxVolume"))
        {
            float soundfxVolume = PlayerPrefs.GetFloat("SoundFxVolume");
            soundfxVolumeSlider.value = soundfxVolume;
            AudioManager.Instance.SetSoundFxVolume(soundfxVolume);
        }
        soundfxVolumeSlider.onValueChanged.AddListener(SetSoundFxVolume);

        // Mouse setting
        mouseHorizontal = PlayerPrefs.GetFloat("MouseHorizontal", 1);
        mouseHorizontalSlider.value = mouseHorizontal;
        mouseHorizontalSlider.onValueChanged.AddListener(SetMouseHorizontal);
        mouseVertical = PlayerPrefs.GetFloat("MouseVertical", 1);
        mouseVerticalSlider.value = mouseVertical;
        mouseVerticalSlider.onValueChanged.AddListener(SetMouseVertical);

        //Game difficulty
        int gameDifficulty = PlayerPrefs.GetInt("GameDifficulty", 1);
        gameDifficultyDropdown.value = gameDifficulty;
        gameDifficultyDropdown.RefreshShownValue();
        HandleDifficultyChange(gameDifficulty);
        gameDifficultyDropdown.onValueChanged.AddListener(HandleDifficultyChange);
    }

    public void HandleDifficultyChange(int index)
    {
        switch (index)
        {
            case 0: // easy
                damageMultiplierSetting = 0.5f;
                break;
            case 1: // normal
                damageMultiplierSetting = 1f;
                break;
            case 2: //hard
                damageMultiplierSetting = 2f;
                break;
        }
        PlayerPrefs.SetInt("GameDifficulty", index);
    }
    public void SetMouseHorizontal(float value)
    {
        mouseHorizontal = value;
        PlayerPrefs.SetFloat("MouseHorizontal", value);
    }

    public void SetMouseVertical(float value)
    {
        mouseVertical = value;
        PlayerPrefs.SetFloat("MouseVertical", mouseVertical);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSoundFxVolume(float volume)
    {
        AudioManager.Instance.SetSoundFxVolume(volume);
        PlayerPrefs.SetFloat("SoundFxVolume", volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnMainMenu() {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
        AudioManager.Instance.ClickSound();
    }

    // Video setting

    private void OnResolutionFromDropdownChange(int index)
    {
        switch (index)
        {
            case 0: // 1280x720
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
            case 1: // 1920x1080
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
            case 2: // 2560x1440
                Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
                break;
            case 3: // 3840x2160
                Screen.SetResolution(3840, 2160, Screen.fullScreenMode);
                break;
        }

        PlayerPrefs.SetInt("ResolutionIndex", index); // Lưu lại lựa chọn
    }
    private void OnDisplayModeDropdownValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        PlayerPrefs.SetInt("ScreenMode", index);
    }

    private void OnQualityDropdownValueChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualitySetting", index);
    }
}
