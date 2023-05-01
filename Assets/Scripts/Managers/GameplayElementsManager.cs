using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayElementsManager : MonoBehaviour
{
    public static GameplayElementsManager Instance;

    public GameObject CristalBluePrefab, CristalYellowPrefab, CristalRedPrefab;
    public GameObject BasePlayerPrefab, BaseEnemyPrefab;
    public GameObject CanvasCristalPrefab, CanvasClicktargetPrefab;
    public GameObject UnitBasePrefab, UnitCarrierPrefab, UnitFighterPrefab;
    public GameObject EnemySmallPrefab, EnemyMediumPrefab, EnemyBigPrefab;
    public GameObject BulletPrefab;

    public Color CristalBlueColor, CristalYellowColor, CristalRedColor;

    private void Awake()
    {
        Instance = this;
    }
}
