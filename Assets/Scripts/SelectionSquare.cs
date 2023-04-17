using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectionSquare : MonoBehaviour
{
    //Camera
    public Camera m_camera;

    //Rectangle de s�lection
    public RectTransform selectionBox;

    //Layers
    public int layerMaskUnit = 3;

    //Unit�s disponibles
    public List<GameObject> availableUnitList;

    //Unit�s s�lectionn�es
    public List<GameObject> selectedUnitList;

    //Position du rectangle de s�lection au moment o� on clique  
    private Vector2 startPos;

    private bool isDown = false;

    public static SelectionSquare Instance;

    public GameObject DebugSphere;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        UIManager.Instance.UnitBaseAmount = 0;
        UIManager.Instance.UnitCarrierAmount = 0;
        UIManager.Instance.UnitFighterAmount = 0;

        //Cherche toutes les unit� sur la sc�ne et remplit availableWarriorList
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

        //Le rectangle de s�lection ne doit pas �tre visible
        selectionBox.gameObject.SetActive(false);

        layerMaskUnit = ~layerMaskUnit;
    }

    private void Update()
    {
        //Si on clique sur le bouton gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            // D�selectionne toutes les unit�s
            foreach (GameObject unit in selectedUnitList)
            {
                unit.GetComponent<Unit>().IsUnselected();
            }

            //Vide le tableau de s�lection
            selectedUnitList.Clear();

            //Position de la souris au moment du clique
            startPos = Input.mousePosition;

            isDown = true;

            //Fait apparaitre le rectangle de s�lection
            selectionBox.gameObject.SetActive(true);

            // Simple clic
            Ray rayLeft = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitLeft;

            if (Physics.Raycast(rayLeft, out hitLeft, Mathf.Infinity, layerMaskUnit))
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

            if (Physics.Raycast(rayRight, out hitRight, Mathf.Infinity))
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

                if (hitRight.collider.CompareTag("Enemy"))
                {
                    foreach (GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<Unit>().EnemyTarget = hitRight.collider.gameObject;
                    }

                    hitRight.point = hitRight.collider.transform.position;
                }

                if (hitRight.collider.CompareTag("Portal"))
                {
                    print("spoueeet");

                    //hitRight.point = hitRight.collider.transform.position;                   

                    //Override le NavMesh.SamplePosition ci-dessous, sinon �a ne marche pas quand on clique sur un portail :/
                    foreach (GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<NavMeshAgent>().destination = hitRight.collider.transform.position + RandomPointOnXZCircle(0.25f * selectedUnitList.Count);

                        //Instantiate(DebugSphere, unit.GetComponent<NavMeshAgent>().destination, Quaternion.identity);
                    }
                }

                NavMeshHit hit;

                if (NavMesh.SamplePosition(hitRight.point, out hit, 0.5f, NavMesh.AllAreas))
                {
                    foreach(GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<NavMeshAgent>().destination = hitRight.point;

                        // Augmente le rayon de la target en fonction du nombre d'unit�s s�lectionn�es (marche pas dans les pentes)
                        // unit.GetComponent<NavMeshAgent>().destination = RandomPointOnXZCircle(hitRight.point, 0.25f * selectedUnitList.Count);

                        //Instantiate(DebugSphere, unit.GetComponent<NavMeshAgent>().destination, Quaternion.identity);

                        unit.GetComponent<Unit>().IsUnselected();
                    }

                    //Vide le tableau de s�lection
                    selectedUnitList.Clear();
                }
            }
        }

        //Si on rel�che le bouton gauche de la souris
        if (Input.GetMouseButtonUp(0))
        {
            isDown = false;

            //Fait disparaitre le rectangle de s�lection
            selectionBox.gameObject.SetActive(false);
        }

        //Si on n'est pas en train d'appuyer le bouton, on return pour ne pas lire le reste du script
        if (!isDown)
            return;

        //Position de la souris cette frame
        Vector2 curMousePos = Input.mousePosition;

        //Position du rectangle de s�lection
        selectionBox.anchoredPosition = startPos;

        //Calcule de la taille du rectangle de s�lection
        // position actuelle de la souris - osition au moment du clique
        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        //Permettra de savoir si une unit� rentre dans le rectangle de s�lection
        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        //Parcours toutes les unite�s
        foreach (GameObject unit in availableUnitList)
        {
            //Convertit la position 3D de l'unit� en position 2D sur l'�cran
            Vector2 posVector2 = m_camera.WorldToScreenPoint(unit.transform.position);

            //V�rifie si l'unit� est dans le rectangle
            if (CheckWarriorInBox(posVector2, bounds) && !(selectedUnitList.Contains(unit)))
            {
                selectedUnitList.Add(unit);

                //Si oui, on s�lectionne l'unit�
                unit.GetComponent<Unit>().IsSelected();
            }
        }
    }

    //Fonction qui v�rifie si l'unit� est dans le rectangle
    private bool CheckWarriorInBox(Vector2 position, Bounds bounds)
    {
        return position.x > bounds.min.x
            && position.x < bounds.max.x
            && position.y > bounds.min.y
            && position.y < bounds.max.y;
    }

    // Randomize une position autour d'un point donn�
    private Vector3 RandomPointOnXZCircle(float radius)
    {
        Vector2 randomPosition = Random.insideUnitCircle * radius;
        return new Vector3(randomPosition.x, 0f, randomPosition.y);
    }
}