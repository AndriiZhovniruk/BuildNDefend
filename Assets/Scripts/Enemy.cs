using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
    
        Transform pfEnemy;
        if ( EnemyWaveManager.Instance.RemainingEnemySpawnAmount() <= EnemyWaveManager.Instance.GetWaveNumber()) {
            pfEnemy = Resources.Load<Transform>("pfBoss");
            
        }
        else
        {
            pfEnemy = Resources.Load<Transform>("pfEnemy");
        }
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        
        return enemy;
    }



    private Transform targetTransform;
    private Rigidbody2D rigidbody2d;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;
    private HealthSystem healthSystem;
    private float moveSpeed = 6f;
    private void Start()
    {

        rigidbody2d = GetComponent<Rigidbody2D>();
        if (BuildingManeger.Instance.GetHQBuilding() != null)
        {
            targetTransform = BuildingManeger.Instance.GetHQBuilding().transform;
        }
        healthSystem =  GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
    }


    private void Update()
    {
        HandleMovement();
        HandeTargeting();
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building != null)
        {
            HealthSystem BuildingHealthSystem = building.GetComponent<HealthSystem>();
            //healthSystem.GetHealthAmount();
            if(BuildingHealthSystem.GetHealthAmount() >= healthSystem.GetHealthAmount())
            {
                BuildingHealthSystem.Damage(healthSystem.GetHealthAmount());
                Destroy(gameObject);
            }
            else
            {
                int enemyHPLeft = BuildingHealthSystem.GetHealthAmount();
                BuildingHealthSystem.Damage(healthSystem.GetHealthAmount());
                healthSystem.Damage(enemyHPLeft);

            }
            
        }
    }

    private void LookForTargets()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);
        foreach(Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if(building != null)
            {
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if(Vector3.Distance(transform.position, building.transform.position) <
                       Vector3.Distance(transform.position, targetTransform.position))
                    {
                        targetTransform = building.transform;
                    }
                }
            }
        }
        if(targetTransform == null)
        {
            if(BuildingManeger.Instance.GetHQBuilding()!= null)
            {
                targetTransform = BuildingManeger.Instance.GetHQBuilding().transform;
            }
            
        }
    }
    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;
            
            rigidbody2d.velocity = moveDir * (moveSpeed + (float)EnemyWaveManager.Instance.GetWaveNumber()/10 );
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }
    private void HandeTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }
}
