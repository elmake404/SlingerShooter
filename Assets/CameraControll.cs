using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    normal,
    playerIsDead,
    cameraShake
}
public class CameraControll : MonoBehaviour
{
    public Transform cameraTransform;
    private CameraState cameraState;
    private Vector3 changedVector;
    private Vector3 velocity;
    private Vector3 savedForvard;
    private float maxSpeed = 20f;
    private float damping = 10f;
    public PlayerController playerController;
    private TargetSlingshotControl targetSlingshotControl;
    private bool isPlayerDead;
    private Rigidbody cameraRigidbody;
    private SphereCollider cameraCollider;
    private ShakeOffset newShake;

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

            case CameraState.cameraShake:
                CameraShake();
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
        cameraRigidbody.AddTorque(-transform.right + 3f*transform.forward, ForceMode.Impulse );
    }

    public void InitCameraShake()
    {
        cameraState = CameraState.cameraShake;
        newShake = new ShakeOffset(transform.rotation.eulerAngles, 0.5f);
    }

    private void CameraShake()
    {
        if (newShake.isEnd == true) 
        {
            cameraState = CameraState.normal;
            newShake = null;
            Debug.Log("NormalCamera");
            return;
        }
        Vector3 offset = newShake.GetSmoothedEulerAngles();
        transform.localRotation = Quaternion.Euler(offset);

    }

    private void RotateCamera()
    {
        Vector3 targetRay = targetSlingshotControl.directionRayTarget.direction;
        float xRotate = Mathf.Atan2(targetRay.x, transform.forward.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, xRotate / 3f, 0f);
    }
    
}
