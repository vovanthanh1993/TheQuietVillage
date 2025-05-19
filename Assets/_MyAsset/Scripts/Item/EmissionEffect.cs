using UnityEngine;

public class EmissionEffect : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private Material itemMaterial; // Vật liệu của item
    [SerializeField] private Color emissionColor = Color.yellow; // Màu phát sáng
    [SerializeField] private float flashSpeed = 5f; // Tốc độ chớp sáng

    void Update()
    {
        float intensity = Mathf.PingPong(Time.time * flashSpeed, 1); // Giá trị sáng tối dao động từ 0 đến 1
        itemMaterial.SetColor("_EmissionColor", emissionColor * intensity*0.5f);
    }
}