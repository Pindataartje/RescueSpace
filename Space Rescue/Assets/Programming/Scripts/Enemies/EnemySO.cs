using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy Data", order = 2)]

public class EnemySO : ScriptableObject
{
    public int health;
    public float speed;

    public float range;

    public int damage;
    public float fireRate;

    public float windUpTime;
    public float windDownTime;

    public float retreatTime;
}
