using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject SelectionImage;

    private void Start()
    {
        Unselect();
    }

    public void Select()
    {
        SelectionImage.SetActive(true);
    }

    public void Unselect()
    {
        SelectionImage.SetActive(false);
    }
}
