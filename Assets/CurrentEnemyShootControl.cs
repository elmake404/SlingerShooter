using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyShooterState
{
    isIdle,
    isShoot,
    isDead
}

public class CurrentEnemyShootControl : MonoBehaviour
{
    public GameObject spawnParticles;
    public GameObject bulletParticles;
    public GameObject enemyRig;
    public Collider actionCollider;
    public Transform pointShoot;
    [HideInInspector] public CameraControll playerCamera;
    private Vector3 pointArriveBullet;
    private AnimationControllerShooterEnemy animationController;
    private Collider[] enemyColliders;
    private Rigidbody[] enemyRigidbodies;
    private int enemyLayerMask;
    [HideInInspector] public bool isDead = false;

    private delegate void MoveBullet();
    private event MoveBullet ActionMoveBullet;


    void Start()
    {
        animationController = GetComponent<AnimationControllerShooterEnemy>();
        StartCoroutine(InitColliderAndRigidbodies());
        StartCoroutine(PeriodicShoot());
        EnableSpawnParticles();
        pointArriveBullet = playerCamera.transform.position;
        transform.rotation = Quaternion.Euler(0f,GetXAngleRotateToPlayer() + 90f, 0f);
        enemyLayerMask = 1 << LayerMask.NameToLayer("EnemyRagdoll");
    }

    private void Update()
    {
        ActionMoveBullet?.Invoke();
    }

    private void MakeShoot()
    {
        animationController.SetAnimation(AnimationStateEnemyShooter.isShoot);
        CreateBullet();
    }

    private void EnableSpawnParticles()
    {
        GameObject instance = Instantiate(spawnParticles);
        instance.transform.position = transform.position;
        StartCoroutine(DeleteSpawnParticles(instance));
    }

    private float GetXAngleRotateToPlayer()
    {
        Vector3 gettedDirection = GetDirectionToPlayer();
        //Vector3 target = new Vector3(gettedDirection.x, 0f, gettedDirection.z);
        Debug.DrawRay(transform.position, Vector3.forward, Color.black, Mathf.Infinity);
        float xRotate = Mathf.Atan2(transform.forward.z, gettedDirection.x) * Mathf.Rad2Deg;
        return xRotate;
    }

    private Vector3 GetDirectionToPlayer()
    {
        Vector3 cameraPos = playerCamera.transform.position;
        cameraPos.y = 0f;
        Vector3 enemyPos = transform.position;
        enemyPos.y = 0f;
        Vector3 returned = cameraPos - enemyPos;
        Debug.DrawRay(transform.position, returned, Color.red, Mathf.Infinity);
        returned = Vector3.Normalize(returned);
        return returned;
    }

    public void InitDeathThisGuy(Ray directionToForce)
    {
        isDead = true;
        animationController.DisableAnimator();
        //enemyMove.moveState = EnemyMoveState.enemyDeath;
        EnableRagdoll();
        actionCollider.enabled = false;
        AddForceToTargetBodyPart(directionToForce);
        StartCoroutine(DisableThisEnemy());
    }

    public void InitDeathThisGuyOnCrashBuild(Vector3 sourceExplosion)
    {
        isDead = true;
        animationController.DisableAnimator();
        //enemyMove.moveState = EnemyMoveState.enemyDeath;
        EnableRagdoll();
        actionCollider.enabled = false;
        AddExplosionForceToBody(sourceExplosion);
        StartCoroutine(DisableThisEnemy());
    }

    private IEnumerator DisableThisEnemy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        yield return null;
    }

    private void EnableRagdoll()
    {
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = true;
            if (enemyRigidbodies[i] != null)
            {
                enemyRigidbodies[i].isKinematic = false;
                enemyRigidbodies[i].useGravity = true;
            }
        }
    }

    private void AddForceToTargetBodyPart(Ray directionToForce)
    {
        RaycastHit hit;
        if (Physics.Raycast(directionToForce, out hit, 50f, enemyLayerMask))
        {
            hit.collider.attachedRigidbody.AddForce(directionToForce.direction * 100f, ForceMode.Impulse);
        }
    }

    private void AddExplosionForceToBody(Vector3 sourceExplosion)
    {
        for (int i = 0; i < enemyRigidbodies.Length; i++)
        {
            enemyRigidbodies[i].AddExplosionForce(20f, sourceExplosion, 20f, 20f, ForceMode.Impulse);
        }
    }

    private void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletParticles);
        bullet.transform.position = pointShoot.position;
        BulletControl bulletControl = bullet.GetComponent<BulletControl>();
        bulletControl.targetPoint = pointArriveBullet;
    }

    

    private IEnumerator DeleteSpawnParticles(GameObject instance)
    {
        float duration = instance.GetComponent<ParticleSystem>().main.duration;
        yield return new WaitForSeconds(duration);
        Destroy(instance);
        yield return null;
    }

    private IEnumerator InitColliderAndRigidbodies()
    {
        yield return new WaitForEndOfFrame();
        enemyColliders = enemyRig.GetComponentsInChildren<Collider>();
        enemyRigidbodies = enemyRig.GetComponentsInChildren<Rigidbody>();
        yield return null;
    }

    private IEnumerator PeriodicShoot()
    {
        yield return new WaitForSeconds(1f);
        while (isDead == false)
        {
            MakeShoot();
            
            yield return new WaitForSeconds(3f);
        }
        yield return null;
    }

}
