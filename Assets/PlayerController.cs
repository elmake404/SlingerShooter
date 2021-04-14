using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    playerIsShooting,
    playerIsMove,
    playerIsDead
}

public class PlayerController : MonoBehaviour
{
    public SlingshotControl slingshotControl;
    public TargetSlingshotControl targetSlingshotControl;
    public AnimationSlingControl animationSlingControl;
    [HideInInspector] public PlayerState playerState;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public CatmulSpline currentSpline;
    private LayerMask enemyLayer;
    private LayerMask impossibleLayer;
    private GameObject currentTargetedEnemy;
    private GameObject currentTargetedBarrel;
    private float travelLength;
    private float velocityMove;
    private float damping = 12f;
    private float maxSpeed = 1f;
    private bool isTargetOnEnemy = false;
    //private bool isPlayerAiming = false;

    private void Awake()
    {
        mainCamera = FindObjectOfType<CameraControll>().transform.GetComponent<Camera>();
        targetSlingshotControl.sceneCamera = mainCamera;
        slingshotControl.targetControl = targetSlingshotControl;

    }

    private void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        impossibleLayer = LayerMask.NameToLayer("Impossible");
        playerState = PlayerState.playerIsShooting;
        Debug.Log(enemyLayer);
    }

    private void Update()
    {
        CurrentState(playerState);
    }


    private void FixedUpdate()
    {
        SetStateSlingshotControl();
    }

    private void CurrentState(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.playerIsShooting:
                SetStateTargetSlingshotControl();
                SetEnemyOnTargetIfItIs(targetSlingshotControl.directionRayTarget);
                break;
            case PlayerState.playerIsMove:
                MovePlayerToNextPlatform();
                break;
            case PlayerState.playerIsDead:
                break;
        }
    }

    private void SetEnemyOnTargetIfItIs(Ray ray)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            int raycastedLayer = raycastHit.transform.gameObject.layer;
            if (raycastedLayer == enemyLayer)
            {
                currentTargetedEnemy = raycastHit.transform.gameObject;
                isTargetOnEnemy = true;
            }
            else if (raycastedLayer == impossibleLayer)
            {
                if(raycastHit.transform.tag == "barrel")
                {
                    //raycastHit.transform.GetComponent<BarrelExplosionControl>().MakeBarrelExplosion();
                    SetTargetedBarrel(raycastHit.transform.gameObject);
                    isTargetOnEnemy = true;
                }
            }
            else
            {
                isTargetOnEnemy = false;
                currentTargetedBarrel = null;
                currentTargetedEnemy = null;
            }
        }
        else
        {
            isTargetOnEnemy = false;
            currentTargetedBarrel = null;
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

    public void SetTargetedBarrel(GameObject barrelToSet)
    {
        if (currentTargetedBarrel != null) { return; }
        else
        {
            currentTargetedBarrel = barrelToSet;
        }
        
    }

    public void ShootOnBarrel()
    {
        if (currentTargetedBarrel == null) { return; }
        currentTargetedBarrel.GetComponent<BarrelExplosionControl>().MakeBarrelExplosion();

    }

    public void StartMovePlayer()
    {
        StartCoroutine(DelayStartMovePlayer());
    }

    private void SetStateTargetSlingshotControl()
    {
        if (ScreenControl.inputVelocity > 0f)
        {
            animationSlingControl.SetAnimationPuling();
            if (isTargetOnEnemy != false)
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

    private IEnumerator DelayStartMovePlayer()
    {
        yield return new WaitForSeconds(0.5f);
        playerState = PlayerState.playerIsMove;
        yield return null;
    }

    private void MovePlayerToNextPlatform()
    {
        velocityMove = Mathf.Clamp(velocityMove, 0f, maxSpeed);
        travelLength += Time.deltaTime * velocityMove;
        if (travelLength >= currentSpline.currentLength - 0.05f)
        {
            playerState = PlayerState.playerIsShooting;
            return;
        }
        
        float n1 = velocityMove - (travelLength - currentSpline.currentLength) * damping * damping * Time.deltaTime;
        float n2 = 1 + damping * Time.deltaTime;
        velocityMove = n1 / (n2 * n2);
        travelLength = Mathf.Clamp(travelLength, 0f, currentSpline.currentLength);
        Vector3 gettedPos = currentSpline.GetSplinePoint(travelLength);
        //Debug.Log(travelLength);
        mainCamera.transform.position = new Vector3(gettedPos.x, mainCamera.transform.position.y, gettedPos.z);
    }
}
