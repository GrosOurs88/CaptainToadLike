using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasClickTarget : MonoBehaviour
{
    public Image ClickTargetIcon;
    public float LifeTime = 1.0f;
    private float time = 0.0f;

    private void Update()
    {
        time += Time.deltaTime;

        if(time>= LifeTime)
        {
            Destroy(gameObject);
        }
    }
}
