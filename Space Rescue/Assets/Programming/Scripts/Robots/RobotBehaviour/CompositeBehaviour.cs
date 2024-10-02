using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior/Composite")]
public class CompositeBehaviour : RobotBehaviour
{
    public RobotBehaviour[] behaviors;
    public float[] weights;

    public override Vector3 CalculateMove(RobotAI agent, List<Transform> context, RobotManager manager)
    {
        //handle data mismatch
        if (weights.Length != behaviors.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;
        }

        //set up move
        Vector3 move = Vector3.zero;

        //iterate through behaviors
        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector3 partialMove = behaviors[i].CalculateMove(agent, context, manager) * weights[i];

            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;

            }
        }
        return move;
    }

}