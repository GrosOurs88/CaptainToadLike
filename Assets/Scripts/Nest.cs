using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nest : MonoBehaviour
{
    public bool IsActivated = false; //[HideInInspector]
    public int SmallEnemiesAmountPerSpawn = 0;
    public int MediumEnemiesAmountPerSpawn = 0;
    public int BigEnemiesAmountPerSpawn = 0;
    public float SpawnRadius = 1.0f;

    public void Activate()
    {
        GetComponent<Animator>().SetTrigger("Activated");
        IsActivated = true;
    }

    public void Deactivate()
    {
        GetComponent<Animator>().SetTrigger("Deactivated");
        IsActivated = false;
    }

    public void Spawn()
    {
        for (int i = 0; i < SmallEnemiesAmountPerSpawn; i++)
        {
            Instantiate(GameplayElementsManager.Instance.EnemySmallPrefab, RandomPointOnXZCircle(transform.position, SpawnRadius), Quaternion.identity);
        }
        for (int i = 0; i < MediumEnemiesAmountPerSpawn; i++)
        {
            Instantiate(GameplayElementsManager.Instance.EnemyMediumPrefab, RandomPointOnXZCircle(transform.position, SpawnRadius), Quaternion.identity);
        }
        for (int i = 0; i < BigEnemiesAmountPerSpawn; i++)
        {
            Instantiate(GameplayElementsManager.Instance.EnemyBigPrefab, RandomPointOnXZCircle(transform.position, SpawnRadius), Quaternion.identity);
        }
    }

    private Vector3 RandomPointOnXZCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }
}
