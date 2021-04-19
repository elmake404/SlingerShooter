using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingOn
{
    enemy,
    enemyShooter,
    barrel,
    empty
}

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
    public GameObject projectileBullet;
    public bool isPlayerDead = false;
    [HideInInspector] public PlayerState playerState;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public CatmulSpline currentSpline;
    private LayerMask enemyLayer;
    private LayerMask enemyShooterLayer;
    private LayerMask impossibleLayer;
    private GameObject currentTargetedObject;
    private float travelLength;
    private float velocityMove;
    private float damping = 12f;
    private float maxSpeed = 1f;
    private bool isTargetOnEnemy = false;
    private List<Projectile> projectileList;
    private Delegate[] shootingsVariantsList;
    private Collider cameraCollider;
    private Rigidbody cameraRigidbody;
    private int numOfMaxEnemyHits = 5;
    private DamageHits damageHits;
    private CameraControll cameraControll;
    public ShootingOn shootingOn = ShootingOn.empty;

    private delegate void ShootingVariants();
    private event ShootingVariants EventShootingVariants;
    //private bool isPlayerAiming = false;

    private void Awake()
    {
        cameraControll = FindObjectOfType<CameraControll>();
        mainCamera = cameraControll.transform.GetComponent<Camera>();
        targetSlingshotControl.sceneCamera = mainCamera;
        slingshotControl.targetControl = targetSlingshotControl;

    }

    private void Start()
    {
        damageHits = new DamageHits(cameraControll, maxHits: 5);

        EventShootingVariants += ShootOnEnemy;
        EventShootingVariants += ShootOnEnemyShooter;
        EventShootingVariants += ShootOnBarrel;

        shootingsVariantsList = EventShootingVariants.GetInvocationList();

        projectileList = new List<Projectile>();

        cameraCollider = mainCamera.transform.GetComponent<SphereCollider>();
        cameraRigidbody = mainCamera.transform.GetComponent<Rigidbody>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enemyShooterLayer = LayerMask.NameToLayer("EnemyShooter");
        impossibleLayer = LayerMask.NameToLayer("Impossible");
        playerState = PlayerState.playerIsShooting;
    }


    private void Update()
    {
        CurrentState(playerState);
    }


    private void FixedUpdate()
    {
        SetStateSlingshotControl();
        ShootedMoveProjectile();
        CheckNumsOfHitDamage();
        //Debug.Log(damageHits.numOfCurrentDamageHits);
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

    public void CheckNumsOfHitDamage()
    {
        if (numOfMaxEnemyHits < damageHits.numOfCurrentDamageHits) { InitPlayerDeath(); }
    }

    private void InitPlayerDeath()
    {
        if (isPlayerDead == true) { return; }
        playerState = PlayerState.playerIsDead;
        cameraControll.InitStatePlayerDeath();
    }



    public void InvokeCurrentShootVariat()
    {
        if (shootingOn == ShootingOn.empty) { return; }
        shootingsVariantsList[(int)shootingOn].DynamicInvoke();
    }

    private void SetEnemyOnTargetIfItIs(Ray ray)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            int raycastedLayer = raycastHit.transform.gameObject.layer;
            if (raycastedLayer == enemyLayer)
            {
                currentTargetedObject = raycastHit.transform.gameObject;
                shootingOn = ShootingOn.enemy;
                isTargetOnEnemy = true;
            }

            else if (raycastedLayer == enemyShooterLayer)
            {
                isTargetOnEnemy = true;
                shootingOn = ShootingOn.enemyShooter;
                currentTargetedObject = raycastHit.transform.gameObject;
            }

            else if (raycastedLayer == impossibleLayer)
            {
                if (raycastHit.transform.tag == "barrel")
                {
                    shootingOn = ShootingOn.barrel;
                    currentTargetedObject = raycastHit.transform.gameObject;
                    isTargetOnEnemy = true;
                }
            }
            else
            {
                isTargetOnEnemy = false;
                shootingOn = ShootingOn.empty;
                currentTargetedObject = null;
            }
        }
        else
        {
            shootingOn = ShootingOn.empty;
            isTargetOnEnemy = false;
            currentTargetedObject = null;
        }

    }



    private void ShootOnEnemy()
    {
        if (currentTargetedObject == null) { return; }
        //Debug.Log(currentTargetedEnemy.transform.name);
        CurrentEnemyControl currentEnemyControl = currentTargetedObject.transform.GetComponent<CurrentEnemyControl>();
        currentEnemyControl.InitDeathThisGuy(targetSlingshotControl.directionRayTarget);
    }

    private void ShootOnEnemyShooter()
    {
        if (currentTargetedObject == null) { return; }
        CurrentEnemyShootControl currentEnemyShootControl = currentTargetedObject.transform.parent.GetComponent<CurrentEnemyShootControl>();
        currentEnemyShootControl.InitDeathThisGuy(targetSlingshotControl.directionRayTarget);

    }

    private void ShootOnBarrel()
    {
        if (currentTargetedObject == null) { return; }
        currentTargetedObject.GetComponent<BarrelExplosionControl>().MakeBarrelExplosion();

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
        if (travelLength >= currentSpline.currentLength - 0.01f)
        {
            playerState = PlayerState.playerIsShooting;
            travelLength = 0f;
            velocityMove = 0f;
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

    public void MakeProjectile()
    {
        Vector3 destinationPoint = targetSlingshotControl.directionRayTarget.origin + 20f * targetSlingshotControl.directionRayTarget.direction;
        Vector3 mainCameraForward = mainCamera.transform.forward + targetSlingshotControl.directionRayTarget.direction;
        Vector3 startPoint = mainCamera.transform.position + mainCameraForward;
        startPoint.y -= 1f;
        
        GameObject instance = Instantiate(projectileBullet);
        instance.transform.position = startPoint;
        Projectile newProjectile = new Projectile(instance, destinationPoint);
        projectileList.Add(newProjectile);
    }

    private void ShootedMoveProjectile()
    {
        if (projectileList.Count != 0)
        {
            for (int i = 0; i < projectileList.Count; i++)
            {
                //Vector3 offsetedPos = Vector3.MoveTowards(projectileList[i].projectile.transform.position, projectileList[i].destinationPoint, 50f * Time.deltaTime);
                projectileList[i].projectile.transform.position = Vector3.MoveTowards(projectileList[i].projectile.transform.position, projectileList[i].destinationPoint,100f* Time.deltaTime);
                if (Vector3.Distance(projectileList[i].projectile.transform.position, projectileList[i].destinationPoint) < 0.1f)
                {
                    DestroyProjectileBullet(i);
                }
            }
        }
    }

    private void DestroyProjectileBullet(int index)
    {
        Destroy(projectileList[index].projectile);
        projectileList.RemoveAt(index);
    }
}


struct Projectile
{
    public GameObject projectile;
    public Vector3 destinationPoint;

    public Projectile(GameObject projectile, Vector3 destination)
    {
        this.projectile = projectile;
        this.destinationPoint = destination;
    }
}

public class DamageHits
{
    private int maxNumOfHits;
    public int numOfCurrentDamageHits;
    public static DamageHits instance;
    private CameraControll playerCamera;

    public DamageHits(CameraControll setPlayerCamera, int maxHits)
    {
        instance = this;
        playerCamera = setPlayerCamera;
        maxNumOfHits = maxHits;
    }

    public void AddHit()
    {
        if (numOfCurrentDamageHits > maxNumOfHits) { return; }
        numOfCurrentDamageHits += 1;
        playerCamera.InitCameraShake();
    }
}