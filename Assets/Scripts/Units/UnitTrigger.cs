using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTrigger : MonoBehaviour
{
    public Unit unit;
    private UnitAnimation unitAnimation;
    private UnitAttack unitAttack;
    private UnitCarry unitCarry;

    private void Start()
    {
        unitAnimation = unit.unitAnimation;
        unitAttack = unit.unitAttack;
        unitCarry = unit.unitCarry;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && unitCarry.CarryACristal == false && unitAttack.HasEnemyTarget == false)
        {
            unitAnimation.SetAnimationBool("IsShooting", true);

            unitAttack.EnemyTarget = other.gameObject;
            unitAttack.HasEnemyTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == unitAttack.EnemyTarget)
        {
            unitAnimation.SetAnimationBool("IsShooting", false);

            unitAttack.EnemyTarget = null;
            unitAttack.HasEnemyTarget = false;
        }
    }
}
