using UnityEngine;

public class JumscareSoundTrigger : MonoBehaviour
{
    public AudioClip sound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu đối tượng va chạm có tag "Player"
        {
            AudioManager.Instance.PlayEffect(sound);
            Destroy(gameObject);
        }
    }
}
