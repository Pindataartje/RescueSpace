using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvoidanceBehaviour", menuName = "AI/Behavior/Avoidance", order = 0)]
public class AvoidanceBehaviour : RobotBehaviour
{
    public override Vector3 CalculateMove(RobotAI agent, List<Transform> context, RobotManager manager)
    {
        throw new System.NotImplementedException();
    }
}
