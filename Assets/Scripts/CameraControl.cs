using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public KeyCode Left, Right, Up, Down, RotLeft, RotRight;
    public Transform TerrainCenterPoint = null;
    public float CameraTranslationSensibility = 100f;
    public float CameraRotationSensibility = 100f;
    public float CameraFOVSensibility = 100f;
    private Camera Cam = null;
    private float CamRotX = 0.0f;
    public float DefaultCamFOV = 15.0f;
    public float MinCamFOV = 10.0f;
    public float MaxCamFOV = 35.0f;
    public bool useRotationKeys;
    public bool useTranslationWorldKeys;
    public Transform RotationCenterPoint = null;


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
                Cam.transform.RotateAround(TerrainCenterPoint.position, Vector3.up, CameraTranslationSensibility * Time.deltaTime);
            }
            if (Input.GetKey(Right))
            {
                Cam.transform.RotateAround(TerrainCenterPoint.position, Vector3.down, CameraTranslationSensibility * Time.deltaTime);
            }
            if (Input.GetKey(Up))
            {
                Cam.transform.RotateAround(TerrainCenterPoint.position, transform.right, CameraTranslationSensibility * Time.deltaTime);
            }
            if (Input.GetKey(Down))
            {
                Cam.transform.RotateAround(TerrainCenterPoint.position, -transform.right, CameraTranslationSensibility * Time.deltaTime);
            }
        }

        else if (useTranslationWorldKeys)
        {
            if (Input.GetKey(Left))
            {
                Cam.transform.Translate(Vector3.left * CameraTranslationSensibility * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(Right))
            {
                Cam.transform.Translate(Vector3.right * CameraTranslationSensibility * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(Up))
            {
                Cam.transform.Translate(transform.worldToLocalMatrix.MultiplyVector(Cam.transform.parent.transform.forward) * CameraTranslationSensibility * Time.deltaTime);
            }
            if (Input.GetKey(Down))
            {
                Cam.transform.Translate(transform.worldToLocalMatrix.MultiplyVector(-Cam.transform.parent.transform.forward) * CameraTranslationSensibility * Time.deltaTime);
            }
            if (Input.GetKey(RotLeft))
            {
                Cam.transform.parent.transform.RotateAround(RotationCenterPoint.position, Vector3.up, CameraRotationSensibility * Time.deltaTime);
            }
            if (Input.GetKey(RotRight))
            {
                Cam.transform.parent.transform.RotateAround(RotationCenterPoint.position, Vector3.down, CameraRotationSensibility * Time.deltaTime);
            }
        }

        CameraFOV();
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
