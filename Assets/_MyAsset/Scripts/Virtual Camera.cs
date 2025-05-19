using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class VirtualCamera: MonoBehaviour
{
    void Start()
    {
        InputManager.Instance.InputSystem.Disable();
        PlayerManager.Instance.player.gameObject.SetActive(false);
        StartCoroutine(DestroyAfterDelay(2f));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        PlayerManager.Instance.player.gameObject.SetActive(true);
        InputManager.Instance.InputSystem.Enable();
        PlayerManager.Instance.InitPlayer();
        TutorialManager.Instance.ShowUseFlashLight();
    }
}
