using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform headPositionTarget; // Empty GameObject in the character's head bone for camera position

    private float xRotation = 0f; // To store the camera's X rotation (for vertical rotation)
    private float yRotation = 0f; // To store the camera's Y rotation (for sideways rotation)
    private readonly float bodyRotationThreshold = 30f; // Threshold for when the body starts rotating

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Apply the rotation for looking up and down (clamped)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotation

        // Apply the rotation to the camera on the X-axis (up and down) and the Y-Axis (right and left)
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Update the Y-axis rotation for looking sideways
        yRotation += mouseX;

        // Clamp camera sideways rotation between -30 and 30 degrees
        if (Mathf.Abs(yRotation) > bodyRotationThreshold)
        {
            // Rotate the player body once the camera's sideways rotation exceeds the threshold
            float bodyRotation = yRotation - (Mathf.Sign(yRotation) * bodyRotationThreshold);
            playerBody.Rotate(Vector3.up * bodyRotation);

            // Reset yRotation to threshold value after rotating the body
            yRotation = Mathf.Sign(yRotation) * bodyRotationThreshold;
        }

        // Note: yRotation is now clamped within the threshold, but when you rotate past 30 degrees, 
        // the body rotates and yRotation resets to stay within that limit.

        // Update the camera's position to follow the empty GameObject in the head bone
        if (headPositionTarget != null)
        {
            transform.position = headPositionTarget.position;
        }
        else
        {
            Debug.LogError("Head position target is not set!");
        }
    }
}
