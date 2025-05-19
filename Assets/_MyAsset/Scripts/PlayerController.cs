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
    [SerializeField]  private float smoothInputSpeed = 0.01f;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    // Movement
    #region
    [Space]
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float walkBackSpeed = 2f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float runBackSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2;
    [SerializeField] private float crouchBackSpeed = 1f;
    #endregion

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float currentSpeed = 3;
    private bool isWalking;
    private bool isSprinting;
    private bool isCrouching;

    #region
    [Space]
    [Header("Animator")]
    [SerializeField]  private Animator playerAnimator;

    [Space]
    [Header("Gravity")]
    [SerializeField] private float gravityMutiplayer = 3f;
    private float gravity = -9.81f;
    private float velocityY;
    #endregion

    // Jump
    #region
    [Space]
    [Header ("Jump")]
    [SerializeField] private float jumpForce = 5f;
    private float lastGroundY;
    public float minFallHeight = 0.0f;
    public bool isFalling = false;
    #endregion
    //Look
    #region
    [Header("Look")]
    [SerializeField] private Transform centerSpinePos;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    private float xRotation;
    private float yRotation;
    #endregion

    #region
    [Space]
    [Header("Camera")]
    [SerializeField] private Transform cameraHolder;
    private Camera playerCamera;
    #endregion

    public static PlayerController Instance { get; private set; }

    private Vector3 previousPosition;
    private float horizontalSpeed;
    public float baseStepSpeed = 0.5f;        // thời gian giữa các bước khi đi bộ
    public float sprintMultiplier = 0.6f;     // giảm thời gian khi chạy
    private float stepTimer;

    [Header("Camera Bobbing")]
    [SerializeField] private float bobFrequency = 8f;
    [SerializeField] private float bobAmplitude = 0.05f;
    private Vector3 cameraInitialLocalPos;
    private float bobTimer;
    private Vector2 crossHairInitialAnchoredPos;
    void Start()
    {
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

            Vector2 bobAnchoredPos = crossHairInitialAnchoredPos + new Vector2(bobOffsetX * 50f, bobOffsetY * 50f); // nhân to lên cho dễ thấy
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

            interval = Mathf.Max(0.15f, interval); // giới hạn tối thiểu

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
        // Nếu đã có một instance khác, thì hủy bản mới
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Nếu muốn player không bị hủy khi load scene mới
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // Update is called once per frame
    void Update()
    {
        if (LoaderManager.instance.isLoading) return;
        Gravity();
        HanleSpeeds();
        HandleInputAndMove();
        //Jump();
        //HanleAnimation();
        Look();
        UpdateHorizontalSpeed();   // ➕ thêm dòng này
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
        playerAnimator.SetBool("IsRunning", isSprinting);
        playerAnimator.SetBool("IsWalking", isWalking);
        playerAnimator.SetBool("IsCrouching", isCrouching);
        CheckGround();
        playerAnimator.SetBool("IsFalling", isFalling);

        playerAnimator.SetFloat("VelocityZ", currentInputVector.x);
        playerAnimator.SetFloat("VelocityY", currentInputVector.y);
    }

    private void CheckGround()
    {
        if(!characterController.isGrounded)
        {
            lastGroundY = transform.position.y;
            if((lastGroundY - transform.position.y) > minFallHeight)
            {
                isFalling = true;
            }
        } else { isFalling = false;};
    }
    private void HanleSpeeds()
    {
        Vector2 inputVector = InputManager.Instance.InputMoveVector();

        // Set the status
        bool sprintInput = InputManager.Instance.IsSprint();
        // Nếu người chơi giữ nút chạy và còn đủ stamina, và được phép chạy
        if (sprintInput && PlayerHealth.Instance.CanSprint())
        {
            isSprinting = true;
            isCrouching = false;
            PlayerHealth.Instance.DrainStamina(); // Trừ stamina
        }
        else
        {
            isSprinting = false;
            isCrouching = false;
        }

        // Nếu không chạy, thì hồi stamina
        if (!isSprinting)
        {
            PlayerHealth.Instance.RegenStamina();
        }

        if (!isCrouching && !isSprinting)
        {
            isWalking = true;
        } else isWalking = false;

        if(inputVector == Vector2.zero) isSprinting = false;

        //Handle the speed
        if (isWalking) currentSpeed = walkSpeed;
        if(isSprinting && inputVector.y > 0.0f) currentSpeed = runSpeed;
        else if(isSprinting && inputVector.y < 0.0f) currentSpeed = runBackSpeed;
        if (isWalking && inputVector.y > 0.0f) currentSpeed = walkSpeed;
        else if (isWalking && inputVector.y < 0.0f) currentSpeed = walkBackSpeed;
        if (isCrouching && inputVector.y > 0.0f) currentSpeed = crouchSpeed;
        else if (isCrouching && inputVector.y < 0.0f) currentSpeed = crouchBackSpeed;
    }

    private void HandleInputAndMove()
    {
        // Input
        Vector2 inputVector = InputManager.Instance.InputMoveVector();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputVector, ref smoothInputVelocity, smoothInputSpeed);

        // Move
        moveDirection = (currentInputVector.y * transform.forward + currentInputVector.x * transform.right).normalized;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        characterController.Move(transform.up * velocityY *Time.deltaTime);
    }

    private void Look()
    {
        Vector2 lookVector = InputManager.Instance.InputLookVector();
        float mouseX = lookVector.x * Time.deltaTime *sensX*SettingsMenuManager.Instance.MouseHorizontal;
        float mouseY = lookVector.y * Time.deltaTime * sensY * SettingsMenuManager.Instance.MouseHorizontal;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        // Setting value to the object
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
}
