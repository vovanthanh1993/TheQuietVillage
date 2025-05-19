using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject exitMenu;

    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        mainMenu.SetActive(false);
        SceneManager.LoadScene("MainStory");
        Destroy(GameObject.Find("SETTINGS MENU MANAGER"));
    }

    public void Settings()
    {
        AudioManager.Instance.ClickSound();
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BtnExitClick()
    {
        exitMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void BtnNoExitClick()
    {
        mainMenu.SetActive(true);
        exitMenu.SetActive(false);
    }
    public void QuitGame()
    {
        AudioManager.Instance.ClickSound();
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        AudioManager.Instance.ClickSound();
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}
