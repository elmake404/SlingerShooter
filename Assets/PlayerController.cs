using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SlingshotControl slingshotControl;
    public TargetSlingshotControl targetSlingshotControl;
    public AnimationSlingControl animationSlingControl;
    [HideInInspector] public Camera mainCamera;
    private LayerMask enemyLayerMask;
    private GameObject currentTargetedEnemy;
    //private bool isPlayerAiming = false;

    private void Awake()
    {
        mainCamera = FindObjectOfType<CameraControll>().transform.GetComponent<Camera>();
        targetSlingshotControl.sceneCamera = mainCamera;
        slingshotControl.targetControl = targetSlingshotControl;

        //slingshotControl.sceneCamera = mainCamera;
    }

    private void Start()
    {
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
        Debug.Log(enemyLayerMask);
    }

    private void Update()
    {
        SetStateTargetSlingshotControl();
        SetEnemyOnTargetIfItIs(targetSlingshotControl.directionRayTarget);
        //Debug.Log(currentTargetedEnemy);
    }


    private void FixedUpdate()
    {
        SetStateSlingshotControl();
    }

    private void SetEnemyOnTargetIfItIs(Ray ray)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            if (raycastHit.transform.gameObject.layer == enemyLayerMask)
            {
                currentTargetedEnemy = raycastHit.transform.gameObject;
            }
            else
            {
                currentTargetedEnemy = null;
            }
        }
        else
        {
            currentTargetedEnemy = null;
        }

    }

    public void ShootOnEnemy()
    {
        if (currentTargetedEnemy == null) { return; }
        //Debug.Log(currentTargetedEnemy.transform.name);
        CurrentEnemyControl currentEnemyControl = currentTargetedEnemy.transform.GetComponent<CurrentEnemyControl>();
        currentEnemyControl.InitDeathThisGuy(targetSlingshotControl.directionRayTarget);
    }

    private void SetStateTargetSlingshotControl()
    {
        if (ScreenControl.inputVelocity > 0f)
        {
            animationSlingControl.SetAnimationPuling();
            if (currentTargetedEnemy != null)
            {
                targetSlingshotControl.controlState = TargetControlState.targetIsOnEnemy;
            }
            else
            {
                targetSlingshotControl.controlState = TargetControlState.targetIsActive;
            }
        }
        else
        {
            animationSlingControl.SetAnimationShot();
            targetSlingshotControl.controlState = TargetControlState.targetIsNonActive;
        }
    }

    private void SetStateSlingshotControl()
    {
        if (ScreenControl.inputVelocity > 0f)
        {
            slingshotControl.slinghsotState = SlinghsotState.slingshotIsActive;
        }
        else
        {
            slingshotControl.slinghsotState = SlinghsotState.slingshotIsNonActive;
        }
    }
}
