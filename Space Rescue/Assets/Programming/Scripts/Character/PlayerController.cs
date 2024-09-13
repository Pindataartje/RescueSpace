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

    [SerializeField] bool _isRecal;

    [SerializeField] LayerMask _throwLayer;
    [SerializeField] RaycastHit _throwHit;

    [SerializeField] GameObject _mouseReticle;

    [SerializeField] Transform _throwSpot;
    public Transform restSpot;

    [SerializeField] List<GameObject> _robots = new();
    public List<GameObject> Robots
    {
        get { return _robots; }
    }

    [SerializeField] int _robotTypeIndex;

    [SerializeField] GameObject _currentRobot;

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
        playerInput.actions.FindAction("Throw").canceled += PerformThrow;

        playerInput.actions.FindAction("Recal").started += OnRecal;
        playerInput.actions.FindAction("Recal").performed += OnRecal;
        playerInput.actions.FindAction("Recal").canceled += OnRecal;
    }

    private void OnDisable()
    {
        playerInput.actions.FindAction("Move").started -= OnMovement;
        playerInput.actions.FindAction("Move").performed -= OnMovement;
        playerInput.actions.FindAction("Move").canceled -= OnMovement;

        playerInput.actions.FindAction("MouseMove").started -= OnMouseMovement;
        playerInput.actions.FindAction("MouseMove").canceled -= OnMouseMovement;

        playerInput.actions.FindAction("Throw").started -= StartThrow;
        playerInput.actions.FindAction("Throw").canceled -= PerformThrow;

        playerInput.actions.FindAction("Recal").started -= OnRecal;
        playerInput.actions.FindAction("Recal").performed -= OnRecal;
        playerInput.actions.FindAction("Recal").canceled -= OnRecal;
    }

    private void Update()
    {
        _currentMovement = (_orientation.forward * _currentMovementInput.y) + (_orientation.right * _currentMovementInput.x);

        SpeedControl();

        if (_isMoveAction && !_isThrow)
        {
            Quaternion lookRotation = Quaternion.LookRotation(_currentMovement, Vector3.up);
            _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, lookRotation, Time.deltaTime * _playerRotationSpeed);
        }

        if (_isThrow)
        {
            HoldThrow();
        }

        HandleMouseReticle();

        if (_isRecal) // can be better optimized to only happen on button press and not always in the update
        {
            _mouseReticle.GetComponent<TestRecal>().canCheck = true;
        }
        else
        {
            _mouseReticle.GetComponent<TestRecal>().canCheck = false;
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

    public void HandleMouseReticle()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out _throwHit, Mathf.Infinity, _throwLayer))
        {
            _mouseReticle.transform.position = _throwHit.point;
        }
    }

    public void SimulatedArcThrow(Vector3 start, Vector3 target, GameObject robot, float duration)
    {
        robot.GetComponent<NavMeshAgent>().enabled = false;

        StartCoroutine(ArcMovement(start, target, robot, duration));
    }

    private IEnumerator ArcMovement(Vector3 start, Vector3 target, GameObject robot, float duration)
    {
        float timeElapsed = 0f;
        Vector3 peak = (start + target) / 2f + Vector3.up * 5f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float t = timeElapsed / duration;
            Vector3 currentPos = CalculateParabolicPosition(start, peak, target, t);

            robot.transform.position = currentPos;

            yield return null;
        }

        // MAYBE MAKE IT NOT GO INTO THE GROUND

        robot.GetComponent<NavMeshAgent>().enabled = true;

        robot.GetComponent<RobotAI>().ChangeState(RobotAI.State.ATTACK);
    }

    private Vector3 CalculateParabolicPosition(Vector3 start, Vector3 peak, Vector3 end, float t)
    {
        Vector3 a = Vector3.Lerp(start, peak, t);
        Vector3 b = Vector3.Lerp(peak, end, t);
        return Vector3.Lerp(a, b, t);
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
        if (_robots.Count > 0)
        {
            _currentRobot = _robots[0];
            _robots.RemoveAt(0);
        }

        if (_currentRobot != null)
        {
            _isThrow = true;

            _currentRobot.GetComponent<RobotAI>().ChangeState(RobotAI.State.IDLE);
            _currentRobot.GetComponent<NavMeshAgent>().enabled = false;
            _currentRobot.GetComponent<Collider>().isTrigger = true;
        }

    }

    public void AddRobot(GameObject newRobot)
    {
        _robots.Add(newRobot);
    }

    void HoldThrow() // maybe change robot state
    {
        _currentRobot.transform.position = _throwSpot.position;

        Quaternion lookRotation = Quaternion.LookRotation(_throwHit.point, Vector3.up);
        _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, lookRotation, Time.deltaTime * _playerRotationSpeed);
    }

    void PerformThrow(InputAction.CallbackContext context)
    {
        _mouseReticle.SetActive(true);

        if (_isThrow)
        {
            _isThrow = false;

            SimulatedArcThrow(_throwSpot.position, _throwHit.point, _currentRobot, 0.75f);
            _currentRobot = null;
        }
        else
        {
            _currentRobot = null;

            Debug.Log("nah");
        }
    }

    void CancleThrow() // put down robot instead of throwing
    {
        _currentRobot = null;

        _currentRobot.GetComponent<RobotAI>().ChangeState(RobotAI.State.FOLLOW);
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

    private void OnRecal(InputAction.CallbackContext context)
    {
        _isRecal = context.ReadValueAsButton();
    }
}
