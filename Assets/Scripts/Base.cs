using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{
    private GameObject CristalCanvas = null;

    public int PV = 10;
    private float DamageRatio = 0f;
   // [HideInInspector] public NavMeshAgent NavmeshAgent;
    public float StopMovingDistance = 0.1f;
    public Vector3 CristalCanvasInstantiationOffset = Vector3.zero;

    private void Start()
    {
  //      NavmeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friendly") && other.GetComponent<UnitCarry>().CarryACristal)
        {
            AddResources(other);

            other.GetComponent<UnitCarry>().PlaceCristal();

            other.GetComponent<NavMeshAgent>().SetDestination(other.GetComponent<UnitCarry>().CristalDepositTarget.transform.position);
        }

        if (other.CompareTag("Enemy"))
        {
            TakeDamage();

            Destroy(other.gameObject);
        }
    }

    private void AddResources(Collider col)
    {
        EnumTypes.CristalTypes cristaltype = col.GetComponent<UnitCarry>().CarriedCristal.GetComponent<Cristal>().cristalType;

        CristalCanvas = Instantiate(GameplayElementsManager.Instance.CanvasCristalPrefab, transform.position + CristalCanvasInstantiationOffset, Quaternion.identity);

        switch (cristaltype)
        {
            case EnumTypes.CristalTypes.blue:
                UIManager.Instance.AddCristals(1, 0, 0);
                CristalCanvas.GetComponent<CanvasCristal>().CristalIcon.color = GameplayElementsManager.Instance.CristalBlueColor;
                break;
            case EnumTypes.CristalTypes.yellow:
                UIManager.Instance.AddCristals(0, 1, 0);
                CristalCanvas.GetComponent<CanvasCristal>().CristalIcon.color = GameplayElementsManager.Instance.CristalYellowColor;
                break;
            case EnumTypes.CristalTypes.red:
                UIManager.Instance.AddCristals(0, 0, 1);
                CristalCanvas.GetComponent<CanvasCristal>().CristalIcon.color = GameplayElementsManager.Instance.CristalRedColor;
                break;
        }
    }

    //private void CheckPath()
    //{
    //    if ((transform.position - NavmeshAgent.destination).magnitude <= StopMovingDistance)
    //    {
    //        NavmeshAgent.ResetPath();
    //    }
    //}

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
