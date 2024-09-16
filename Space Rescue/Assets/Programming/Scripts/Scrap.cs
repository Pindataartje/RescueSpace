using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scrap : MonoBehaviour
{
    [SerializeField] Transform _objectToCarry;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] int _numberOfRobotsNeeded;
    [SerializeField] int _numberOfRobots;

    [SerializeField] float _extraRobotPercent;

    [SerializeField] float _radius;

    [SerializeField] Transform _player;
    [SerializeField] List<Transform> _robots = new List<Transform>();

    [SerializeField] List<Transform> _robotPositionTransforms = new List<Transform>();

    private void Start()
    {
        GeneratePositionTransforms();
    }

    private void GeneratePositionTransforms()
    {
        int totalPositions = Mathf.CeilToInt(_numberOfRobotsNeeded * _extraRobotPercent);

        float angleStep = 360f / totalPositions;

        for (int i = 0; i < totalPositions; i++)
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
        _numberOfRobots++;

        return _robotPositionTransforms[_numberOfRobots - 1];
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
