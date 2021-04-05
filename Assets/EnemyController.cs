using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlatformController platformController;
    [HideInInspector] public FlowField currentFlowField;
    public static UsedCells createdUsedCells;
    private Cell targetCell;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        createdUsedCells = new UsedCells();
        createdUsedCells.usedCells = new List<Cell>();
        platformController = transform.parent.GetComponent<PlatformController>();
        targetCell = platformController.gridController.targetCell;
        StartCoroutine(PeriodicSpawnEnemy());
    }

    private IEnumerator PeriodicSpawnEnemy()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 9; i++)
        {
            GameObject newEnemy = PoolGameObjects.poolGameObjects.GetObjectInPool(PooledItemID.enemy1);
            if (newEnemy != null)
            {
                EnemyMove enemyMove = newEnemy.GetComponent<EnemyMove>();
                enemyMove.targetCell = targetCell;
                enemyMove.targetFlowField = currentFlowField;
                enemyMove.ownUsedCells = createdUsedCells;
                newEnemy.transform.position = platformController.GetSpawnPoint().position;
                

            }
            
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }



}
