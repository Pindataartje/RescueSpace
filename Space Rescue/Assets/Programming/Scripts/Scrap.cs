using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scrap : MonoBehaviour
{
    [SerializeField] Transform _objectToCarry;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] int _numberOfRobotsNeeded;
    [SerializeField] int _maxRobotAmount;

    [SerializeField] int _robotsCarrying;
    [SerializeField] int _robotsAttempting;

    [SerializeField] float _extraRobotPercent;

    [SerializeField] float _radius;

    [SerializeField] Transform _player;

    [SerializeField] Transform _target;

    [SerializeField] List<RobotAI> _robots = new();

    [SerializeField] List<Transform> _robotPositionTransforms = new List<Transform>();

    [SerializeField] float _distanceFromTarget;

    [SerializeField] int _scrapWorth;

    [SerializeField] ScrapRobot _scrapRobot;

    private void Start()
    {
        GeneratePositionTransforms();
    }

    private void Update()
    {
        if (_robotsCarrying >= _numberOfRobotsNeeded)
        {
            if (_agent.isActiveAndEnabled)
            {
                _agent.SetDestination(_target.position);

                _agent.isStopped = false;
            }

            Vector3 targetWithOffset = new Vector3(_target.position.x, transform.position.y, _target.position.z);

            _distanceFromTarget = Vector3.Distance(transform.position, targetWithOffset);

            if (_agent.stoppingDistance >= _distanceFromTarget && _agent.isActiveAndEnabled && !_agent.isStopped)
            {
                for (int i = 0; i < _robots.Count; i++)
                {
                    _robots[i].CollectScrapAtBase();
                }

                _robots.Clear();

                _robotsCarrying = 0;

                _scrapRobot.CollectScrap(_scrapWorth);

                Destroy(gameObject);
            }
        }
        else if (_robotsCarrying < _numberOfRobotsNeeded && _agent.isActiveAndEnabled && !_agent.isStopped)
        {
            _agent.isStopped = true;
        }
    }

    private void GeneratePositionTransforms()
    {
        _maxRobotAmount = Mathf.CeilToInt(_numberOfRobotsNeeded * _extraRobotPercent);

        float angleStep = 360f / _maxRobotAmount;

        for (int i = 0; i < _maxRobotAmount; i++)
        {
            float angle = i * angleStep;
            float angleRad = Mathf.Deg2Rad * angle;

            Vector3 position = new Vector3(_objectToCarry.position.x + Mathf.Cos(angleRad) * _radius, _objectToCarry.position.y, _objectToCarry.position.z + Mathf.Sin(angleRad) * _radius);

            GameObject positionObject = new GameObject("Position_" + i);

            positionObject.transform.position = position;
            positionObject.transform.parent = transform;

            _robotPositionTransforms.Add(positionObject.transform);
        }
    }

    public Transform GetGatherPosition()
    {
        if (_robotsAttempting >= _maxRobotAmount)
        {
            return null;
        }
        else
        {
            _robotsAttempting++;
        }

        return _robotPositionTransforms[_robotsAttempting - 1];
    }

    public void AddRobot(RobotAI robot)
    {
        _robotsCarrying++;

        _robots.Add(robot);
    }

    public void RemoveRobot(RobotAI robot)
    {
        Debug.Log("RemoveRobot");

        if (_robotsAttempting > _robotsCarrying) // this still needs a fix // new robots get placed in a position where a robot already is ( because the removed robot does not have to be in the slot of the new robot position so they overlap )
        {
            _robotsAttempting--;
        }
        else
        {
            _robotsCarrying--;

            _robotsAttempting--;
        }

        _robots.Add(robot);
    }

    [SerializeField] float gizmoSize = 0.1f;

    private void OnDrawGizmos()
    {
        if (_robotPositionTransforms != null && _robotPositionTransforms.Count > 0)
        {
            Gizmos.color = Color.white;

            foreach (Transform position in _robotPositionTransforms)
            {
                if (position != null)
                {
                    Gizmos.DrawCube(position.position, Vector3.one * gizmoSize);
                }
            }
        }
    }
}
