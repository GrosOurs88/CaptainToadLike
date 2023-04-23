using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNestArea : MonoBehaviour
{
    private bool IsActivated = false;
    public List<GameObject> Nests = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Friendly") && IsActivated == false)
        {
            foreach (GameObject Nest in Nests)
            {
                Nest.GetComponent<Nest>().Activate();

                Nest.GetComponent<Nest>().NestAreas.Add(gameObject);
            }

            IsActivated = true;
        }
    }

    public void CheckNests()
    {
        foreach(GameObject Nest in Nests)
        {
            if(Nest.GetComponent<Nest>().IsActivated)
            {
                return;
            }
        }

        IsActivated = false;
    }
}
