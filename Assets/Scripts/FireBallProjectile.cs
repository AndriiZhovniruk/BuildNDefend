using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{

    public static FireBallProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfFireBallProjectile = Resources.Load<Transform>("pfFireBallProjectile");
        Transform fireballTransform = Instantiate(pfFireBallProjectile, position, Quaternion.identity);
        FireBallProjectile fireBallProjectile = fireballTransform.GetComponent<FireBallProjectile>();
        fireBallProjectile.SetTarget(enemy);
        return fireBallProjectile;
    }

    private Enemy targetEnemy;
    private Vector3 lastMoveDir;
    private float timeToDie = 1f;
    private void Update()
    {
        Vector3 moveDir;
        if (targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            moveDir = lastMoveDir;
        }
        float moveSpeed = 40;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAcngeFromVector(moveDir));

        timeToDie -= Time.deltaTime;
        if (timeToDie < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        int damageAmount = 8;
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            Explode(damageAmount);
            Destroy(gameObject);
        }

    }

    private void Explode(int dmg)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D nearbyCollider in colliders)
        {
            
                Enemy enemy = nearbyCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.GetComponent<HealthSystem>().Damage(dmg);
                }
            
        }

    }

}
