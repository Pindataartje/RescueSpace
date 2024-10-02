using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetingBehaviour", menuName = "AI/Behavior/Targeting", order = 0)]
public class TargetingBehaviour : RobotBehaviour
{
    public Vector3 targetPosition; // This can be set dynamically in your game

    public override Vector3 CalculateMove(RobotAI agent, List<Transform> context, RobotManager manager)
    {
        // Calculate the direction towards the target
        Vector3 directionToTarget = targetPosition - agent.transform.position;

        // If already at target, return no movement
        if (directionToTarget.magnitude < 0.1f)
            return Vector3.zero;

        // Return normalized movement towards target
        return directionToTarget.normalized;
    }
}
