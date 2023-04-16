using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public EnumTypes.PlayerTypes playerType = EnumTypes.PlayerTypes.Count;
    public EnumTypes.UnitTypes unitType = EnumTypes.UnitTypes.Count;

    public GameObject SelectionImage;
    public bool AsACristalDepositTarget = false;
    public GameObject CristalDepositTarget;
    public bool CarryACristal = false;
    public GameObject CarriedCristal = null;
    private bool HasPath = false;
    public Transform CristalCarryPosition;
    public GameObject Base;
    private NavMeshAgent NavmeshAgent;

    private void Start()
    {
        NavmeshAgent = GetComponent<NavMeshAgent>();

        SetBase();
        IsUnselected();
    }

    private void Update()
    {
        CheckPath();
    }

    public void CheckPath()
    {
        if (!HasPath)
        {
            if (NavmeshAgent.hasPath)
            {
                SetAnimationTrigger("IsMoving");
                HasPath = true;
            }
        }
        if (HasPath)
        {
            if (!NavmeshAgent.hasPath)
            {
                SetAnimationTrigger("Idle");
                HasPath = false;
            }
        }
    }

    public void IsSelected()
    {
        SelectionImage.SetActive(true);
    }

    public void IsUnselected()
    {
        SelectionImage.SetActive(false);
    }

    public void SelectCristalTarget(GameObject target)
    {
        CristalDepositTarget = target;
        AsACristalDepositTarget = true;
    }

    public void UnselectCristalTarget()
    {
        CristalDepositTarget = null;
        AsACristalDepositTarget = false;
    }

    public void CarryCristal(GameObject cristalDeposit)
    {
        EnumTypes.CristalTypes type = cristalDeposit.GetComponent<CristalDeposit>().cristalType;

        switch(type)
        {
            case EnumTypes.CristalTypes.blue:
                CarriedCristal = Instantiate(EnumTypes.Instance.CristalBluePrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
                break;
            case EnumTypes.CristalTypes.yellow:
                CarriedCristal = Instantiate(EnumTypes.Instance.CristalYellowPrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
                break;
            case EnumTypes.CristalTypes.red:
                CarriedCristal = Instantiate(EnumTypes.Instance.CristalRedPrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
                break;
        }

        CarryACristal = true;
    }  

    public void DestroyCristal()
    {
        Destroy(CarriedCristal);
    }

    public void PlaceCristal()
    {
        Destroy(CarriedCristal);
        CarriedCristal = null; //security to override "missing" value
        CarryACristal = false;
    }

    public void SetAnimationTrigger(string animTriggerName)
    {
        GetComponent<Animator>().SetTrigger(animTriggerName);
    }

    public void SetBase()
    {
        switch (playerType)
        {
            case EnumTypes.PlayerTypes.player:
                Base = EnumTypes.Instance.BasePlayer;
                break;
            case EnumTypes.PlayerTypes.enemy1:
                Base = EnumTypes.Instance.BaseEnemy1;
                break;
            case EnumTypes.PlayerTypes.enemy2:
                Base = EnumTypes.Instance.BaseEnemy2;
                break;
            case EnumTypes.PlayerTypes.enemy3:
                Base = EnumTypes.Instance.BaseEnemy3;
                break;
        }
    }
}
