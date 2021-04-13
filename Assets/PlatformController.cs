﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public List<Transform> pointsToSpawn;
    public GridController gridController;
    public EnemyController enemyController;
    public CatmulSpline currentSpline;
    private Queue<Transform> queueSpawnPoints;
    [HideInInspector] public ChangePlatformManager changePlatformManager;
    [HideInInspector] public CameraControll playerCamera;
    [HideInInspector] public FlowField currentFlowField;

    private void Start()
    {
        playerCamera = FindObjectOfType<CameraControll>();
        queueSpawnPoints = new Queue<Transform>();
        FillQueueSpawnPoints();
        StartCoroutine(InitEnablePlatform());
    }

    public Transform GetSpawnPoint()
    {
        Transform returnTransform = queueSpawnPoints.Peek();
        queueSpawnPoints.Dequeue();
        queueSpawnPoints.Enqueue(returnTransform);

        return returnTransform;
    }

    private void FillQueueSpawnPoints()
    {
        for (int i = 0; i < pointsToSpawn.Count; i++)
        {
            queueSpawnPoints.Enqueue(pointsToSpawn[i]);
        }
    }

    private IEnumerator InitEnablePlatform()
    {
        yield return new WaitForEndOfFrame();
        gridController.enabled = true;
        gridController.target = currentSpline.transform.GetChild(1);
        yield return new WaitForEndOfFrame();
        enemyController.enabled = true;
        enemyController.currentFlowField = currentFlowField;
        yield return new WaitForEndOfFrame();
        currentSpline.enabled = true;
        yield return null;
    }

    public void TimeToLeavePlatform()
    {
        changePlatformManager.GiveCommandToPlayer();
        
    }
}
