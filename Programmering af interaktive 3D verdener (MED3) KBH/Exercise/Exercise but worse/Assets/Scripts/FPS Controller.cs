using UnityEngine;


public class FPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    
    [Header("Audio")]
    public AudioSource footstepAudio;
    public AudioClip[] footstepSounds;
    public float footstepRate = 0.5f;
    
    // Components
    private CharacterController controller;
    private Camera playerCamera;
    
    // Movement
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    
    // Mouse Look
    private float mouseX, mouseY;
    private float xRotation = 0f;
    
    // Footsteps
    private float footstepTimer;
    
    // Input
    private Vector2 moveInput;
    private bool isRunning;
    private bool jumpInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        
        if (playerCamera == null)
        {
            // Create camera if none exists
            GameObject cameraGO = new GameObject("Player Camera");
            cameraGO.transform.SetParent(transform);
            cameraGO.transform.localPosition = new Vector3(0, 1.8f, 0);
            playerCamera = cameraGO.AddComponent<Camera>();
        }
        
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleInput();
        HandleMouseLook();
        HandleMovement();
        HandleFootsteps();
        HandleCursorToggle();
    }
    
    void HandleInput()
    {
        // Movement input
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        
        // Running
        isRunning = Input.GetKey(KeyCode.LeftShift);
        
        // Jumping
        jumpInput = Input.GetButtonDown("Jump");
    }
    
    void HandleMouseLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;
        
        // Get mouse input
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Rotate the player body left and right
        transform.Rotate(Vector3.up * mouseX);
        
        // Rotate the camera up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    void HandleMovement()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }
        
        // Calculate move direction
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        
        // Determine speed
        currentSpeed = isRunning ? runSpeed : walkSpeed;
        
        // Move the controller
        controller.Move(move * currentSpeed * Time.deltaTime);
        
        // Jumping
        if (jumpInput && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    void HandleFootsteps()
    {
        if (!isGrounded || moveInput.magnitude == 0) return;
        
        footstepTimer += Time.deltaTime;
        
        float currentFootstepRate = isRunning ? footstepRate * 0.6f : footstepRate;
        
        if (footstepTimer >= currentFootstepRate)
        {
            PlayFootstepSound();
            footstepTimer = 0f;
        }
    }
    
    void PlayFootstepSound()
    {
        if (footstepAudio == null || footstepSounds.Length == 0) return;
        
        AudioClip footstepClip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        footstepAudio.PlayOneShot(footstepClip);
    }
    
    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    
    // Public methods for other scripts to access
    public Camera GetPlayerCamera()
    {
        return playerCamera;
    }
    
    public bool IsMoving()
    {
        return moveInput.magnitude > 0 && isGrounded;
    }
    
    public bool IsRunning()
    {
        return isRunning && IsMoving();
    }
    
    public Vector3 GetMovementInput()
    {
        return new Vector3(moveInput.x, 0, moveInput.y);
    }
}