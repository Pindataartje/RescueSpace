using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Entity
{
    [Header("Setup")]

    [SerializeField] EnemySO _enemyInfo;
    [SerializeField] Animator _animator;
    [SerializeField] NavMeshAgent _agent;

    [Header("General")]
    public bool isAlive;
    [SerializeField] Transform _target;
    [SerializeField] float _distanceFromTarget;

    [Header("Patrol")]
    [SerializeField] float _distanceFromPatrol;

    [SerializeField] float _patrolRange;
    [SerializeField] int _currentPatrol;

    [SerializeField] int _patrolPointCount;
    [SerializeField] float _patrolRadius;

    [SerializeField] NavMeshHit _navmeshHit;
    [SerializeField] List<Transform> _patrolPoints = new();

    [Header("Search")]

    [SerializeField] float _searchTime;
    [SerializeField] float _timeToSearch;
    [SerializeField] int _searchCount;



    [SerializeField] State _currentState;

    public enum State
    {
        IDLE,
        PATROL,
        CHASE,
        SEARCH,
        ATTACK,
    }

    public virtual void ChangeState(State newState)
    {
        switch (_currentState)
        {
            case State.IDLE:
                StopIdle();
                break;
            case State.PATROL:
                StopPatrol();
                break;
            case State.CHASE:
                StopChase();
                break;
            case State.SEARCH:
                StopSearch();
                break;
            case State.ATTACK:
                StopAttack();
                break;
            default:
                Debug.Log($"{newState} is not supported on this robot");
                break;
        }

        switch (newState)
        {
            case State.IDLE:
                StartIdle();
                break;
            case State.PATROL:
                StartPatrol();
                break;
            case State.CHASE:
                StartChase();
                break;
            case State.SEARCH:
                StartSearch();
                break;
            case State.ATTACK:
                StartAttack();
                break;
            default:
                Debug.Log($"{newState} is not supported on this robot");
                break;
        }
    }

    public virtual void CheckState()
    {
        switch (_currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.PATROL:
                Patrol();
                break;
            case State.CHASE:
                Chase();
                break;
            case State.ATTACK:
                Attack();
                break;
        }
    }

    #region Idle

    public virtual void StartIdle()
    {
        _currentState = State.IDLE;
    }

    public virtual void Idle()
    {

    }

    public virtual void StopIdle()
    {

    }

    #endregion

    #region Patrol

    public virtual void StartPatrol()
    {
        _currentState = State.PATROL;

    }

    public virtual void Patrol()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);

            Vector3 targetWithOffset = new Vector3(_target.position.x, transform.position.y, _target.position.z);

            _distanceFromTarget = Vector3.Distance(transform.position, targetWithOffset);
        }

        if (_agent.stoppingDistance >= _distanceFromTarget && !_agent.isStopped)
        {
            _currentPatrol++;
            if (_currentPatrol >= _patrolPointCount)
            {
                _currentPatrol = 0;
            }
            _target = _patrolPoints[_currentPatrol];
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    public virtual void StopPatrol()
    {

    }

    #endregion

    #region Chase

    public virtual void StartChase()
    {
        _currentState = State.CHASE;
    }

    public virtual void Chase()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);

            Vector3 targetWithOffset = new Vector3(_target.position.x, transform.position.y, _target.position.z);

            _distanceFromTarget = Vector3.Distance(transform.position, targetWithOffset);

            Vector3 patrolWithOffset = new Vector3(_patrolPoints[_currentPatrol].position.x, transform.position.y, _patrolPoints[_currentPatrol].position.z);

            _distanceFromPatrol = Vector3.Distance(transform.position, patrolWithOffset);
        }

        if (_patrolRange <= _distanceFromPatrol)
        {
            ChangeState(State.PATROL);
        }

        if (_agent.stoppingDistance >= _distanceFromTarget)
        {
            ChangeState(State.ATTACK);
        }
    }

    public virtual void StopChase()
    {

    }

    #endregion

    #region Search

    public virtual void StartSearch()
    {
        _currentState = State.SEARCH;
    }

    public virtual void Search()
    {
        if (_searchTime < _timeToSearch)
        {
            _searchTime += Time.deltaTime;
        }
        else if (_searchTime >= _timeToSearch)
        {
            _searchCount++;

            // MOVE RANDOM DIRECTION AND THEN SEARCH AGAIN
        }
    }

    public virtual void StopSearch()
    {

    }

    #endregion

    #region Attack

    public virtual void StartAttack()
    {
        _currentState = State.ATTACK;

    }

    public virtual void Attack()
    {

    }

    public virtual void StopAttack()
    {

    }

    #endregion

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Start()
    {
        health = _enemyInfo.health;

        if (health > 0)
        {
            isAlive = true;
        }

        GeneratePatrol(_patrolPointCount);

        _target = _patrolPoints[0];
    }

    public override void Update()
    {
        CheckState();
    }

    public virtual void GeneratePatrol(int patrolPoints)
    {
        float angleStep = 360f / patrolPoints;
        float randomOffsetRadius = 2f;

        GameObject holdObj = new GameObject("Hold_Points");

        for (int i = 0; i < patrolPoints; i++)
        {
            bool validPositionFound = false;

            while (!validPositionFound)
            {
                float angle = i * angleStep;

                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _patrolRadius;
                float z = Mathf.Sin(angle * Mathf.Deg2Rad) * _patrolRadius;
                Vector3 basePosition = new Vector3(x, 0, z) + transform.position;

                Vector3 randomOffset = Random.insideUnitSphere * randomOffsetRadius;
                randomOffset.y = 0;
                Vector3 randomPosition = basePosition + randomOffset;

                if (NavMesh.SamplePosition(randomPosition, out _navmeshHit, _patrolRadius, NavMesh.AllAreas))
                {
                    validPositionFound = true;

                    GameObject patrolObj = new GameObject($"Hold_Pos ({i})");

                    patrolObj.transform.position = _navmeshHit.position;
                    patrolObj.transform.parent = transform;

                    patrolObj.transform.SetParent(holdObj.transform);

                    _patrolPoints.Add(patrolObj.transform);
                }
            }
        }
    }

    [SerializeField] float _gizmoSize;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _patrolPoints.Count; i++)
        {
            Gizmos.color = Color.yellow;

            foreach (Transform position in _patrolPoints)
            {
                if (position != null)
                {
                    Gizmos.DrawSphere(position.position, _gizmoSize);
                }
            }
        }
    }

    public virtual void EnterDetection(Transform newDetected)
    {
        _target = newDetected;

        ChangeState(State.CHASE);
    }

    public virtual void Detection()
    {

    }

    public virtual void ExitDetection()
    {
        ChangeState(State.SEARCH);
    }
}
