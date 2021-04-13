using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMoveState
{
    enemyRunToTarget = 0,
    enemyAvoidOther = 1,
    enemyFindPlayer = 2,
    enemyAttack = 3, 
    enemyDeath = 4,
    enemyIdle = 5
}

public class EnemyMove : MonoBehaviour
{
    [HideInInspector]public FlowField targetFlowField;
    [HideInInspector]public UsedCells ownUsedCells;
    private FlowField avoidFlowField;
    private EnemyAnimation enemyAnimation;
    [HideInInspector] public EnemyController enemyController;
    public EnemyActionCollider enemyActionCollider;
    private Cell currentCell;
    private Cell previousCell;
    public Cell targetCell;
    [HideInInspector] public CameraControll playerCamera;
    private int currentHash;
    private float percentOfDistance = 1f;
    private float minDistanceToTarget = 3f;
    private float minDistanceToOther = 8f;
    private float moveTowardsSpeed = 50f;
    private float rotateTowardsSpeed = 2f;
    private float currentVelocity = 0f;
    private Vector3 lastTransformPos;
    [HideInInspector]public EnemyMoveState moveState = EnemyMoveState.enemyRunToTarget;
    private Coroutine waitToNearObjects;
    bool isEnemyFindPlayer = false;
    bool isDeath = false;
    bool isEnemyToFight = false;


    private void OnEnable()
    {
        moveState = EnemyMoveState.enemyRunToTarget;
        lastTransformPos = Vector3.zero;
    }

    private void Start()
    {
        currentHash = gameObject.GetHashCode();
        enemyAnimation = GetComponent<EnemyAnimation>();
        waitToNearObjects = StartCoroutine(WaitToAvoidAnother());
        StartCoroutine(WaitToFindPlayer());
        StartCoroutine(WaitMoveStateEnemyDeath());
        StartCoroutine(WaitMoveStateFindPlayer());

    }

    private void FixedUpdate()
    {
        MoveState(moveState);   
    }

    private void MoveState(EnemyMoveState enemyMoveState)
    {
        
        switch (enemyMoveState)
        {
            case EnemyMoveState.enemyRunToTarget:
                CalculateDistanceToTarget();
                SetCurrentVelocity();
                SetCurrentCell(targetFlowField);
                WorkWithCellsToUsed();
                MoveEnemy();
                RotateEnemyInCellDirect();
                break;

            case EnemyMoveState.enemyAvoidOther:
                CalculateDistanceToTarget();
                SetCurrentVelocity();
                SetCurrentCell(avoidFlowField);
                WorkWithCellsToUsed();
                MoveEnemy();
                RotateEnemyInCellDirect();
                break;

            case EnemyMoveState.enemyFindPlayer:
                enemyAnimation.ChangeAnimationState(AnimatorEnemyState.walk);
                MoveEnemy();
                RotateEnemyToPlayer();
                break;

            case EnemyMoveState.enemyAttack:
                enemyAnimation.ChangeAnimationState(AnimatorEnemyState.attack);
                RotateEnemyToPlayer();
                break;
 
            case EnemyMoveState.enemyDeath:
                break;

            case EnemyMoveState.enemyIdle:
                RotateEnemyToPlayer();
                WaitToFight();

                break;
        }
    }

    private void MoveEnemy()
    {
        
        Vector3 startDirection = transform.forward;
        Vector3 targetPos = 2f*transform.forward;
        
        if (moveState == EnemyMoveState.enemyFindPlayer)
        {

            if (enemyController.EnemyIsOnFight(currentHash) != true)
            {
                    moveState = EnemyMoveState.enemyIdle;
                    enemyAnimation.ChangeAnimationState(AnimatorEnemyState.idle);
                    return;
            }

            enemyAnimation.ChangeAnimationState(AnimatorEnemyState.walk);
            float distanceToTarget = Vector3.Distance( new Vector3(playerCamera.transform.position.x,0f, playerCamera.transform.position.z), transform.position);
            percentOfDistance = Mathf.InverseLerp(2f, minDistanceToTarget, distanceToTarget);

            if (percentOfDistance < 0.3f)
            {
                enemyAnimation.ChangeAnimationState(AnimatorEnemyState.attack);
                percentOfDistance = 0f;
                moveState = EnemyMoveState.enemyAttack;
                enemyAnimation.ChangeAnimationState(AnimatorEnemyState.attack);
            }
        }

        transform.position += Vector3.MoveTowards(startDirection, targetPos, moveTowardsSpeed * Time.deltaTime) * percentOfDistance * Time.deltaTime;
        
    }  
    
