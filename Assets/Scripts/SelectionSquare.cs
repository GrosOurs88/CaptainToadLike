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

    //Unit layer
    public int layerMask = 3;

    //Unités disponibles
    public List<GameObject> availableUnitList;

    //Unités sélectionnées
    public List<GameObject> selectedUnitList;

    //Position du rectangle de sélection au moment où on clique  
    private Vector2 startPos;

    private bool isDown = false;

    public static SelectionSquare Instance;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
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

        layerMask = ~layerMask;
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
            }

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

            if (Physics.Raycast(rayLeft, out hitLeft, 100.0f, layerMask))
            {
                selectedUnitList.Add(hitLeft.collider.gameObject);

                foreach(GameObject unit in selectedUnitList)
                {
                    unit.GetComponent<Unit>().IsSelected();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray rayRight = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitRight;

            if (Physics.Raycast(rayRight, out hitRight, 100.0f))
            {
                if(hitRight.collider.CompareTag("Cristal"))
                {
                    foreach (GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<Unit>().AsACristalDepositTarget = true;
                        unit.GetComponent<Unit>().CristalDepositTarget = hitRight.collider.gameObject;
                    }                    

                    hitRight.point = hitRight.collider.transform.position;
                }

                NavMeshHit hit;

                if (NavMesh.SamplePosition(hitRight.point, out hit, 0.5f, NavMesh.AllAreas))
                {
                    foreach(GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<NavMeshAgent>().destination = hitRight.point;

                      //  unit.GetComponent<Unit>().SetAnimationTrigger("IsMoving");
                        unit.GetComponent<Unit>().IsUnselected();
                    }

                    //Vide le tableau de sélection
                    selectedUnitList.Clear();
                }
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
}