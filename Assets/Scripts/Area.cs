using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<GameObject> Targets = new List<GameObject>();
    [HideInInspector] public GameObject SelectedTarget = null;
    public float SelectedTargetScale = 1.0f;

    public GameObject TakeRandomTarget()
    {
        int rand = Random.Range(0, Targets.Count);

        return Targets[rand];
    }

    public void HideEveryTarget()
    {
        foreach(GameObject Target in Targets)
        {
            Target.SetActive(false);
        }
    }

    public void DisplaySelectedTarget()
    {
        SelectedTarget.SetActive(true);
        SelectedTarget.transform.localScale = new Vector3(SelectedTargetScale, SelectedTargetScale, SelectedTargetScale);
    }
}
