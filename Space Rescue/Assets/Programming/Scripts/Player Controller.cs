using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] bool _isMoveAction;
    [SerializeField] Vector2 _currentMovementInput;

    [SerializeField] Rigidbody _rb;
    [SerializeField] private Transform _orientation;


    [SerializeField] Vector3 _movement;
    [SerializeField] Vector3 _currentMovement;

    [SerializeField] float _moveForce;

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
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        _currentMovement = (_orientation.forward * _currentMovementInput.y) + (_orientation.right * _currentMovementInput.x); // NORMALIZE MAYBE?

        SpeedControl();
    }

    private void FixedUpdate()
    {
        MoveMent();
    }

    void MoveMent()
    {
        _rb.AddForce(_currentMovement.normalized * _moveForce * 10f, ForceMode.Force);
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
}
