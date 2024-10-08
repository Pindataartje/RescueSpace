using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Cache the main camera for performance
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the camera is assigned
        if (mainCamera != null)
        {
            // Make the UI element face the camera
            transform.LookAt(mainCamera.transform);

            // Optional: Invert the rotation on the Y-axis to face it correctly
            transform.Rotate(0, 180, 0);
        }
    }
}