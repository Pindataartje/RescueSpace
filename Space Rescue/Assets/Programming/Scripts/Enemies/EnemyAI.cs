using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Entity
{
    [SerializeField] Enemy _enemyInfo;

    [SerializeField] Material _killedMaterial;

    [SerializeField] Material _normalMaterial;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        StartCoroutine(Damage());
    }

    public override void Start()
    {
        base.Start();

        health = _enemyInfo.health;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    IEnumerator Damage()
    {
        Renderer renderer = GetComponent<Renderer>();

        renderer.material = _killedMaterial;

        yield return new WaitForSeconds(0.2f);

        if (health > 0)
        {
            renderer.material = _normalMaterial;
        }
    }
}
