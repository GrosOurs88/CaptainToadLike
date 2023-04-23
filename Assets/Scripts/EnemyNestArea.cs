using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNestArea : MonoBehaviour
{
    public bool IsActivated = false; //private
    public List<GameObject> Nests = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Friendly") && IsActivated == false)
        {
            foreach (GameObject Nest in Nests)
            {
                Nest.GetComponent<Nest>().Activate();
            }

            IsActivated = true;
        }
    }

    public void CheckNests()
    {
        if(Nests.Count > 0)
        {
            foreach (GameObject Nest in Nests)
            {
                if (Nest.GetComponent<Nest>().IsActivated)
                {
                    return;
                }
            }

            IsActivated = false;
        }        
    }
}
