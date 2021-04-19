using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentEnemyControl : MonoBehaviour
{
    public EnemyMove enemyMove;
    public Transform rigCharecter;
    public EnemyActionCollider playerActionCollider;
    private Rigidbody currentRigidbody;
    private EnemyAnimation enemyAnimation;
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;
    private int enemyLayerMask;
    private Vector3 spawnPos;
    private float explosionForce = 20f;
    private void Start()
    {
        spawnPos = transform.position;
        enemyAnimation = GetComponent<EnemyAnimation>();
        InitRagdoll();
        DisableRagdoll();
        enemyLayerMask = 1 << LayerMask.NameToLayer("EnemyRagdoll");
        currentRigidbody = transform.GetComponent<Rigidbody>();
        
    }

    private void InitRagdoll()
    {
        rigidbodies = rigCharecter.GetComponentsInChildren<Rigidbody>();
        colliders = rigCharecter.GetComponentsInChildren<Collider>();
        
    }

    public void DisableRagdoll()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
            rigidbodies[i].useGravity = false;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    public void EnableRagdoll()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
            rigidbodies[i].useGravity = true;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
    }

    public void InitDeathThisGuy(Ray directionToForce)
    {
            enemyAnimation.DisableAnimator();
            enemyMove.moveState = EnemyMoveState.enemyDeath;
            EnableRagdoll();
            playerActionCollider.DisableActionCollider();
            AddForceToTargetBodyPart(directionToForce);
            StartCoroutine(DisableThisEnemy());
    }

    public void InitDeathOnBarrelExplosion(Vector3 posExplosion)
    {
        enemyAnimation.DisableAnimator();
        enemyMove.moveState = EnemyMoveState.enemyDeath;
        EnableRagdoll();
        playerActionCollider.DisableActionCollider();
        AddExplosionForceToBody(posExplosion);
        StartCoroutine(DisableThisEnemy());
    }

    private void AddForceToTargetBodyPart(Ray directionToForce)
    {
        RaycastHit hit;
        if (Physics.Raycast(directionToForce, out hit, 50f, enemyLayerMask))
        {
            hit.collider.attachedRigidbody.AddForce(directionToForce.direction * 100f, ForceMode.Impulse);
        }
        

    }

    private void AddExplosionForceToBody(Vector3 posExplosion)
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].AddExplosionForce(explosionForce, posExplosion, 4f, 20f, ForceMode.Impulse);
        }
    }
    
    private IEnumerator DisableThisEnemy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
