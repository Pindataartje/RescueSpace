using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy Data", order = 2)]

public class EnemySO : ScriptableObject
{
    // general

    public float health;
    public float speed;
    public float damage;

    public float regenRate;
    public float regenAmount;

    // patrol

    public float patrolRadius;
    public float patrolReturnDistance;
    public int patrolPoints;

    // search

    public float searchRadius;
    public float timeToSearch;
    public int searchCount;



}
