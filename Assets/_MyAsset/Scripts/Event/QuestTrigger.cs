using Unity.VisualScripting;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private int questId;
    void OnTriggerEnter(Collider other)
    {
       GUIManager.Instance.ShowQuest(questId);
        Destroy(gameObject);
    }
}
