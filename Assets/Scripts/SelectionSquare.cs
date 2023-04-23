using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectionSquare : MonoBehaviour
{
    //Camera
    public Camera m_camera;

    //Rectangle de sélection
    public RectTransform selectionBox;

    //Layers
    public LayerMask layerMaskUnit, layerMaskTargetArrow;

    //Unités disponibles
    public List<GameObject> availableUnitList;

    //Unités sélectionnées
    public List<GameObject> selectedUnitList;

    //Position du rectangle de sélection au moment où on clique  
    private Vector2 startPos;

    private bool isDown = false;

    private bool IsACristalSelected = false;

    public static SelectionSquare Instance;

    public GameObject DebugSphere;

    private MovementManagerScriptableObject MovementData;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        MovementData = EnumTypes.Instance.MovementData;

        UIManager.Instance.UnitBaseAmount = 0;
        UIManager.Instance.UnitCarrierAmount = 0;
        UIManager.Instance.UnitFighterAmount = 0;

        //Cherche toutes les unité sur la scène et remplit availableWarriorList
        foreach (var unit in FindObjectsOfType<Unit>())
        {
            availableUnitList.Add(unit.gameObject);

            //Add units available at start to the units UI amounts
            switch (unit.unitType)
            {
                case EnumTypes.UnitTypes.unitbase:
                    UIManager.Instance.UnitBaseAmount += 1;
                    UIManager.Instance.UnitBaseAmountText.text = UIManager.Instance.UnitBaseAmount.ToString();
                    break;
                case EnumTypes.UnitTypes.unitcarrier:
                    UIManager.Instance.UnitCarrierAmount += 1;
                    UIManager.Instance.UnitCarrierAmountText.text = UIManager.Instance.UnitCarrierAmount.ToString();
                    break;
                case EnumTypes.UnitTypes.unitfighter:
                    UIManager.Instance.UnitFighterAmount += 1;
                    UIManager.Instance.UnitFighterAmountText.text = UIManager.Instance.UnitFighterAmount.ToString();
                    break;
            }
        }

        //Initialise selectedWarriorList
        selectedUnitList = new List<GameObject>();

        //Le rectangle de sélection ne doit pas être visible
        selectionBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Si on clique sur le bouton gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            // Déselectionne toutes les unités
            foreach (GameObject unit in selectedUnitList)
            {
                unit.GetComponent<Unit>().IsUnselected();
                unit.GetComponent<Unit>().HideHealthGauge();
            }

            //--------- Maybe unit.GetComponent<Unit>().HideHealthGauge(); ??

            //Vide le tableau de sélection
            selectedUnitList.Clear();

            //Position de la souris au moment du clique
            startPos = Input.mousePosition;

            isDown = true;

            //Fait apparaitre le rectangle de sélection
            selectionBox.gameObject.SetActive(true);

            // Simple clic
            Ray rayLeft = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitLeft;

            if (Physics.Raycast(rayLeft, out hitLeft, Mathf.Infinity, layerMaskUnit))
            {
                selectedUnitList.Add(hitLeft.collider.gameObject);

                foreach (GameObject unit in selectedUnitList)
                {
                    unit.GetComponent<Unit>().IsSelected();
                    unit.GetComponent<Unit>().DisplayHealthGauge();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            IsACristalSelected = false;

            Ray rayRight = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitRight;

            if (Physics.Raycast(rayRight, out hitRight, Mathf.Infinity))
            {
                if (hitRight.collider.CompareTag("Cristal"))
                {
                    IsACristalSelected = true;

                    foreach (GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<Unit>().AsACristalDepositTarget = true;
                        unit.GetComponent<Unit>().CristalDepositTarget = hitRight.collider.gameObject;
                    }

                    GetStaticMovementPosition(hitRight.point);

                    hitRight.point = hitRight.collider.transform.position;
                }

                if (hitRight.collider.CompareTag("Enemy"))
                {
                    foreach (GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<Unit>().EnemyTarget = hitRight.collider.gameObject;

                        if (unit.GetComponent<Unit>().CarryACristal)
                        {
                            unit.GetComponent<Unit>().CarryACristal = false;
                            unit.GetComponent<Unit>().CarriedCristal = null;
                            Destroy(unit.GetComponent<Unit>().CristalCarryPosition.GetChild(0).gameObject);
                        }
                    }

                    hitRight.point = hitRight.collider.transform.position;
                }

                if (hitRight.collider.CompareTag("Portal"))
                {
                    print("Portal selected");
                }

                NavMeshHit hit;

                if (NavMesh.SamplePosition(hitRight.point, out hit, 0.5f, NavMesh.AllAreas))
                {
                    if (IsACristalSelected)
                    {
                        //Give the same position to each unit
                        GetStaticMovementPosition(hitRight.point);
                    }

                    else
                    {
                        //Give position to the units according to the MovementData scriptable object
                        GetDynamicMovementPosition(hitRight.point);
                    }
                }

                GameObject newCLickTarget = Instantiate(EnumTypes.Instance.canvasClicktarget,
                                                        hitRight.point + new Vector3(0.0f, 0.1f, 0.0f),
                                                        EnumTypes.Instance.canvasClicktarget.GetComponent<RectTransform>().rotation);
            }

        }

        //Si on relâche le bouton gauche de la souris
        if (Input.GetMouseButtonUp(0))
        {
            isDown = false;

            //Fait disparaitre le rectangle de sélection
            selectionBox.gameObject.SetActive(false);
        }

        //Si on n'est pas en train d'appuyer le bouton, on return pour ne pas lire le reste du script
        if (!isDown)
            return;

        //Position de la souris cette frame
        Vector2 curMousePos = Input.mousePosition;

        //Position du rectangle de sélection
        selectionBox.anchoredPosition = startPos;

        //Calcule de la taille du rectangle de sélection
        // position actuelle de la souris - osition au moment du clique
        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        //Permettra de savoir si une unité rentre dans le rectangle de sélection
        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        //Parcours toutes les uniteés
        foreach (GameObject unit in availableUnitList)
        {
            //Convertit la position 3D de l'unité en position 2D sur l'écran
            Vector2 posVector2 = m_camera.WorldToScreenPoint(unit.transform.position);

            //Vérifie si l'unité est dans le rectangle
            if (CheckWarriorInBox(posVector2, bounds) && !(selectedUnitList.Contains(unit)))
            {
                selectedUnitList.Add(unit);

                //Si oui, on sélectionne l'unité
                unit.GetComponent<Unit>().IsSelected();
                unit.GetComponent<Unit>().DisplayHealthGauge();
            }
        }
    }

    //Fonction qui vérifie si l'unité est dans le rectangle
    private bool CheckWarriorInBox(Vector2 position, Bounds bounds)
    {
        return position.x > bounds.min.x
            && position.x < bounds.max.x
            && position.y > bounds.min.y
            && position.y < bounds.max.y;
    }

    // Randomize une position autour d'un point donné
    private Vector3 RandomPointOnXZCircle(float radius)
    {
        Vector2 randomPosition = Random.insideUnitCircle * radius;
        return new Vector3(randomPosition.x, 0f, randomPosition.y);
    }

    private void GetStaticMovementPosition(Vector3 center)
    {
        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            selectedUnitList[i].GetComponent<NavMeshAgent>().destination = center;
        }
    }

    private void GetDynamicMovementPosition(Vector3 center)
    {
        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            selectedUnitList[i].GetComponent<NavMeshAgent>().destination = center + MovementData.movementPoints[i];

            //Instantiate(DebugSphere, center + MovementData.movementPoints[i], Quaternion.identity); //--DEBUG
        }
    }
}