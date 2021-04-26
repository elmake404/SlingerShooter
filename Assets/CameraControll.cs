using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    normal,
    playerIsDead,
    //cameraShake
}
public class CameraControll : MonoBehaviour
{
    public Transform cameraTransform;
    private CameraState cameraState;
    private Vector3 changedVector;
    private Vector3 velocity;
    private Vector3 savedForvard;
    private float xRotate;
    private float yRotate;
    private float maxSpeed = 20f;
    private float damping = 10f;
    public PlayerController playerController;
    private TargetSlingshotControl targetSlingshotControl;
    private bool isPlayerDead;
    private Rigidbody cameraRigidbody;
    private SphereCollider cameraCollider;
    public bool isNowShaked = false;

    private void Start()
    {
        targetSlingshotControl = playerController.targetSlingshotControl;
        savedForvard = transform.forward;

        cameraRigidbody = GetComponent<Rigidbody>();
        cameraCollider = GetComponent<SphereCollider>();
    }

    private void LateUpdate()
    {
        CurrentCameraState(cameraState);
        
    }


    private void CurrentCameraState(CameraState cameraState)
    {
        switch (cameraState)
        {
            case CameraState.normal:
                RotateCamera();
                break;

            case CameraState.playerIsDead:
                break;

        }
    }

    public void InitStatePlayerDeath()
    {
        if (isPlayerDead == true) { return; }
        isPlayerDead = true;

        cameraState = CameraState.playerIsDead;

        cameraRigidbody.isKinematic = false;
        cameraRigidbody.useGravity = true;
        cameraCollider.enabled = true;

        //Vector3 randomVector = new Vector3(Random.Range(-10f,10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f))/20f;
        cameraRigidbody.AddTorque(-transform.right + 3f * transform.forward, ForceMode.Impulse );
    }


    private void RotateCamera()
    {
        Vector3 targetRay = targetSlingshotControl.directionRayTarget.direction;
        xRotate = Mathf.Atan2(targetRay.x, transform.forward.z) * Mathf.Rad2Deg;  
        yRotate = Mathf.Atan2(targetRay.y, transform.forward.z) * Mathf.Rad2Deg;  
        
        transform.localRotation = Quaternion.Euler(-yRotate / 1.8f, xRotate / 1.2f, 0f);
        
    }
    
}
