using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementManager : MonoBehaviour
{
    public static UnitMovementManager Instance;

    public MovementManagerScriptableObject MovementData;

    private void Awake()
    {
        Instance = this;
    }
}
