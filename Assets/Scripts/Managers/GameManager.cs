using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> EnemySpawnersActive = new List<GameObject>(); //private
    public GameObject AreaStart = null;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnemySpawnersActive.Clear();
        SetEnemySpawnerActiveList();
    }

    private void SetEnemySpawnerActiveList()
    {
        foreach(GameObject EnemySpawner in AreaStart.GetComponent<Area>().EnemySpawners)
        {
            EnemySpawnersActive.Add(EnemySpawner);
        }
    }

    public void TriggerEnemySpawnEvent() //to enlarge with different types
    {
        foreach (GameObject EnemySpawner in EnemySpawnersActive)
        {
            EnemySpawner.GetComponent<Nest>().Spawn();
        }
    }
}
