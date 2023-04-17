using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public EnumTypes.PlayerTypes playerType = EnumTypes.PlayerTypes.Count;
    public EnumTypes.UnitTypes unitType = EnumTypes.UnitTypes.Count;

    [Header("Cristal")]
    public GameObject SelectionImage = null;
    public bool AsACristalDepositTarget = false;
    public GameObject CristalDepositTarget = null;
    public bool CarryACristal = false;
    public GameObject CarriedCristal = null;
    private bool HasPath = false;
    public Transform CristalCarryPosition = null;
    public GameObject Base = null;
    private NavMeshAgent NavmeshAgent = null;
    [Header("Shot")]
    public bool HasEnemyTarget = false;
    public GameObject EnemyTarget = null;
    public float FireRate = 2.0f;
    private float FireRateTimer = 0.0f;
    public CapsuleCollider CapsuleTrigger = null;
    private float ShotDistance = 0.0f;
    public Transform BulletSpawner = null;
    private GameObject NewBullet = null;
    private GameObject BulletPrefab;
    public int PV = 5;
    public float ShootOffsetY = 1.0f;


    private void Start()
    {
        NavmeshAgent = GetComponent<NavMeshAgent>();
        ShotDistance = CapsuleTrigger.radius + 0.5f; //offset to avoid the enemy to be perfectly at the distance of the sphere trigger
        BulletPrefab = EnumTypes.Instance.BulletPrefab;

        SetBase();
        IsUnselected();
    }

    private void Update()
    {
        CheckPath();
        if (HasEnemyTarget)
        {
            if (EnemyTarget == null)
            {
                SetAnimationBool("IsShooting", false);
                HasEnemyTarget = false;

                LookForANewTarget();

                return;
            }

            LookAtEnemy();
            CheckEnemyDistance();

            if (CheckEnemyDistance() <= ShotDistance)
            {
                FireRateTimer += Time.deltaTime;
                if (FireRateTimer >= FireRate)
                {
                    Shoot();
                    FireRateTimer = 0.0f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && HasEnemyTarget == false)
        {
            SetAnimationBool("IsShooting", true);

            EnemyTarget = other.gameObject;
            HasEnemyTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == EnemyTarget)
        {
            SetAnimationBool("IsShooting", false);

            EnemyTarget = null;
            HasEnemyTarget = false;
        }
    }

    private float CheckEnemyDistance()
    {
        return (transform.position - EnemyTarget.transform.position).magnitude;
    }

    private void CheckPath()
    {
        if (!HasPath)
        {
            if (NavmeshAgent.hasPath)
            {
                SetAnimationTrigger("IsMoving");
                HasPath = true;
            }
        }
        if (HasPath)
        {
            if (!NavmeshAgent.hasPath)
            {
                SetAnimationTrigger("Idle");
                HasPath = false;
            }
        }
    }

    public void IsSelected()
    {
        SelectionImage.SetActive(true);
    }

    public void IsUnselected()
    {
        SelectionImage.SetActive(false);
    }

    private void SelectCristalTarget(GameObject target)
    {
        CristalDepositTarget = target;
        AsACristalDepositTarget = true;
    }

    private void UnselectCristalTarget()
    {
        CristalDepositTarget = null;
        AsACristalDepositTarget = false;
    }

    public void CarryCristal(GameObject cristalDeposit)
    {
        EnumTypes.CristalTypes type = cristalDeposit.GetComponent<CristalDeposit>().cristalType;

        switch (type)
        {
            case EnumTypes.CristalTypes.blue:
                CarriedCristal = Instantiate(EnumTypes.Instance.CristalBluePrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
                break;
            case EnumTypes.CristalTypes.yellow:
                CarriedCristal = Instantiate(EnumTypes.Instance.CristalYellowPrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
                break;
            case EnumTypes.CristalTypes.red:
                CarriedCristal = Instantiate(EnumTypes.Instance.CristalRedPrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
                break;
        }

        CarryACristal = true;
    }

    public void DestroyCristal()
    {
        Destroy(CarriedCristal);
    }

    public void PlaceCristal()
    {
        Destroy(CarriedCristal);
        CarriedCristal = null; //security to override "missing" value
        CarryACristal = false;
    }

    private void SetAnimationTrigger(string animTriggerName)
    {
        GetComponent<Animator>().SetTrigger(animTriggerName);
    }

    private void SetAnimationBool(string animBoolName, bool boolValue)
    {
        GetComponent<Animator>().SetBool(animBoolName, boolValue);
    }

    private void SetBase()
    {
        switch (playerType)
        {
            case EnumTypes.PlayerTypes.player:
                Base = EnumTypes.Instance.BasePlayer;
                break;
            case EnumTypes.PlayerTypes.enemy1:
                Base = EnumTypes.Instance.BaseEnemy1;
                break;
            case EnumTypes.PlayerTypes.enemy2:
                Base = EnumTypes.Instance.BaseEnemy2;
                break;
            case EnumTypes.PlayerTypes.enemy3:
                Base = EnumTypes.Instance.BaseEnemy3;
                break;
        }
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
            if(col.CompareTag("Enemy"))
            {
                if ((col.transform.position - transform.position).magnitude < Dist)
                {
                    Dist = (col.transform.position - transform.position).magnitude;
                    NewTarget = col.gameObject;
                }
            }
        }

        if(NewTarget != null)
        {
            SetAnimationBool("IsShooting", true);
            EnemyTarget = NewTarget;
            HasEnemyTarget = true;
        }
    }

    public void TakeDamage()
    {
        PV -= 1;

        if (PV <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}