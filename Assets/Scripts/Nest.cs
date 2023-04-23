using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nest : MonoBehaviour
{
    public List<GameObject> NestAreas = new List<GameObject>();

    [HideInInspector] public bool IsActivated = false;

    public float SpawnTimer = 10.0f;
    public int SmallEnemiesAmountPerSpawn = 0;
    public int MediumEnemiesAmountPerSpawn = 0;
    public int BigEnemiesAmountPerSpawn = 0;
    public Image NestGauge = null;
    public float SpawnRadius = 1.0f;


    private void Start()
    {
        ResetSpawn();
    }

    private void Update()
    {
        if(IsActivated == true)
        {
            SpawnProgress();
        }
    }

    public void Activate()
    {
        NestGauge.transform.parent.gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Activated");
        IsActivated = true;
    }

    public void Deactivate()
    {
        GetComponent<Animator>().SetTrigger("Deactivated");

        foreach (GameObject Area in NestAreas)
        {
            Area.GetComponent<EnemyNestArea>().CheckNests();
        }

        IsActivated = false;
    }

    private void ResetSpawn()
    {
        NestGauge.fillAmount = 0.0f;
        NestGauge.transform.parent.gameObject.SetActive(false);
    }

    private void SpawnProgress()
    {
        NestGauge.fillAmount += Time.deltaTime / SpawnTimer;

        if (NestGauge.fillAmount == 1.0f)
        {
            Spawn();
            ResetSpawn();
            Deactivate();
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < SmallEnemiesAmountPerSpawn; i++)
        {
            Instantiate(EnumTypes.Instance.EnemySmallPrefab, RandomPointOnXZCircle(transform.position, SpawnRadius), Quaternion.identity);
        }
        for (int i = 0; i < MediumEnemiesAmountPerSpawn; i++)
        {
            Instantiate(EnumTypes.Instance.EnemyMediumPrefab, RandomPointOnXZCircle(transform.position, SpawnRadius), Quaternion.identity);
        }
        for (int i = 0; i < BigEnemiesAmountPerSpawn; i++)
        {
            Instantiate(EnumTypes.Instance.EnemyBigPrefab, RandomPointOnXZCircle(transform.position, SpawnRadius), Quaternion.identity);
        }
    }

    private Vector3 RandomPointOnXZCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }

}
