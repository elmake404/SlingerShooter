using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlatformController platformController;
    [HideInInspector] public FlowField currentFlowField;
    public static UsedCells createdUsedCells;
    public GameObject enemy1;
    private Cell targetCell;
    private CameraControll playerCamera;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        createdUsedCells = new UsedCells();
        createdUsedCells.usedCells = new List<Cell>();
        platformController = transform.parent.GetComponent<PlatformController>();
        playerCamera = platformController.playerCamera;
        targetCell = platformController.gridController.targetCell;
        StartCoroutine(PeriodicSpawnEnemy());
    }

    private IEnumerator PeriodicSpawnEnemy()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 10; i++)
        {
            GameObject newEnemy = Instantiate(enemy1);
                EnemyMove enemyMove = newEnemy.GetComponent<EnemyMove>();
                enemyMove.targetCell = targetCell;
                enemyMove.targetFlowField = currentFlowField;
                enemyMove.ownUsedCells = createdUsedCells;
                enemyMove.playerCamera = playerCamera;
                newEnemy.transform.position = platformController.GetSpawnPoint().position;
            
            
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }



}
