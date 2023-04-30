using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    public void SetAnimationTrigger(string animTriggerName)
    {
        GetComponent<Animator>().SetTrigger(animTriggerName);
    }

    public void ResetAnimationTrigger(string animTriggerName)
    {
        GetComponent<Animator>().ResetTrigger(animTriggerName);
    }

    public void SetAnimationBool(string animBoolName, bool boolValue)
    {
        GetComponent<Animator>().SetBool(animBoolName, boolValue);
    }
}
