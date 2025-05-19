using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    [SerializeField] private int level;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GUIManager.Instance.ResetQuest();
            SceneLoadHandler.Instance.LoadSceneWithInit("GamePlayLv" + level);
        }
    }
}
