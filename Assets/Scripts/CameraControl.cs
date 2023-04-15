using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public KeyCode Left, Right, Up, Down;
    public Transform TerrainCenterPoint = null;
    public float CameraSensibility = 100f;
    public float CameraHeightSensibility = 100f;
    private Camera Cam = null;
    private float CamRotX = 0.0f;
    public float MinCamRotX = 0.0f;
    public float MaxCamRotX = 80.0f;

    void Start()
    {
        Cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKey(Left))
        {
            Cam.transform.RotateAround(TerrainCenterPoint.position, Vector3.up, CameraSensibility * Time.deltaTime);
        }
        if (Input.GetKey(Right))
        {
            Cam.transform.RotateAround(TerrainCenterPoint.position, Vector3.down, CameraSensibility * Time.deltaTime);
        }
        if (Input.GetKey(Up))
        {
            Cam.transform.RotateAround(TerrainCenterPoint.position, transform.right, CameraSensibility * Time.deltaTime);
        }
        if (Input.GetKey(Down))
        {
            Cam.transform.RotateAround(TerrainCenterPoint.position, -transform.right, CameraSensibility * Time.deltaTime);
        }

        CameraHeight();
    }

    public void CameraHeight()
    {
        Vector3 pos = Cam.transform.position;
        pos.y += Input.mouseScrollDelta.y * CameraHeightSensibility * Time.deltaTime;
        Cam.transform.position = pos;
    }
}
