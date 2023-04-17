using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{
    private GameObject CristalCanvas = null;

    public int PV = 10;
    private float DamageRatio = 0f;

    private void Start()
    {
        DamageRatio = transform.localScale.x / (float)PV;
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

        CristalCanvas = Instantiate(EnumTypes.Instance.CanvasCristalPrefab, transform.position, Quaternion.identity);

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

    private void TakeDamage()
    {
        transform.localScale -= new Vector3(DamageRatio, 0f, DamageRatio);

        if (transform.localScale.x <= 0.05f)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        print("Game Over");
    }
}
