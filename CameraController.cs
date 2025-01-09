using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    public float xSensitivity = 60f;
    public float ySensitivity = 60f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // Corrected the property name to 'visible'
        Cursor.visible = false;
    }

    public void ProcessLook(Vector2 Input)
    {
        float mouseX = Input.x;
        float mouseY = Input.y;
        // calculate cam rotation for looking up or down
        xRotation -= (mouseY * Time.deltaTime * ySensitivity);
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //rotate player to look left or right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
