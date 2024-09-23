using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] PlayerController playerController;

    Vector2 _isCamera;

    [SerializeField] Transform _target;
    [SerializeField] Transform _orientation;

    [SerializeField] float distanceFromPlayer = 5.0f; // Camera distance from the player
    [SerializeField] float rotationSpeed = 5.0f; // Speed at which the camera rotates

    [SerializeField] float _playerRotationSpeed;

    private float currentX = 0.0f; // X-axis rotation
    private float currentY = 0.0f; // Y-axis rotation
    public float minYAngle = 60f;
    public float maxYAngle = 60f;

    private void OnEnable()
    {
        playerInput.actions.FindAction("Camera").started += OnCamera;
        playerInput.actions.FindAction("Camera").performed += OnCamera;
        playerInput.actions.FindAction("Camera").canceled += OnCamera;
    }

    private void OnDisable()
    {
        playerInput.actions.FindAction("Camera").started -= OnCamera;
        playerInput.actions.FindAction("Camera").performed -= OnCamera;
        playerInput.actions.FindAction("Camera").canceled -= OnCamera;
    }

    void OnCamera(InputAction.CallbackContext context)
    {
        _isCamera = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        currentX += _isCamera.x * rotationSpeed * 10 * Time.deltaTime;
        currentY -= _isCamera.y * rotationSpeed * 10 * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        Vector3 cameraForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        _orientation.forward = cameraForward;
    }

    private void LateUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -distanceFromPlayer);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = _target.position + rotation * direction;
        transform.LookAt(_target.position);
    }
}