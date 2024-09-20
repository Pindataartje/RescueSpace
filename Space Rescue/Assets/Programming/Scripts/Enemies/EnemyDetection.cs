using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void OnDisable()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot") || other.CompareTag("Player"))
        {
            enemyAI.EnterDetection(other.transform);
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
