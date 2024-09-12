using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPosition : MonoBehaviour
{
    // Public variables to set the radius of the circle and how many Pikmin are around the object
    public Transform objectToCarry;   // Object that Pikmin will carry
    public int numberOfPikmin = 5;    // Number of Pikmin
    public float radius = 1f;         // Radius of the circle around the object

    // Call this to reposition Pikmin around the object
    public void PositionPikmin(Transform[] pikminArray)
    {
        numberOfPikmin = pikminArray.Length;
        // Calculate angle step
        float angleStep = 360f / numberOfPikmin;

        for (int i = 0; i < numberOfPikmin; i++)
        {
            // Calculate angle for this Pikmin
            float angle = i * angleStep;

            // Convert the angle to radians
            float angleRad = Mathf.Deg2Rad * angle;

            // Calculate the position on the circle
            Vector3 pikminPos = new Vector3(
                objectToCarry.position.x + Mathf.Cos(angleRad) * radius,
                objectToCarry.position.y,
                objectToCarry.position.z + Mathf.Sin(angleRad) * radius
            );

            // Set Pikmin position
            pikminArray[i].position = pikminPos;

            // Optionally rotate Pikmin to face the object
            pikminArray[i].LookAt(objectToCarry);
        }
    }
}
