using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCarry : MonoBehaviour
{
    public Transform CristalCarryPosition = null;
    public bool AsACristalDepositTarget = false;
    public GameObject CristalDepositTarget = null;
    public bool CarryACristal = false;
    public GameObject CarriedCristal = null; 

    private void SelectCristalTarget(GameObject target)
    {
        CristalDepositTarget = target;
        AsACristalDepositTarget = true;
    }

    private void UnselectCristalTarget()
    {
        CristalDepositTarget = null;
        AsACristalDepositTarget = false;
    }

    public void CarryCristal(GameObject cristalDeposit)
    {
        EnumTypes.CristalTypes type = cristalDeposit.GetComponent<CristalDeposit>().cristalType;

        switch (type)
        {
            case EnumTypes.CristalTypes.blue:
                CarriedCristal = Instantiate(GameplayElementsManager.Instance.CristalBluePrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
                break;
            case EnumTypes.CristalTypes.yellow:
                CarriedCristal = Instantiate(GameplayElementsManager.Instance.CristalYellowPrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
                break;
            case EnumTypes.CristalTypes.red:
                CarriedCristal = Instantiate(GameplayElementsManager.Instance.CristalRedPrefab, CristalCarryPosition.position, Quaternion.identity, CristalCarryPosition);
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
}
