using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CristalDeposit : MonoBehaviour
{
    public EnumTypes.CristalTypes cristalType = EnumTypes.CristalTypes.Count;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friendly") && other.GetComponent<Unit>().CristalDepositTarget == gameObject)
        {
            if(other.GetComponent<Unit>().CarryACristal)
            {
                other.GetComponent<Unit>().DestroyCristal();
            }

            other.GetComponent<Unit>().CarryCristal(gameObject);

            other.GetComponent<NavMeshAgent>().SetDestination(other.GetComponent<Unit>().Base.transform.position);
        }
    }
}
