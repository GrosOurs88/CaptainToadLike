using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public EnumTypes.UnitTypes unitType = EnumTypes.UnitTypes.Count;
    public UnitAnimation unitAnimation;
    public UnitAttack unitAttack;
    public UnitCarry unitCarry;

    private UnitGroup unitGroup;

    public GameObject CanvasSelectionImage = null;
    public GameObject Base = null;
    [HideInInspector] public NavMeshAgent NavmeshAgent = null;
    public float StopMovingDistance = 0.1f;

    private void Start()
    {
        NavmeshAgent = GetComponent<NavMeshAgent>();
        unitGroup = transform.parent.gameObject.GetComponent<UnitGroup>();

        SetBase();
        IsUnselected();
    }

    private void Update()
    {
        CheckPath();
    }   

    private void CheckPath()
    {
        if ((transform.position - NavmeshAgent.destination).magnitude <= StopMovingDistance)
        {
            NavmeshAgent.ResetPath();
            unitAnimation.ResetAnimationTrigger("IsMoving");
            unitAnimation.SetAnimationTrigger("Idle");

            if (unitCarry.CarryACristal)
            {
                print("CarryACristal");
                NavmeshAgent.destination = Base.transform.position;
            }
        }
    }

    public void IsSelected()
    {
        CanvasSelectionImage.SetActive(true);
    }

    public void IsUnselected()
    {
        CanvasSelectionImage.SetActive(false);
    }

    private void SetBase()
    {
        Base = GameplayElementsManager.Instance.BasePlayerPrefab;               
    }  
}