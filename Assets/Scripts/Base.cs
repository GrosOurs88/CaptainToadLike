using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{
    private GameObject CristalCanvas = null;

    public int PV = 10;
    private float DamageRatio = 0f;
    [HideInInspector] public NavMeshAgent NavmeshAgent;
    public float StopMovingDistance = 0.1f;

    private void Start()
    {
        NavmeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friendly") && other.GetComponent<Unit>().CarryACristal)
        {
            AddResources(other);

            other.GetComponent<Unit>().PlaceCristal();

            other.GetComponent<NavMeshAgent>().SetDestination(other.GetComponent<Unit>().CristalDepositTarget.transform.position);
        }

        if (other.CompareTag("Enemy"))
        {
            TakeDamage();

            Destroy(other.gameObject);
        }
    }

    private void AddResources(Collider col)
    {
        EnumTypes.CristalTypes cristaltype = col.GetComponent<Unit>().CarriedCristal.GetComponent<Cristal>().cristalType;

        CristalCanvas = Instantiate(EnumTypes.Instance.CanvasCristalPrefab, transform.position + new Vector3(0.0f, 3.5f, 0.0f), Quaternion.identity);

        switch (cristaltype)
        {
            case EnumTypes.CristalTypes.blue:
                UIManager.Instance.AddCristals(1, 0, 0);
                CristalCanvas.GetComponent<CanvasCristal>().CristalIcon.color = EnumTypes.Instance.CristalBlueColor;
                break;
            case EnumTypes.CristalTypes.yellow:
                UIManager.Instance.AddCristals(0, 1, 0);
                CristalCanvas.GetComponent<CanvasCristal>().CristalIcon.color = EnumTypes.Instance.CristalYellowColor;
                break;
            case EnumTypes.CristalTypes.red:
                UIManager.Instance.AddCristals(0, 0, 1);
                CristalCanvas.GetComponent<CanvasCristal>().CristalIcon.color = EnumTypes.Instance.CristalRedColor;
                break;
        }
    }

    private void CheckPath()
    {
        if ((transform.position - NavmeshAgent.destination).magnitude <= StopMovingDistance)
        {
            NavmeshAgent.ResetPath();
        }
    }

    public void TakeDamage()
    {
        PV -= 1;

        if (PV <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        print("Game Over");
    }
}
