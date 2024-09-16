using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPosition : MonoBehaviour
{
    public Transform objectToCarry;
    public int numberOfRobotsNeeded;
    public float radius;
    public Transform player;
    public List<Transform> robots = new List<Transform>();

    public float _extraRobotPercent;

    private List<Transform> robotPositionTransforms = new List<Transform>();

    void Start()
    {
        GeneratePositionTransforms();
    }

    private void GeneratePositionTransforms()
    {
        int totalPositions = Mathf.CeilToInt(numberOfRobotsNeeded * _extraRobotPercent);

        float angleStep = 360f / totalPositions;

        for (int i = 0; i < totalPositions; i++)
        {
            float angle = i * angleStep;
            float angleRad = Mathf.Deg2Rad * angle;

            Vector3 position = new Vector3(objectToCarry.position.x + Mathf.Cos(angleRad) * radius, objectToCarry.position.y, objectToCarry.position.z + Mathf.Sin(angleRad) * radius);

            GameObject positionObject = new GameObject("Position_" + i);
            positionObject.transform.position = position;
            positionObject.transform.parent = transform;

            robotPositionTransforms.Add(positionObject.transform);
        }
    }

    [SerializeField] float gizmoSize = 0.1f;

    private void OnDrawGizmos()
    {
        if (robotPositionTransforms != null && robotPositionTransforms.Count > 0)
        {
            Gizmos.color = Color.white;

            foreach (Transform position in robotPositionTransforms)
            {
                if (position != null)
                {
                    Gizmos.DrawCube(position.position, Vector3.one * gizmoSize);
                }
            }
        }
    }
}
