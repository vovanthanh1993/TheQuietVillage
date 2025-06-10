using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class VirtualCamera: MonoBehaviour
{
    void Start()
    {
        InputManager.Instance.InputSystem.Disable();
        PlayerController.Instance.gameObject.SetActive(false);
        StartCoroutine(DestroyAfterDelay(2f));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        PlayerController.Instance.gameObject.SetActive(true);
        InputManager.Instance.InputSystem.Enable();
        PlayerController.Instance.InitPlayer();
        TutorialManager.Instance.ShowUseFlashLight();
    }
}
