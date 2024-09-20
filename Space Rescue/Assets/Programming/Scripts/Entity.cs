using UnityEngine;

public class Entity : MonoBehaviour
{

    [Header("Type")]
    public Type type;

    public enum Type
    {
        NONE,
        FIRE,
        WATER,
    }

    [Header("Info")]

    public float health;
    public float maxHealth;

    public float speed;
    public float damage;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
    }
}
