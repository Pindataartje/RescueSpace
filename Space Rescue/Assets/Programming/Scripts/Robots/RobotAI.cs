using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
{
    [SerializeField] Robot _robotInfo;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] Transform _target;

    [SerializeField] Transform _player;

    [SerializeField] Collider _collider;
    [SerializeField] Rigidbody _rb;


    [SerializeField] bool _isAttacking;

    [SerializeField] float _distanceFromTarget;

    private void Start()
    {
        if (_robotInfo != null)
        {
            _agent.speed = _robotInfo.speed;
        }
    }

    private void Update()
    {
        CheckState(_currentState);

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (_currentState == State.FOLLOW)
            {
                ChangeState(State.ATTACK);
            }
            else
            {
                ChangeState(State.FOLLOW);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeState(State.IDLE);
        }

    }

    public State _currentState;

    public enum State
    {
        IDLE,
        FOLLOW,
        ATTACK,
        ATTACHED,
    }

    public void ChangeState(State newState)
    {
        switch (_currentState)
        {
            case State.IDLE:
                StopIdle();
                break;
            case State.FOLLOW:
                StopFollow();
                break;
            case State.ATTACK:
                StopAttack();
                break;
            case State.ATTACHED:
                StopAttached();
                break;
        }

        switch (newState)
        {
            case State.IDLE:
                StartIdle();
                break;
            case State.FOLLOW:
                StartFollow();
                break;
            case State.ATTACK:
                StartAttack();
                break;
            case State.ATTACHED:
                StartAttached();
                break;
        }
    }

    void CheckState(State state)
    {
        switch (state)
        {
            case State.IDLE:
                Idle();
                break;
            case State.FOLLOW:
                Follow();
                break;
            case State.ATTACK:
                Attack();
                break;
            case State.ATTACHED:
                Attached();
                break;
        }
    }

    #region Idle

    void StartIdle()
    {
        _currentState = State.IDLE;
    }

    void Idle()
    {

    }

    void StopIdle()
    {

    }

    #endregion

    #region Follow

    void StartFollow()
    {
        _currentState = State.FOLLOW;

        _target = _player;

        _agent.stoppingDistance = _robotInfo.followDistance;
    }

    void Follow()
    {
        _agent.SetDestination(_target.position);

        _distanceFromTarget = Vector3.Distance(transform.position, _target.position);

        // if (agent.stoppingDistance > _distanceFromTarget)
        // {
        //     agent.isStopped = true;
        // }
        // else
        // {
        //     agent.isStopped = false;
        // }
    }

    void StopFollow()
    {

    }

    #endregion

    #region Attack

    void StartAttack()
    {
        _currentState = State.ATTACK;

        _agent.stoppingDistance = _robotInfo.range;

        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    void Attack()
    {
        if (_agent.isActiveAndEnabled)
        {
            _agent.SetDestination(GameObject.FindGameObjectWithTag("Enemy").transform.position);
        }

        _distanceFromTarget = Vector3.Distance(transform.position, _target.position);

        if (_agent.stoppingDistance > _distanceFromTarget && !_isAttacking)
        {
            if (_agent.isActiveAndEnabled)
            {
                _agent.isStopped = true;
            }

            StartCoroutine(StartAttacking());
        }
        else
        {
            if (_agent.isActiveAndEnabled)
            {
                _agent.isStopped = false;
            }
        }
    }

    void StopAttack()
    {
        StartCoroutine(StopAttacking());
    }

    #endregion

    #region Attached

    void StartAttached()
    {
        _currentState = State.ATTACHED;

        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;

        transform.SetParent(_target.transform, true);
    }

    void Attached()
    {

    }

    void StopAttached()
    {
        transform.SetParent(null);

        _collider.isTrigger = false;
        _rb.useGravity = true;

        _agent.enabled = true; // maybe when hits the floor
    }

    #endregion

    public IEnumerator StartAttacking()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(_robotInfo.windUpTime);

        StartCoroutine(Attacking());
    }

    public IEnumerator Attacking()
    {
        _target.GetComponent<Entity>().TakeDamage(_robotInfo.damage);

        yield return new WaitForSeconds(_robotInfo.fireRate);

        if (_isAttacking)
        {
            StartCoroutine(Attacking());
        }
    }

    public IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(_robotInfo.windDownTime);
        _isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit enemy");
            other.gameObject.GetComponent<Entity>().TakeDamage(_robotInfo.impactDamage);

            _target = other.transform;
            ChangeState(State.ATTACHED);
        }
    }
}
