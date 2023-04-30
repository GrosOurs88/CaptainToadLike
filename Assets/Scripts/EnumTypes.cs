using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnumTypes : MonoBehaviour
{
    public static EnumTypes Instance;

    public enum CristalTypes { blue, yellow, red, Count };
    public enum PlayerTypes { player, enemy1, enemy2, enemy3, Count };
    public enum UnitTypes { unitbase, unitcarrier, unitfighter, Count };

    private void Awake()
    {
        Instance = this;
    }
}
