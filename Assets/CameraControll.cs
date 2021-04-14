using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public Transform cameraTransform;
    private Vector3 changedVector;
    private Vector3 velocity;
    private Vector3 savedForvard;
    private float maxSpeed = 20f;
    private float damping = 10f;
    public PlayerController playerController;
    private TargetSlingshotControl targetSlingshotControl;

    private void Start()
    {
        targetSlingshotControl = playerController.targetSlingshotControl;
        savedForvard = transform.forward;
    }

    private void LateUpdate()
    {
        Vector3 targetRay = targetSlingshotControl.directionRayTarget.direction;
        float xRotate = Mathf.Atan2(targetRay.x, transform.forward.z) * Mathf.Rad2Deg;
        Debug.Log(xRotate);
        transform.rotation = Quaternion.Euler(0f, xRotate /3f, 0f);
        
    }
}
