using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{
    private GameObject CristalCanvas = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friendly") && other.GetComponent<Unit>().CarryACristal)
        {
            AddResources(other);

            other.GetComponent<Unit>().PlaceCristal();

            other.GetComponent<NavMeshAgent>().SetDestination(other.GetComponent<Unit>().CristalDepositTarget.transform.position);
        }
    }

    public void AddResources(Collider col)
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
}
