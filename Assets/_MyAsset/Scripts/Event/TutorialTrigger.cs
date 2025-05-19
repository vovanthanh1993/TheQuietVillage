using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private string type;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case "Run":
                    TutorialManager.Instance.ShowRunTutorial();
                    break;
                case "Weapon":
                    TutorialManager.Instance.ChangeWeaponTutorial();
                    break;
                default:
                    // code thực hiện nếu không khớp case nào
                    break;
            }
            Destroy(gameObject);
        }
    }
}
