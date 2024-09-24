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

    public virtual void Start() { }

    public virtual void Update() { }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;

            Death();
        }
    }

    public virtual void Death()
    {
        Destroy(this.gameObject);
    }
}
