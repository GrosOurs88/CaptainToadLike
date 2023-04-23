using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public Area Area = null;
    public ParticleSystem FogParticleSystem = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Friendly"))
        {
            RemoveFog();
            Area.ActivateAreaEnemySpawners();
        }
    }

    public void RemoveFog()
    {
        FogParticleSystem.Stop();
    }
}
