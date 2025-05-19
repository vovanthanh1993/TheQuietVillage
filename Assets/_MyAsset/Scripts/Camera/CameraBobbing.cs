using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Bobbing Settings")]
    public float bobFrequency = 8f;
    public float bobAmplitude = 0.05f;
    public float bobSpeedMultiplier = 1f;

    private float bobTimer = 0f;
    private Vector3 initialPos;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        initialPos = cameraTransform.localPosition;
    }

    void Update()
    {
        if (controller == null) return;

        Vector3 velocity = controller.velocity;
        Vector2 horizontalSpeed = new Vector2(velocity.x, velocity.z);

        if (horizontalSpeed.magnitude > 0.1f && controller.isGrounded)
        {
            bobTimer += Time.deltaTime * bobFrequency * (horizontalSpeed.magnitude * bobSpeedMultiplier);
            float bobOffsetY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.5f;

            cameraTransform.localPosition = initialPos + new Vector3(bobOffsetX, bobOffsetY, 0f);
        }
        else
        {
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, initialPos, Time.deltaTime * 5f);
        }
    }
}
