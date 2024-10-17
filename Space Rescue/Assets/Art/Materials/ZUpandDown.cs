using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZUpandDown : MonoBehaviour
{
    public float speed = 2.0f;  // Speed of the up and down movement
    public float height = 0.25f; // Height of the movement range

    private Vector3 startPosition;  // To store the initial position

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = Mathf.Sin(Time.time * speed) * height;

        // Set the object's position, keeping X and Z the same
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
    }
}
