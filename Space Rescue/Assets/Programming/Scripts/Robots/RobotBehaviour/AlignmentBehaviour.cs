using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlignmentBehaviour", menuName = "AI/Behavior/Alignment", order = 0)]
public class AlignmentBehaviour : RobotBehaviour
{
    public override Vector3 CalculateMove(RobotAI agent, List<Transform> context, RobotManager manager)
    {
        throw new System.NotImplementedException();
    }
}
