using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float leanAngle = 15f;
    private Vector3 originalPosition;


    private float xRotation = 0f;
    private float defaultCameraZRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor for better control
        Cursor.visible = false;
        defaultCameraZRotation = transform.localRotation.eulerAngles.z;
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        HandleCameraMovement();
        HandleLeaning();
    }

    void HandleCameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (pitch) - only affect the camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent flipping

        transform.localRotation = Quaternion.Euler(xRotation, 0f, transform.localRotation.eulerAngles.z);

        // Horizontal rotation (yaw) - rotate the entire player body
        playerBody.Rotate(Vector3.up * mouseX);
    }

void HandleLeaning()
{
    float zRotation = defaultCameraZRotation;
    Vector3 targetPosition = originalPosition; // Ensure originalPosition is stored

    if (Input.GetKey(KeyCode.E))
    {
        zRotation = -leanAngle;
        targetPosition += Vector3.right * 0.5f; // Move right
    }
    else if (Input.GetKey(KeyCode.Q))
    {
        zRotation = leanAngle;
        targetPosition += Vector3.left * 0.5f; // Move left
    }

    // Smoothly interpolate position and rotation
    transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);
    transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 10f);
}

}