    private void RotateEnemyInCellDirect()
    {
        Vector3 diretionToRotate = new Vector3(Mathf.MoveTowards(transform.forward.x ,currentCell.bestDirection.Vector.x, rotateTowardsSpeed * Time.deltaTime), transform.position.y,Mathf.MoveTowards (transform.forward.z, currentCell.bestDirection.Vector.y,rotateTowardsSpeed * Time.deltaTime));
        float angle = Mathf.Atan2(diretionToRotate.x, diretionToRotate.z) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, angle, 0f) ;
    }

    private void RotateEnemyToPlayer()
    {
        Vector3 direcToTarget = playerCamera.transform.position - transform.position;
        Vector3 diretionToRotate = new Vector3(Mathf.MoveTowards(transform.forward.x, direcToTarget.x, rotateTowardsSpeed * Time.deltaTime), transform.position.y, Mathf.MoveTowards(transform.forward.z, direcToTarget.z, rotateTowardsSpeed * Time.deltaTime));
        float angle = Mathf.Atan2(diretionToRotate.x, diretionToRotate.z) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void SetCurrentCell(FlowField selectedFlowField)
    {
        currentCell = selectedFlowField.GetCellFromWorldPos(transform.position);
    }

    private void WorkWithCellsToUsed()
    {
        if (previousCell != currentCell)
        {
            ownUsedCells.AddCellToUsed(currentCell);
            ownUsedCells.RemoveCellFromUsed(previousCell);
            previousCell = currentCell;
        }
    }

    private void SetCurrentVelocity()
    {
        float velocity = Vector3.Distance(Vector3.zero, (transform.position - lastTransformPos)) / Time.deltaTime;
        currentVelocity = velocity;
        lastTransformPos = transform.position;
    }

    private void CalculateDistanceToTarget()
    {
        float distance = Vector3.Distance(transform.position, targetFlowField.destinationCell.worldPos);
        
        if (distance < minDistanceToTarget)
        {
            isEnemyFindPlayer = true;

        }
    }

    private List<Cell> GetNearCells(out bool isAvoid)
    {
        isAvoid = false;
        List<Cell> cellToAvoid = new List<Cell>();
        if (ownUsedCells == null) { return null; }
        List<Cell> usedList = ownUsedCells.usedCells;

        for (int i = 0; i < usedList.Count; i++)
        {
            if (Vector3.Distance(transform.position, usedList[i].worldPos) < minDistanceToOther)
            {
                isAvoid = true;
                cellToAvoid.Add(usedList[i]);
            }
        }
        List<Cell> cellsToReturn = new List<Cell>();
        if (isAvoid == false)
        {
            return null;
        }
        else
        {
            return cellToAvoid;
            
        }
    }

    private void WaitToFight()
    {
        if (enemyController.EnemyIsOnFight(currentHash) == true)
        {
            moveState = EnemyMoveState.enemyFindPlayer;
            isEnemyToFight = true;
        }
    }

    private void GenerateToAvoidFlowField(List<Cell> cellToAvoid)
    {
        float cellRadius = targetFlowField.cellRadius;
        Vector2Int gridSize = targetFlowField.gridSize;
        Vector3 offsetGrid = targetFlowField.offsetGrid;

        avoidFlowField = new FlowField(cellRadius, gridSize, offsetGrid);
        avoidFlowField.CreateGrid();
        avoidFlowField.CreateCostField();
        avoidFlowField.ChangeCostFieldForAvoidOther(cellToAvoid);
        avoidFlowField.CreateIntegrationField(targetCell);
        avoidFlowField.CreateFlowField();
    }

    private IEnumerator WaitToFindPlayer()
    {
        
        yield return new WaitWhile(()=> isEnemyFindPlayer == false);
        StopCoroutine(waitToNearObjects);
        moveState = EnemyMoveState.enemyFindPlayer;
        enemyAnimation.ChangeAnimationState(AnimatorEnemyState.walk);
        
        yield return null;
    }

    private IEnumerator WaitToAvoidAnother()
    {
        bool isAvoid = false;
        List<Cell> nearCells = GetNearCells(out isAvoid);
        yield return new WaitWhile(()=> isAvoid == false);
        GenerateToAvoidFlowField(nearCells);
        moveState = EnemyMoveState.enemyAvoidOther;
        yield return new WaitForSeconds(3f);
        moveState = EnemyMoveState.enemyRunToTarget;
        waitToNearObjects = StartCoroutine(WaitToAvoidAnother());
       
        yield return null;
    }

    private IEnumerator WaitMoveStateFindPlayer()
    {
        yield return new WaitWhile(() => moveState != EnemyMoveState.enemyFindPlayer);
        enemyController.AddEnemyToFightQueue(currentHash);
        yield return null;
    }

    private IEnumerator WaitMoveStateEnemyDeath()
    {
        yield return new WaitWhile(() => moveState != EnemyMoveState.enemyDeath);
        enemyController.AddEnemyToDeadList();
        enemyController.RemoveKilledEnemyOnFight(currentHash);
        StopAllCoroutines();
        yield return null;
    }

    private void OnDestroy()
    {
        ownUsedCells.RemoveCellFromUsed(previousCell);
        ownUsedCells.RemoveCellFromUsed(currentCell);
        StopAllCoroutines();
    }
}
