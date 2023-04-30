using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CristalDeposit : MonoBehaviour
{
    public EnumTypes.CristalTypes cristalType = EnumTypes.CristalTypes.Count;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friendly") && other.GetComponent<UnitCarry>().CristalDepositTarget == gameObject)
        {
            if(other.GetComponent<UnitCarry>().CarryACristal)
            {
                other.GetComponent<UnitCarry>().DestroyCristal();
            }

            other.GetComponent<UnitCarry>().CarryCristal(gameObject);

            other.GetComponent<NavMeshAgent>().SetDestination(other.GetComponent<Unit>().Base.transform.position);
        }
    }
}
