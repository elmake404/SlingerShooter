using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public List<Transform> pointsToSpawn;
    public GridController gridController;
    public EnemyController enemyController;
    private Queue<Transform> queueSpawnPoints;
    

    private void OnEnable()
    {
        queueSpawnPoints = new Queue<Transform>();
        FillQueueSpawnPoints();
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
}
