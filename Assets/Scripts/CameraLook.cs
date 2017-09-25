using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float controllerSensX = 10f;
    public float controllerSensY = 10f;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;
    public GameObject gameCamera;
    private PlayerInfo pInfo;

    void Update()
    {
        if (pInfo.ControllerType == PlayerInfo.InputMethod.Keyboard)
            MouseLook();
        else
            ControllerLook();
    }

    void ControllerLook()
    {
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("ControllerLookX " + (int) pInfo.ControllerType) * controllerSensX;
        rotationY += Input.GetAxis("ControllerLookY " + (int) pInfo.ControllerType) * controllerSensY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        transform.localEulerAngles = new Vector3(0,rotationX,0);
        gameCamera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
    }

    void MouseLook()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(0, rotationX, 0);
            gameCamera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            gameCamera.transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    void Start()
    {
        pInfo = GetComponent<PlayerInfo>();
        gameCamera = GetComponentInChildren<Camera>().gameObject;
        
    }
}
