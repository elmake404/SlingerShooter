using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [HideInInspector]public FlowField targetFlowField;
    private Cell currentCell;
    private float minDistance = 1f;
    private float moveTowardsSpeed = 50f;
    private float rotateTowardsSpeed = 2f;
    private float currentVelocity = 0f;
    private Vector3 lastTransformPos;
    

    private void OnEnable()
    {
        lastTransformPos = Vector3.zero;
    }

    private void FixedUpdate()
    {
        SetCurrentVelocity();
        SetCurrentCell();
        MoveEnemy();
        RotateEnemy();
    }

    private void Update()
    {

        //transform.position += moveVelocity*Time.deltaTime;
    }

    private void MoveEnemy()
    {
        Vector3 startDirection = transform.forward;
        Vector3 targetPos = 2f*transform.forward;
        float percentOfDistance = 1f;
        float distanceToTarget = Vector3.Distance(transform.position, targetFlowField.destinationCell.worldPos);
        
        if (distanceToTarget < minDistance + 1f)
        {
            percentOfDistance = Mathf.InverseLerp(minDistance, minDistance + 1f, distanceToTarget);
        }
        transform.position += Vector3.MoveTowards(startDirection, targetPos, moveTowardsSpeed * Time.deltaTime) * percentOfDistance*Time.deltaTime;
    }   

    private void RotateEnemy()
    {
        Vector3 diretionToRotate = new Vector3(Mathf.MoveTowards(transform.forward.x ,currentCell.bestDirection.Vector.x, rotateTowardsSpeed * Time.deltaTime), transform.position.y,Mathf.MoveTowards (transform.forward.z, currentCell.bestDirection.Vector.y,rotateTowardsSpeed * Time.deltaTime));
        float angle = Mathf.Atan2(diretionToRotate.x, diretionToRotate.z) * Mathf.Rad2Deg;
        
        transform.localRotation = Quaternion.Euler(0f, angle, 0f) ;
        
    }

    private void SetCurrentCell()
    {
        currentCell = targetFlowField.GetCellFromWorldPos(transform.position);
    }

    private void SetCurrentVelocity()
    {
        float velocity = Vector3.Distance(Vector3.zero, (transform.position - lastTransformPos)) / Time.deltaTime;
        currentVelocity = velocity;
        Debug.Log(velocity);
        lastTransformPos = transform.position;
        
    }
}
