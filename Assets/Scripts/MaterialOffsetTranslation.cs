using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffsetTranslation : MonoBehaviour
{
    public float X, Z;
    
    void Update()
    {
        //GetComponent<MeshRenderer>().material.SetFloat("Tiling")
        GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(X, Z) * Time.deltaTime;
    }
}
