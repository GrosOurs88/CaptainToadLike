using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI CristalBlueText, CristalYellowText, CristalRedText;
    public int CristalBlueAmount, CristalYellowAmount, CristalRedAmount;
    private GameObject PlayerBase;
    public float UnitSpawnZOffset = -2.0f;
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
    [Header("Target Timer")]
    public GameObject PanelGlobalEvent = null;
    public Image ImageGlobalEvent = null;
    public float SliderGlobalEventTimer = 10.0f;

    private GameObject NewUnit = null;
    private Vector3 RandomSpawningPosition;

    private SelectionSquare selectionSquare;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        selectionSquare = SelectionSquare.Instance;
        PlayerBase = GameplayElementsManager.Instance.BasePlayerPrefab;
        SetCristalUI();
        //ResetCristalsUI();
        ResetUnitsUI();
        ResetSliderTarget();
    }

    private void Update()
    {

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

    private void RemoveCristals(int amountBlue, int amountYellow, int amountRed)
    {
        CristalBlueAmount -= amountBlue;
        CristalYellowAmount -= amountYellow;
        CristalRedAmount -= amountRed;

        CristalBlueText.text = CristalBlueAmount.ToString();
        CristalYellowText.text = CristalYellowAmount.ToString();
        CristalRedText.text = CristalRedAmount.ToString();
    }

    private void SetCristalUI()
    {
        CristalBlueText.text = CristalBlueAmount.ToString();
        CristalYellowText.text = CristalYellowAmount.ToString();
        CristalRedText.text = CristalRedAmount.ToString();
    }

    private void ResetCristalsUI()
    {
        CristalBlueAmount = 0;
        CristalYellowAmount = 0;
        CristalRedAmount = 0;

        CristalBlueText.text = CristalBlueAmount.ToString();
        CristalYellowText.text = CristalYellowAmount.ToString();
        CristalRedText.text = CristalRedAmount.ToString();
    }

    private void ResetUnitsUI()
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

    private void ResetSliderTarget()
    {
        ImageGlobalEvent.GetComponent<Image>().fillAmount = 0.0f;
    }

    public void CreateUnitBase()
    {
        if(CristalBlueAmount >= UnitBaseCristalBlueNeeded && CristalYellowAmount >= UnitBaseCristalYellowNeeded && CristalRedAmount >= UnitBaseCristalRedNeeded)
        {
            NewUnit = Instantiate(GameplayElementsManager.Instance.UnitBasePrefab,
                                  PlayerBase.transform.position + PlayerBase.transform.forward * UnitSpawnZOffset,
                                  Quaternion.identity);

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
            NewUnit = Instantiate(GameplayElementsManager.Instance.UnitCarrierPrefab,
                                  PlayerBase.transform.position + PlayerBase.transform.forward * UnitSpawnZOffset,
                                  Quaternion.identity);

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
            NewUnit = Instantiate(GameplayElementsManager.Instance.UnitFighterPrefab,
                                  PlayerBase.transform.position + PlayerBase.transform.forward * UnitSpawnZOffset,
                                  Quaternion.identity);

            SelectionSquare.Instance.availableUnitList.Add(NewUnit);

            RemoveCristals(UnitFighterCristalBlueNeeded, UnitFighterCristalYellowNeeded, UnitFighterCristalRedNeeded);

            UnitFighterAmount += 1;
            UnitFighterAmountText.text = UnitFighterAmount.ToString();
        }
    }

    private Vector3 RandomPositionInsideBaseArea(float radius)
    {
        RandomSpawningPosition = new Vector3 (Random.Range(-1.0f,1.0f) * radius, 0.0f, Random.Range(-1.0f, 1.0f) * radius);

        return RandomSpawningPosition;
    }
}
