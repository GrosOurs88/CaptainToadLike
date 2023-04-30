using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    UnitAnimation unitAnimation;

    public bool HasEnemyTarget = false;
    public GameObject EnemyTarget = null;
    public float FireRate = 2.0f;
    private float FireRateTimer = 0.0f;
    public CapsuleCollider CapsuleTrigger = null;
    private float ShotDistance = 0.0f;
    public Transform BulletSpawner = null;
    private GameObject NewBullet = null;
    private GameObject BulletPrefab;
    public float ShootOffsetY = 1.0f;

    private void Start()
    {
        ShotDistance = CapsuleTrigger.radius + 0.5f; //offset to avoid the enemy to be perfectly at the distance of the sphere trigger
        BulletPrefab = GameplayElementsManager.Instance.BulletPrefab;
    }

    private void Update()
    {
        if (HasEnemyTarget)
        {
            if (EnemyTarget == null)
            {
                unitAnimation.SetAnimationBool("IsShooting", false);
                HasEnemyTarget = false;

                LookForANewTarget();

                return;
            }

            LookAtEnemy();
            CheckEnemyDistance();

            if (CheckEnemyDistance() <= ShotDistance)
            {
                //  NavmeshAgent.ResetPath();

                FireRateTimer += Time.deltaTime;
                if (FireRateTimer >= FireRate)
                {
                    Shoot();
                    FireRateTimer = 0.0f;
                }
            }
        }
    }

    private float CheckEnemyDistance()
    {
        return (transform.position - EnemyTarget.transform.position).magnitude;
    }

    private void LookAtEnemy()
    {
        transform.LookAt(EnemyTarget.transform.position, Vector3.up);
        transform.rotation *= Quaternion.Euler(1.0f, 0.0f, 1.0f);
    }

    private void Shoot()
    {
        NewBullet = Instantiate(BulletPrefab, BulletSpawner.position, Quaternion.identity);
        NewBullet.transform.LookAt(EnemyTarget.transform.position + Vector3.up * ShootOffsetY);
    }

    private void LookForANewTarget()
    {
        float Dist = 100.0f;
        GameObject NewTarget = null;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, CapsuleTrigger.radius);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                if ((col.transform.position - transform.position).magnitude < Dist)
                {
                    Dist = (col.transform.position - transform.position).magnitude;
                    NewTarget = col.gameObject;
                }
            }
        }

        if (NewTarget != null)
        {
            unitAnimation.SetAnimationBool("IsShooting", true);
            EnemyTarget = NewTarget;
            HasEnemyTarget = true;
        }
    }
}
