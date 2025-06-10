using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("Input")]
    [SerializeField] private float smoothInputSpeed = 0.01f;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    #region Movement
    [Space]
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float walkBackSpeed = 2f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float runBackSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float crouchBackSpeed = 1f;
    #endregion

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float currentSpeed = 3f;
    private bool isWalking;
    private bool isSprinting;
    private bool isCrouching;

    #region Animator & Gravity
    [Space]
    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    [Space]
    [Header("Gravity")]
    [SerializeField] private float gravityMutiplayer = 3f;
    private float gravity = -9.81f;
    private float velocityY;
    #endregion

    #region Jump
    [Space]
    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    private float lastGroundY;
    public float minFallHeight = 0.0f;
    public bool isFalling = false;
    #endregion

    #region Look
    [Header("Look")]
    [SerializeField] private Transform centerSpinePos;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    private float xRotation;
    private float yRotation;
    #endregion

    #region Camera
    [Space]
    [Header("Camera")]
    [SerializeField] private Transform cameraHolder;
    private Camera playerCamera;
    #endregion

    public static PlayerController Instance { get; private set; }
    private PlayerHealth playerHealth;
    public  PlayerHealth PlayerHealth => playerHealth;

    private PlayerStamina stamina;
    public bool IsWalking { get; set; }
    public bool IsSprinting { get; set; }
    public bool IsCrouching { get; set; }
    
    public PlayerStamina Stamina { get; set; }

    private Vector3 previousPosition;
    private float horizontalSpeed;
    public float baseStepSpeed = 0.5f;
    public float sprintMultiplier = 0.6f;
    private float stepTimer;

    #region Camera Bobbing
    [Header("Camera Bobbing")]
    [SerializeField] private float bobFrequency = 8f;
    [SerializeField] private float bobAmplitude = 0.05f;
    private Vector3 cameraInitialLocalPos;
    private float bobTimer;
    private Vector2 crossHairInitialAnchoredPos;
    #endregion

    

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        cameraInitialLocalPos = cameraHolder.localPosition;
        RectTransform crossRect = GUIManager.Instance.crosshair.GetComponent<RectTransform>();
        crossHairInitialAnchoredPos = crossRect.anchoredPosition;
        previousPosition = transform.position;
    }

    private void HandleCameraBobbing()
    {
        RectTransform crossRect = GUIManager.Instance.crosshair.GetComponent<RectTransform>();

        if (characterController.isGrounded && horizontalSpeed > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;

            float bobOffsetY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.5f;

            Vector3 bobPosition = cameraInitialLocalPos + new Vector3(bobOffsetX, bobOffsetY, 0f);
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, bobPosition, Time.deltaTime * 5f);

            Vector2 bobAnchoredPos = crossHairInitialAnchoredPos + new Vector2(bobOffsetX * 50f, bobOffsetY * 50f);
            crossRect.anchoredPosition = Vector2.Lerp(crossRect.anchoredPosition, bobAnchoredPos, Time.deltaTime * 5f);
        }
        else
        {
            bobTimer = 0f;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, cameraInitialLocalPos, Time.deltaTime * 5f);
            crossRect.anchoredPosition = Vector2.Lerp(crossRect.anchoredPosition, crossHairInitialAnchoredPos, Time.deltaTime * 5f);
        }
    }

    public void HandleFootSteps()
    {
        if (characterController.isGrounded
            && horizontalSpeed > 0.1f
            && Mathf.Abs(characterController.velocity.y) < 0.1f)
        {
            float interval = baseStepSpeed;

            if (currentSpeed >= 4f)
            {
                interval *= sprintMultiplier;
            }

            interval = Mathf.Max(0.15f, interval);

            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                AudioManager.Instance.PlayFootSteep();
                stepTimer = interval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private void UpdateHorizontalSpeed()
    {
        Vector3 currentPosition = transform.position;
        Vector3 deltaPosition = currentPosition - previousPosition;
        Vector3 horizontalVelocity = new Vector3(deltaPosition.x, 0f, deltaPosition.z) / Time.deltaTime;

        horizontalSpeed = horizontalVelocity.magnitude;
        previousPosition = currentPosition;
    }

    private void Awake()
    {
        Cursor.visible = false;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (LoaderManager.instance.isLoading) return;
        Gravity();
        HanleSpeeds();
        HandleInputAndMove();
        Look();
        UpdateHorizontalSpeed();
        HandleFootSteps();
        HandleCameraBobbing();
    }

    private void Gravity()
    {
        if (characterController.isGrounded && velocityY < 0.0f)
        {
            velocityY = -1f;
        }
        else
        {
            velocityY += gravity * Time.deltaTime * gravityMutiplayer;
        }
    }

    /*private void Jump()
    {
        if (InputSystem.Player.Jump.IsPressed() && characterController.isGrounded) {
            playerAnimator.SetTrigger("IsJump");
            velocityY = jumpForce;
        }
    }*/

    IEnumerator JumpingBoolTime()
    {
        playerAnimator.SetBool("IsJumping", true);
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("IsJumping", false);
    }

    private void HanleAnimation()
    {
        playerAnimator.SetBool("IsRunning", IsSprinting);
        playerAnimator.SetBool("IsWalking", IsWalking);
        playerAnimator.SetBool("IsCrouching", IsCrouching);
        CheckGround();
        playerAnimator.SetBool("IsFalling", isFalling);

        playerAnimator.SetFloat("VelocityZ", currentInputVector.x);
        playerAnimator.SetFloat("VelocityY", currentInputVector.y);
    }

    private void CheckGround()
    {
        if (!characterController.isGrounded)
        {
            lastGroundY = transform.position.y;
            if ((lastGroundY - transform.position.y) > minFallHeight)
            {
                isFalling = true;
            }
        }
        else
        {
            isFalling = false;
        }
    }

    private void HanleSpeeds()
    {
        Vector2 inputVector = InputManager.Instance.InputMoveVector();

        bool sprintInput = InputManager.Instance.IsSprint();
        if (sprintInput)
        {
            IsSprinting = true;
            IsCrouching = false;
        }
        else
        {
            IsSprinting = false;
            IsCrouching = false;
        }

        if (!IsCrouching && !IsSprinting)
        {
            IsWalking = true;
        }
        else
        {
            IsWalking = false;
        }

        if (inputVector == Vector2.zero) IsSprinting = false;

        if (IsWalking) currentSpeed = walkSpeed;
        if (IsSprinting && inputVector.y > 0.0f) currentSpeed = runSpeed;
        else if (IsSprinting && inputVector.y < 0.0f) currentSpeed = runBackSpeed;
        if (IsWalking && inputVector.y > 0.0f) currentSpeed = walkSpeed;
        else if (IsWalking && inputVector.y < 0.0f) currentSpeed = walkBackSpeed;
        if (IsCrouching && inputVector.y > 0.0f) currentSpeed = crouchSpeed;
        else if (IsCrouching && inputVector.y < 0.0f) currentSpeed = crouchBackSpeed;
    }

    private void HandleInputAndMove()
    {
        Vector2 inputVector = InputManager.Instance.InputMoveVector();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputVector, ref smoothInputVelocity, smoothInputSpeed);

        moveDirection = (currentInputVector.y * transform.forward + currentInputVector.x * transform.right).normalized;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        characterController.Move(transform.up * velocityY * Time.deltaTime);
    }

    private void Look()
    {
        Vector2 lookVector = InputManager.Instance.InputLookVector();
        float mouseX = lookVector.x * Time.deltaTime * sensX * SettingsMenuManager.Instance.MouseHorizontal;
        float mouseY = lookVector.y * Time.deltaTime * sensY * SettingsMenuManager.Instance.MouseHorizontal;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        centerSpinePos.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void LateUpdate()
    {
        playerCamera.transform.position = cameraHolder.transform.position;
        playerCamera.transform.rotation = cameraHolder.transform.rotation;
    }

    public void GunShoot()
    {
        playerAnimator.SetTrigger("Shoot");
    }

    void OnEnable()
    {
       PlayerEvents.OnDie += Death;
    }

    void OnDisable()
    {
        PlayerEvents.OnDie -= Death;
    }

    private void Death()
    {
        this.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GUIManager.Instance.ShowHUD(false);
        StartCoroutine(DeathCameraEffect());
    }

    IEnumerator DeathCameraEffect()
    {
        float duration = 3f;
        float elapsed = 0f;

        Quaternion startRot = playerCamera.transform.rotation;
        Quaternion endRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(90, 0, 0));

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            playerCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / duration);
            yield return null;
        }
        PauseMenuManager.Instance.ShowGameOverPanel(true);
    }

    public void InitPlayer()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPos");
        transform.position = spawnPoint.transform.position;
    }
    public void ResetPlayer()
    {
        InitPlayer();
        gameObject.SetActive(true);
        this.enabled = true;
        playerHealth.ResetHealth();
        PlayerItem.Instance.ResetPlayerItems();
    }
}