using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    [SerializeField] public GameObject useFlashLight;
    [SerializeField] public GameObject changeWeaponTutorial;
    [SerializeField] public GameObject notes;
    [SerializeField] public GameObject runTutorial;
    [SerializeField] public GameObject inventoryTutorial;

    void Awake()
    {
        // Nếu đã có instance khác, hủy đối tượng này
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Gán instance
        Instance = this;
    }

    public void ShowUseFlashLight()
    {
        if (useFlashLight != null)
            StartCoroutine(ShowAndDestroy(useFlashLight));
    }

    public void ShowInventoryTutorial()
    {
        if (inventoryTutorial != null)
            StartCoroutine(ShowAndDestroy(inventoryTutorial));
    }

    public void ShowRunTutorial()
    {
        if (runTutorial != null)
            StartCoroutine(ShowAndDestroy(runTutorial));
    }

    public void ChangeWeaponTutorial()
    {
        StartCoroutine(ShowAndDestroy(changeWeaponTutorial));
    }

    public void ShowOpenNotes()
    {
        StartCoroutine(ShowAndDestroy(notes));
    }

    private IEnumerator ShowAndDestroy(GameObject tutorial)
    {
        tutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        tutorial.SetActive(false);
    }

    private void Start()
    {

    }
}
