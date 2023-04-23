using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<GameObject> EnemySpawners = new List<GameObject>();
    private GameManager GameManager = null;

    private void Start()
    {
        GameManager = GameManager.Instance;
    }

    public void ActivateAreaEnemySpawners()
    {
        foreach(GameObject EnemySpawner in EnemySpawners)
        {
            if(!GameManager.EnemySpawnersActive.Contains(EnemySpawner))
            {
                GameManager.EnemySpawnersActive.Add(EnemySpawner);
            }
        }
    }
}
