using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public KeyCode Left, Right, Up, Down;
    public Transform TerrainCenterPoint = null;
    public float CameraSensibility = 100f;
    public float CameraFOVSensibility = 100f;
    private Camera Cam = null;
    private float CamRotX = 0.0f;
    public float DefaultCamFOV = 15.0f;
    public float MinCamFOV = 10.0f;
    public float MaxCamFOV = 35.0f;
    public bool useRotationKeys;
    public bool useTranslationWorldKeys;


    private void Start()
    {
        Cam = GetComponent<Camera>();
        Cam.fieldOfView = DefaultCamFOV;
    }

    private void Update()
    {
        if (useRotationKeys)
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
        }

        else if (useTranslationWorldKeys)
        {
            if (Input.GetKey(Left))
            {
                Cam.transform.Translate(Vector3.left * CameraSensibility * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(Right))
            {
                Cam.transform.Translate(Vector3.right * CameraSensibility * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(Up))
            {
                Cam.transform.Translate(Vector3.forward * CameraSensibility * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(Down))
            {
                Cam.transform.Translate(Vector3.back * CameraSensibility * Time.deltaTime, Space.World);
            }
        }

        CameraFOV();
        // CameraHeight();
    }

    private void CameraFOV()
    {
        Cam.fieldOfView -= Input.mouseScrollDelta.y * CameraFOVSensibility * Time.deltaTime;
        Cam.fieldOfView = Mathf.Clamp(Cam.fieldOfView, MinCamFOV, MaxCamFOV);
    }

    //public void CameraHeight()
    //{
    //    Vector3 pos = Cam.transform.position;
    //    pos.y += Input.mouseScrollDelta.y * CameraHeightSensibility * Time.deltaTime;
    //    Cam.transform.position = pos;
    //}
}
