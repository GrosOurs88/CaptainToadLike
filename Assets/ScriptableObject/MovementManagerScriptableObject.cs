using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementManagerScriptableObject", order = 1)]
public class MovementManagerScriptableObject : ScriptableObject
{
    public Vector3[] movementPoints;
}