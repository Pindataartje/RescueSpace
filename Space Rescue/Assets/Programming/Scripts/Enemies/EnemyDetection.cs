using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] EnemyAI enemyAI;

    public string[] detectableObjects;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < detectableObjects.Length; i++)
        {
            if (other.CompareTag(detectableObjects[i]))
            {
                enemyAI.EnterDetection(other.transform);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Robot") || other.CompareTag("Player"))
        {
            enemyAI.ExitDetection();
        }
    }
}
