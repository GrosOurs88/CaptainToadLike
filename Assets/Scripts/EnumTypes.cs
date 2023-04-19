using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnumTypes : MonoBehaviour
{
    public static EnumTypes Instance;

    public MovementManagerScriptableObject MovementData;

    public enum CristalTypes { blue, yellow, red, Count };

    public GameObject CristalBluePrefab, CristalYellowPrefab, CristalRedPrefab;

    public enum PlayerTypes { player, enemy1, enemy2, enemy3, Count };

    public GameObject BasePlayer, BaseEnemy1, BaseEnemy2, BaseEnemy3;

    public enum UnitTypes { unitbase, unitcarrier, unitfighter, Count };

    public GameObject CanvasCristalPrefab;

    public Color CristalBlueColor, CristalYellowColor, CristalRedColor;

    public GameObject UnitBasePrefab, UnitCarrierPrefab, UnitFighterPrefab;

    public GameObject EnemySmallPrefab, EnemyMediumPrefab, EnemyBigPrefab;

    public GameObject BulletPrefab;

    private void Awake()
    {
        Instance = this;
    }
}
