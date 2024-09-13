using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPosition : MonoBehaviour
{
    public Transform objectToCarry;
    public int numberOfRobts;
    public float radius;

    public Transform player;

    public void PositionPikmin(Transform[] robots)
    {
        numberOfRobts = robots.Length;

        float angleStep = 360f / numberOfRobts;

        for (int i = 0; i < numberOfRobts; i++)
        {
            float angle = i * angleStep;
            float angleRad = Mathf.Deg2Rad * angle;

            Vector3 robotPos = new Vector3(objectToCarry.position.x + Mathf.Cos(angleRad) * radius, objectToCarry.position.y, objectToCarry.position.z + Mathf.Sin(angleRad) * radius);

            robots[i].position = robotPos;
            robots[i].LookAt(objectToCarry);
        }
    }

    void RobotTypeSplitter(Transform[] robots)
    {
        numberOfRobts = robots.Length;

        float angleStep = 180f / (numberOfRobts - 1);

        for (int i = 0; i < numberOfRobts; i++)
        {
            float angle = -90f + (i * angleStep);
            float angleRad = Mathf.Deg2Rad * angle;

            Vector3 positionOffset = new Vector3(Mathf.Sin(angleRad), 0, -Mathf.Cos(angleRad)) * radius;
            Vector3 robotPos = player.position + player.right * positionOffset.x + player.forward * positionOffset.z;

            robots[i].position = robotPos;
            robots[i].LookAt(player);
        }
    }
}
