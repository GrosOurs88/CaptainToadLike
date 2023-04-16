using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCristal : MonoBehaviour
{
    public Image CristalIcon;

    private void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CanvasCristal_Idle"))
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
