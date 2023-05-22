using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroup : MonoBehaviour
{
    [HideInInspector] public List<GameObject> Units = new List<GameObject>();
    
    void Start()
    {
        SetUnitsList();
    }

    private void SetUnitsList()
    {
        foreach(Transform child in transform)
        {
            Units.Add(child.gameObject);
        }
    }

    private void SelectUnitGroup()
    {

    }

}
