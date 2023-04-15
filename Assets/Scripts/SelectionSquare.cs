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

    //Unit layer
    public int layerMask = 3;

    //Unit�s disponibles
    public List<GameObject> availableUnitList;

    //Unit�s s�lectionn�es
    public List<GameObject> selectedUnitList;

    //Position du rectangle de s�lection au moment o� on clique  
    private Vector2 startPos;

    private bool isDown = false;

   // ----- private NavMeshAgent SelectedAgent = null;

    private void Awake()
    {
        //Cherche toutes les unit� sur la sc�ne et remplit availableWarriorList
        foreach (var unit in FindObjectsOfType<Unit>())
        {
            availableUnitList.Add(unit.gameObject);
        }

        //Initialise selectedWarriorList
        selectedUnitList = new List<GameObject>();

        //Le rectangle de s�lection ne doit pas �tre visible
        selectionBox.gameObject.SetActive(false);

        layerMask = ~layerMask;
    }

    private void Update()
    {
        //Si on clique sur le bouton gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            // D�selectionne toutes les unit�s
            foreach (GameObject unit in selectedUnitList)
            {
                unit.GetComponent<Unit>().Unselect();
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

            if (Physics.Raycast(rayLeft, out hitLeft, 100.0f, layerMask))
            {
                selectedUnitList.Add(hitLeft.collider.gameObject);

                foreach(GameObject unit in selectedUnitList)
                {
                    unit.GetComponent<Unit>().Select();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray rayRight = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitRight;

            if (Physics.Raycast(rayRight, out hitRight, 100.0f))
            {
                NavMeshHit hit;

                if (NavMesh.SamplePosition(hitRight.point, out hit, 0.5f, NavMesh.AllAreas))
                {
                    foreach(GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<NavMeshAgent>().destination = hitRight.point;
                    }

                    // D�selectionne toutes les unit�s
                    foreach (GameObject unit in selectedUnitList)
                    {
                        unit.GetComponent<Unit>().Unselect();
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
                unit.GetComponent<Unit>().Select();
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
}