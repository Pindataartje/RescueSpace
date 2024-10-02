using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SteeredBehaviour", menuName = "AI/Behavior/Steered", order = 0)]
public class SteeredCohesionBehaviour : RobotBehaviour
{
    public override Vector3 CalculateMove(RobotAI agent, List<Transform> context, RobotManager manager)
    {
        throw new System.NotImplementedException();
    }
}
