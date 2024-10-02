using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotBehaviour", menuName = "RobotBehaviour", order = 0)]
public abstract class RobotBehaviour : ScriptableObject
{
    public abstract Vector3 CalculateMove(RobotAI agent, List<Transform> context, RobotManager manager);
}
