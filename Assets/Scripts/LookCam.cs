using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCam : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        // Find the main camera in the scene
        mainCameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // Make the UI element look at the camera
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
            mainCameraTransform.rotation * Vector3.up);
    }
}
