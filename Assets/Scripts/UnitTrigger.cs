using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTrigger : MonoBehaviour
{
    public Unit unit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && unit.CarryACristal == false && unit.HasEnemyTarget == false)
        {
            unit.SetAnimationBool("IsShooting", true);

            unit.EnemyTarget = other.gameObject;
            unit.HasEnemyTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == unit.EnemyTarget)
        {
            unit.SetAnimationBool("IsShooting", false);

            unit.EnemyTarget = null;
            unit.HasEnemyTarget = false;
        }
    }
}
