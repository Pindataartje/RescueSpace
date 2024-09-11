using UnityEngine;

[CreateAssetMenu(fileName = "Robot", menuName = "Robot Data", order = 1)]

public class Robot : ScriptableObject
{
    public float health;
    public float speed;

    public float range;
    public float followDistance;

    public int damage;
    public int impactDamage;

    public float fireRate;

    public float windUpTime;
    public float windDownTime;

    public float retreatTime;
}