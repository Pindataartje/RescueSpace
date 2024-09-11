using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] bool _isMoveAction;
    [SerializeField] Vector2 _currentMovementInput;
    [SerializeField] Vector3 _currentMovement;

    [SerializeField] bool _isMouseMovement;

    [SerializeField] bool _isThrow;

    [SerializeField] LayerMask _throwLayer;
    [SerializeField] RaycastHit _throwHit;

    [SerializeField] GameObject _throwHitArea;
    [SerializeField] Transform _throwSpot;

    [SerializeField] List<GameObject> _robots = new();

    [SerializeField] GameObject _robotToThrow;

    [SerializeField] LayerMask _walkLayerMask;

    [SerializeField] Rigidbody _rb;
    [SerializeField] Transform _playerModel;
    [SerializeField] Transform _orientation;
    [SerializeField] Camera _camera;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] float _moveForce;

    [SerializeField] float _playerRotationSpeed;

    private void Awake()
    {
        playerInput.actions.FindActionMap("Game").Enable();
        playerInput.actions.FindActionMap("Menu").Enable();
    }

    private void OnEnable()
    {
        playerInput.actions.FindAction("Move").started += OnMovement;
        playerInput.actions.FindAction("Move").performed += OnMovement;
        playerInput.actions.FindAction("Move").canceled += OnMovement;

        playerInput.actions.FindAction("MouseMove").started += OnMouseMovement;
        playerInput.actions.FindAction("MouseMove").canceled += OnMouseMovement;

        playerInput.actions.FindAction("Throw").started += StartThrow;
        // playerInput.actions.FindAction("Throw").performed += UpdateThrow;
        playerInput.actions.FindAction("Throw").canceled += StopThrow;
    }

    private void OnDisable()
    {
        playerInput.actions.FindAction("Move").started -= OnMovement;
        playerInput.actions.FindAction("Move").performed -= OnMovement;
        playerInput.actions.FindAction("Move").canceled -= OnMovement;

        playerInput.actions.FindAction("MouseMove").started -= OnMouseMovement;
        playerInput.actions.FindAction("MouseMove").canceled -= OnMouseMovement;

        playerInput.actions.FindAction("Throw").started -= StartThrow;
        // playerInput.actions.FindAction("Throw").performed -= UpdateThrow;
        playerInput.actions.FindAction("Throw").canceled -= StopThrow;
    }

    private void Update()
    {
        _currentMovement = (_orientation.forward * _currentMovementInput.y) + (_orientation.right * _currentMovementInput.x); // NORMALIZE MAYBE?

        SpeedControl();

        if (_isMoveAction && !_isThrow)
        {
            Quaternion lookRotation = Quaternion.LookRotation(_currentMovement, Vector3.up);
            _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, lookRotation, Time.deltaTime * _playerRotationSpeed);
        }

        if (_isThrow)
        {
            UpdateThrow();
        }
        else
        {
            _throwHitArea.SetActive(false);
        }

        // if (_isMouseMovement) // Set Destanation to null if reached
        // {
        //     _isMouseMovement = false;

        //     Vector3 mousePos = Mouse.current.position.ReadValue();

        //     Ray ray = Camera.main.ScreenPointToRay(mousePos);

        //     if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _walkLayerMask))
        //     {
        //         Debug.Log($"Ray hit the ground at: {hit.point}");

        //         _agent.SetDestination(hit.point);
        //     }
        // }
    }

    public void ArcThrow(Vector3 start, Vector3 target)
    {
        Vector3 direction = (target - start);
        float distance = direction.magnitude;
        direction.Normalize();

        float angle = 45f * Mathf.Deg2Rad;
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * angle));

        Vector3 horizontalVelocity = direction * velocity * Mathf.Cos(angle);
        Vector3 verticalVelocity = Vector3.up * velocity * Mathf.Sin(angle);

        _robotToThrow.GetComponent<Rigidbody>().velocity = horizontalVelocity + verticalVelocity;
        _robotToThrow.GetComponent<Rigidbody>().useGravity = true;
    }

    private void FixedUpdate()
    {
        MoveMent();
    }

    void MoveMent()
    {
        _rb.AddForce(_currentMovement.normalized * _moveForce * 10f, ForceMode.Force);
    }

    void StartThrow(InputAction.CallbackContext context)
    {
        _robotToThrow = _robots[0]; // FIX EMPTY

        _robots.RemoveAt(0);

        if (_robotToThrow != null)
        {
            _isThrow = true;

            _robotToThrow.GetComponent<RobotAI>().ChangeState(RobotAI.State.IDLE);
            _robotToThrow.GetComponent<NavMeshAgent>().enabled = false;
            _robotToThrow.GetComponent<Collider>().isTrigger = true;
        }

    }

    void UpdateThrow()
    {
        _throwHitArea.SetActive(true);

        _robotToThrow.transform.position = _throwSpot.position;

        Vector3 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out _throwHit, Mathf.Infinity, _throwLayer))
        {
            Debug.Log($"Ray hit the ground at: {_throwHit.point}");
            _throwHitArea.transform.position = _throwHit.point;

            Quaternion lookRotation = Quaternion.LookRotation(_throwHit.point, Vector3.up);
            _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, lookRotation, Time.deltaTime * _playerRotationSpeed);
        }
    }

    void StopThrow(InputAction.CallbackContext context)
    {
        if (_isThrow)
        {
            _isThrow = false;

            ArcThrow(_throwSpot.position, _throwHit.point);
        }
        else
        {
            Debug.Log("nah");
        }
    }

    void CancleThrow() // put down robot instead of throwing
    {

    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        if (flatVel.magnitude > _moveForce)
        {
            Vector3 limitedVel = flatVel.normalized * _moveForce;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>().normalized;
        _isMoveAction = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    private void OnMouseMovement(InputAction.CallbackContext context)
    {
        _isMouseMovement = context.ReadValueAsButton();
    }
}