using UnityEngine;
using UnityEngine.UI;  // Required for UI Image manipulation

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float leanAngle = 15f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDecreaseRate = 5f;  // How fast stam decreases per second of running
    [SerializeField] private float staminaRegenRate = 2f;  // How fast stamina regenerates per second when not running
    [SerializeField] private Image screenOverlay; // Reference to the black screen overlay image
    [SerializeField] private float interactionDistance = 3f;  // The distance at which the player can interact
    [SerializeField] private LayerMask interactableLayer;     // LayerMask to filter interactable objects

    private float xRotation = 0f;
    private bool isCrouching = false;
    private float defaultCameraZRotation = 0f;
    private float currentStamina;
    private bool isSprinting = false;  // Tracks if the player is sprinting
    private float sprintTimer = 0f;    // Timer to track how long the player has been sprinting
    private bool canHoldBreath = true; // Track if player can hold breath

    public Transform playerCamera;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;  // Prevents unwanted physics rotation
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defaultCameraZRotation = playerCamera.localRotation.eulerAngles.z;
        currentStamina = maxStamina;

        if (screenOverlay != null)
        {
            screenOverlay.color = new Color(0, 0, 0, 0); // Set initial alpha to 0 (transparent)
        }
    }

    void Update()
    {
        // Movement related stuff
        HandleMovement();
        HandleCameraMovement();
        HandleCrouch();
        HandleLeaning();

        // Non-Movement stuff
        Heartrate();
        // Holdmybreath(); // COMMENTED OUT for now

        // Stamina regeneration (only if NOT sprinting and NOT holding breath)
        if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina); // Ensure it doesn't exceed max
        }
        Debug.Log($"Current Stamina: {currentStamina}");

        if (!canHoldBreath && currentStamina >= 75)
        {
            canHoldBreath = true;
        }

        // Adjust screen opacity based on stamina
        if (currentStamina < 65)
        {
            // Gradual opacity adjustment as stamina drops below 65, becomes fully opaque at -15
            float opacity = Mathf.InverseLerp(65f, -15f, currentStamina);  // Map stamina to opacity value
            screenOverlay.color = new Color(0, 0, 0, 1 - opacity); // Increase opacity as stamina decreases
        }
        else
        {
            // If stamina is above 65, ensure the screen overlay is transparent
            screenOverlay.color = new Color(0, 0, 0, 0);
        }

        // Interaction: Press 'F' to interact with objects
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    void HandleMovement()
    {
        float moveSpeed = walkSpeed;
        bool isMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > 0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0f;

        // Sprinting condition
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 65 && isMoving) // Lowered sprint threshold to 65
        {
            if (!isSprinting) // Ensure stamina is only decreased once per frame
            {
                isSprinting = true;
                sprintTimer = 0f; // Reset sprint timer for each sprint session
            }

            moveSpeed = sprintSpeed;
            sprintTimer += Time.deltaTime;
            DecreaseStamina(sprintTimer); // Decrease stamina while sprinting
        }
        else
        {
            isSprinting = false;
            sprintTimer = 0f;
        }

        if (isCrouching) moveSpeed = crouchSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
    }

    void HandleCameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, playerCamera.localRotation.eulerAngles.z);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            capsuleCollider.height = isCrouching ? crouchHeight : standHeight;
            transform.position += Vector3.down * (isCrouching ? 0.5f : -0.5f);
        }
    }

    void HandleLeaning()
    {
        float zRotation = defaultCameraZRotation;

        if (Input.GetKey(KeyCode.Q))
        {
            zRotation = -leanAngle;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            zRotation = leanAngle;
        }

        playerCamera.localRotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, zRotation);
    }

    void DecreaseStamina(float timeSpentRunning)
    {
        if (currentStamina > 0)
        {
            currentStamina -= staminaDecreaseRate * timeSpentRunning * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);  // Make sure stamina doesn't go below 0
        }
    }

    // COMMENTED OUT for now
    // void Holdmybreath()
    // {
    //     // Disable holding breath if the player doesn't have enough stamina to sprint
    //     if (currentStamina < 65) // You can adjust this threshold if needed
    //     {
    //         canHoldBreath = false;
    //     }

    //     if (Input.GetKey(KeyCode.V) && canHoldBreath) // Only allow if canHoldBreath is true
    //     {
    //         currentStamina -= staminaDecreaseRate * Time.deltaTime;
    //         currentStamina = Mathf.Max(currentStamina, 0f); // Prevent stamina from going negative

    //         if (currentStamina == 0)
    //         {
    //             canHoldBreath = false; // Disable holding breath when stamina hits 0
    //         }
    //     }
    // }

    void Heartrate()
    {
        float currentBPM = BPMManager.Instance.bpm;
        Debug.Log($"Heartrate: {currentBPM}");
        // Pulling from BPMManager
    }

    // Interaction methods
    void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, interactableLayer))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                hit.collider.GetComponent<InteractableObject>().Interact();
            }
        }
    }
}
