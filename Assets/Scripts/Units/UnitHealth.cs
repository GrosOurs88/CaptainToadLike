using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
    public Unit UnitScript;

    public int PV = 5;
    public GameObject CanvasHealth;
    public Slider CanvasHealthSlider;

    private void Start()
    {
        CanvasHealthSlider.value = 1.0f;
        HideHealthGauge();
    }

    public void DisplayHealthGauge()
    {
        CanvasHealth.SetActive(true);
    }

    public void HideHealthGauge()
    {
        CanvasHealth.SetActive(false);
    }

    public void TakeDamage()
    {
        CanvasHealthSlider.value -= (1 / PV);

        PV -= 1;

        UpdateHealthGaugeColour();

        if (PV <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthGaugeColour()
    {
        if (CanvasHealthSlider.value <= 0.3f)
        {
            CanvasHealthSlider.fillRect.GetComponent<Image>().color = Color.red;
        }

        else if (CanvasHealthSlider.value <= 0.7f)
        {
            CanvasHealthSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        }

        else
        {
            CanvasHealthSlider.fillRect.GetComponent<Image>().color = Color.green;
        }
    }

    public void Die()
    {
        SelectionSquare.Instance.availableUnitList.Remove(gameObject);

        if (SelectionSquare.Instance.selectedUnitList.Contains(gameObject))
        {
            SelectionSquare.Instance.selectedUnitList.Remove(gameObject);
        }

        Destroy(gameObject);
    }
}
