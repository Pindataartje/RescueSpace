using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy Data", order = 2)]

public class EnemySO : ScriptableObject
{
    // general

    public int health;
    public float speed;
    public int damage;

    // patrol

    public float patrolRadius;
    public float patrolReturnDistance;
    public int patrolPoints;

    // search

    public float searchRadius;
    public float timeToSearch;
    public int searchCount;



}
