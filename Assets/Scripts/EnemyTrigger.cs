using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friendly") && enemy.HasUnitTarget == false)
        {
            enemy.SetAnimationBool("IsMoving", true);

            enemy.UnitTarget = other.gameObject;
            enemy.SetNewTarget(enemy.UnitTarget);
            enemy.HasUnitTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == enemy.UnitTarget)
        {
            enemy.SetAnimationBool("IsMoving", false);

            enemy.UnitTarget = null;
            enemy.HasUnitTarget = false;
        }
    }
}
