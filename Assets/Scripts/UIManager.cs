using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI CristalBlueText, CristalYellowText, CristalRedText;
    public int CristalBlueAmount, CristalYellowAmount, CristalRedAmount;
    public GameObject PlayerBase;
    [Header("Base Units")]
    public TextMeshProUGUI UnitBaseAmountText;
    [HideInInspector] public int UnitBaseAmount = 0;
    public TextMeshProUGUI UnitBaseCristalBlueNeededText;
    public int UnitBaseCristalBlueNeeded;
    public TextMeshProUGUI UnitBaseCristalYellowNeededText;
    public int UnitBaseCristalYellowNeeded;
    public TextMeshProUGUI UnitBaseCristalRedNeededText;
    public int UnitBaseCristalRedNeeded;
    [Header("Carrier Units")]
    public TextMeshProUGUI UnitCarrierAmountText;
    [HideInInspector] public int UnitCarrierAmount = 0;
    public TextMeshProUGUI UnitCarrierCristalBlueNeededText;
    public int UnitCarrierCristalBlueNeeded;
    public TextMeshProUGUI UnitCarrierCristalYellowNeededText;
    public int UnitCarrierCristalYellowNeeded;
    public TextMeshProUGUI UnitCarrierCristalRedNeededText;
    public int UnitCarrierCristalRedNeeded;
    [Header("Fighter Units")]
    public TextMeshProUGUI UnitFighterAmountText;
    [HideInInspector] public int UnitFighterAmount = 0;
    public TextMeshProUGUI UnitFighterCristalBlueNeededText;
    public int UnitFighterCristalBlueNeeded;
    public TextMeshProUGUI UnitFighterCristalYellowNeededText;
    public int UnitFighterCristalYellowNeeded;
    public TextMeshProUGUI UnitFighterCristalRedNeededText;
    public int UnitFighterCristalRedNeeded;

    private GameObject NewUnit = null;
    private Vector3 RandomSpawningPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetCristalsUI();
        ResetUnitsUI();
    }

    public void AddCristals(int amountBlue, int amountYellow, int amountRed)
    {
        CristalBlueAmount += amountBlue;
        CristalYellowAmount += amountYellow;
        CristalRedAmount += amountRed;

        CristalBlueText.text = CristalBlueAmount.ToString();
        CristalYellowText.text = CristalYellowAmount.ToString();
        CristalRedText.text = CristalRedAmount.ToString();
    }

    public void RemoveCristals(int amountBlue, int amountYellow, int amountRed)
    {
        CristalBlueAmount -= amountBlue;
        CristalYellowAmount -= amountYellow;
        CristalRedAmount -= amountRed;

        CristalBlueText.text = CristalBlueAmount.ToString();
        CristalYellowText.text = CristalYellowAmount.ToString();
        CristalRedText.text = CristalRedAmount.ToString();
    }

    public void ResetCristalsUI()
    {
        CristalBlueAmount = 0;
        CristalYellowAmount = 0;
        CristalRedAmount = 0;

        CristalBlueText.text = CristalBlueAmount.ToString();
        CristalYellowText.text = CristalYellowAmount.ToString();
        CristalRedText.text = CristalRedAmount.ToString();
    }

    public void ResetUnitsUI()
    {
        UnitBaseAmountText.text = UnitBaseAmount.ToString();
        UnitCarrierAmountText.text = UnitCarrierAmount.ToString();
        UnitFighterAmountText.text = UnitFighterAmount.ToString();

        UnitBaseCristalBlueNeededText.text = UnitBaseCristalBlueNeeded.ToString();
        UnitBaseCristalYellowNeededText.text = UnitBaseCristalYellowNeeded.ToString();
        UnitBaseCristalRedNeededText.text = UnitBaseCristalRedNeeded.ToString();

        UnitCarrierCristalBlueNeededText.text = UnitCarrierCristalBlueNeeded.ToString();
        UnitCarrierCristalYellowNeededText.text = UnitCarrierCristalYellowNeeded.ToString();
        UnitCarrierCristalRedNeededText.text = UnitCarrierCristalRedNeeded.ToString();

        UnitFighterCristalBlueNeededText.text = UnitFighterCristalBlueNeeded.ToString();
        UnitFighterCristalYellowNeededText.text = UnitFighterCristalYellowNeeded.ToString();
        UnitFighterCristalRedNeededText.text = UnitFighterCristalRedNeeded.ToString();
    }

    public void CreateUnitBase()
    {
        if(CristalBlueAmount >= UnitBaseCristalBlueNeeded && CristalYellowAmount >= UnitBaseCristalYellowNeeded && CristalRedAmount >= UnitBaseCristalRedNeeded)
        {
            NewUnit = Instantiate(EnumTypes.Instance.UnitBasePrefab, PlayerBase.transform.position + RandomPositionInsideBaseArea(), Quaternion.identity);
            SelectionSquare.Instance.availableUnitList.Add(NewUnit);

            RemoveCristals(UnitBaseCristalBlueNeeded, UnitBaseCristalYellowNeeded, UnitBaseCristalRedNeeded);

            UnitBaseAmount += 1;
            UnitBaseAmountText.text = UnitBaseAmount.ToString();
        }
    }

    public void CreateUnitCarrier()
    {
        if (CristalBlueAmount >= UnitCarrierCristalBlueNeeded && CristalYellowAmount >= UnitCarrierCristalYellowNeeded && CristalRedAmount >= UnitCarrierCristalRedNeeded)
        {
            NewUnit = Instantiate(EnumTypes.Instance.UnitCarrierPrefab, PlayerBase.transform.position + RandomPositionInsideBaseArea(), Quaternion.identity);
            SelectionSquare.Instance.availableUnitList.Add(NewUnit);

            RemoveCristals(UnitCarrierCristalBlueNeeded, UnitCarrierCristalYellowNeeded, UnitCarrierCristalRedNeeded);

            UnitCarrierAmount += 1;
            UnitCarrierAmountText.text = UnitCarrierAmount.ToString();
        }
    }

    public void CreateUnitFighter()
    {
        if (CristalBlueAmount >= UnitFighterCristalBlueNeeded && CristalYellowAmount >= UnitFighterCristalYellowNeeded && CristalRedAmount >= UnitFighterCristalRedNeeded)
        {
            NewUnit = Instantiate(EnumTypes.Instance.UnitFighterPrefab, PlayerBase.transform.position + RandomPositionInsideBaseArea(), Quaternion.identity);
            SelectionSquare.Instance.availableUnitList.Add(NewUnit);

            RemoveCristals(UnitFighterCristalBlueNeeded, UnitFighterCristalYellowNeeded, UnitFighterCristalRedNeeded);

            UnitFighterAmount += 1;
            UnitFighterAmountText.text = UnitFighterAmount.ToString();
        }
    }

    public Vector3 RandomPositionInsideBaseArea()
    {
        RandomSpawningPosition = new Vector3 (Random.Range(-1,1), 0.0f, Random.Range(-1, 1));

        return RandomSpawningPosition;
    }
}
