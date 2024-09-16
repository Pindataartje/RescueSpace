using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCarry : MonoBehaviour
{
    public TestPosition pikminPositioner;  // Reference to the positioner script
    public Transform[] pikminArray;            // Array of Pikmin Transforms

    void Update()
    {
        // Example: Dynamically position the Pikmin around the object every frame
        if (Input.GetKeyDown(KeyCode.P))
        {
            // pikminPositioner.PositionPikmin(pikminArray);
        }
    }
}
