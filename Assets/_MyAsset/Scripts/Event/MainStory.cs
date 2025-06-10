using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
    private void OnEnable()
    {
        SceneLoadHandler.Instance.LoadSceneWithInit(GameConstants.SCENE_GAMEPLAY_LV1);
    }
}
