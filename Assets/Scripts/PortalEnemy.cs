using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEnemy : MonoBehaviour
{
    public GameObject SpawnGauge = null;
    public GameObject CaptureGauge = null;
    public float SpawnSpeed = 0.1f;
    public float CaptureSpeed = 0.01f;
    public int SmallEnemiesAmountPerSpawn = 0;
    public int MediumEnemiesAmountPerSpawn = 0;
    public int BigEnemiesAmountPerSpawn = 0;
    public int UnitsOnThePortal = 0;

    private void Start()
    {
        SpawnGauge.transform.localScale = new Vector3(0f, 1.01f, 0f);
        CaptureGauge.transform.localScale = new Vector3(0f, 1.02f, 0f);
    }

    private void Update()
    {
        SpawnProgress();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friendly"))
        {
            UnitsOnThePortal +=1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Friendly"))
        {
            UnitsOnThePortal -= 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Friendly"))
        {
            CaptureProgress();
        }
    }

    public void ResetSpawn()
    {
        SpawnGauge.transform.localScale = new Vector3(0f, 1.01f, 0f);
    }

    public void SpawnProgress()
    {
        SpawnGauge.transform.localScale += new Vector3(SpawnSpeed * Time.deltaTime, 0f, SpawnSpeed * Time.deltaTime);

        if (SpawnGauge.transform.localScale.x >= 1.0f)
        {
            Spawn();
            ResetSpawn();
        }
    }

    public void Spawn()
    {
        for(int i = 0; i< SmallEnemiesAmountPerSpawn; i++)
        {
            Instantiate(EnumTypes.Instance.EnemySmallPrefab, RandomPointOnXZCircle(transform.position, transform.localScale.x/2), Quaternion.identity);
        }
        for (int i = 0; i < MediumEnemiesAmountPerSpawn; i++)
        {
            Instantiate(EnumTypes.Instance.EnemyMediumPrefab, RandomPointOnXZCircle(transform.position, transform.localScale.x / 2), Quaternion.identity);
        }
        for (int i = 0; i < BigEnemiesAmountPerSpawn; i++)
        {
            Instantiate(EnumTypes.Instance.EnemyBigPrefab, RandomPointOnXZCircle(transform.position, transform.localScale.x / 2), Quaternion.identity);
        }
    }

    public void CaptureProgress()
    {
        CaptureGauge.transform.localScale += new Vector3(CaptureSpeed * UnitsOnThePortal * Time.deltaTime, 0f, CaptureSpeed * UnitsOnThePortal * Time.deltaTime);

        if (CaptureGauge.transform.localScale.x >= 1.0f)
        {
            Capture();
        }
    }

    public void Capture()
    {
        Destroy(gameObject);
    }

    public Vector3 RandomPointOnXZCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }
}
