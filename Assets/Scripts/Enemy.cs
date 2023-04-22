using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent NavmeshAgent;
    public int PV = 10;
    public bool HasUnitTarget = false;
    public GameObject UnitTarget = null;
    public float FireRate = 2.0f;
    private float FireRateTimer = 0.0f;
    public float AttackRange = 1.0f;
    public SphereCollider SphereTrigger;

    private void Start()
    {
        GetComponent<NavMeshAgent>().destination = EnumTypes.Instance.BasePlayer.transform.position;
        NavmeshAgent = GetComponent<NavMeshAgent>();

        SetAnimationBool("IsMoving", true);
    }

    private void Update()
    {
        if (NavmeshAgent.hasPath)
        {
            if (HasUnitTarget)
            {
                if (UnitTarget == null)
                {
                    SetAnimationBool("IsMoving", false);
                    HasUnitTarget = false;

                    LookForANewTarget();

                    if (!HasUnitTarget)
                    {
                        SetNewTarget(EnumTypes.Instance.BasePlayer);
                        SetAnimationBool("IsMoving", true);
                    }

                    return;
                }

                LookAtEnemy();

                if(CheckEnemyDistance() <= AttackRange)
                {
                    SetAnimationBool("IsMoving", false);

                    FireRateTimer += Time.deltaTime;
                    if (FireRateTimer >= FireRate)
                    {
                        Attack();
                        FireRateTimer = 0.0f;
                    }
                }
            }            
        }
        else
        {
            LookForANewTarget();

            if(!HasUnitTarget)
            {
                SetNewTarget(EnumTypes.Instance.BasePlayer);
                SetAnimationBool("IsMoving", true);
            }
        }
    }

    public void SetAnimationTrigger(string animTriggerName)
    {
        GetComponent<Animator>().SetTrigger(animTriggerName);
    }

    public void SetAnimationBool(string animBoolName, bool boolValue)
    {
        GetComponent<Animator>().SetBool(animBoolName, boolValue);
    }

    private void LookAtEnemy()
    {
        transform.LookAt(UnitTarget.transform.position, Vector3.up);
        transform.rotation *= Quaternion.Euler(1.0f, 0.0f, 1.0f);
    }

    private float CheckEnemyDistance()
    {
        return (transform.position - UnitTarget.transform.position).magnitude;
    }

    private void LookForANewTarget()
    {
        float Dist = 100.0f;
        GameObject NewTarget = null;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SphereTrigger.radius);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Friendly"))
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
            SetAnimationBool("IsMoving", true);
            UnitTarget = NewTarget;
            HasUnitTarget = true;
        }
    }

    public void SetNewTarget(GameObject Go)
    {
        GetComponent<NavMeshAgent>().destination = Go.transform.position;
    }

    private void Attack()
    {
        SetAnimationTrigger("Attack");
        if(UnitTarget.GetComponent<Unit>())
        {
            UnitTarget.GetComponent<Unit>().TakeDamage();
        }
        else if(UnitTarget.GetComponent<Base>())
        {
            UnitTarget.GetComponent<Base>().TakeDamage();
        }
    }

    public void TakeDamage()
    {
        PV -= 1;

        if(PV <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
