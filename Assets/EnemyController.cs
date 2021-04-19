using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlatformController platformController;
    [HideInInspector] public FlowField currentFlowField;
    [HideInInspector] public bool isWaitFighters = false;
    public static UsedCells createdUsedCells;
    public GameObject enemy1;
    private Cell targetCell;
    private CameraControll playerCamera;
    public List<int> enemiesThatFight;
    private List<int> enemiesThatWantFight;
    private int numOfSpawnEnemies = 5;
    private int numOfDeadEnemies = 0;

    private void Start()
    {
        enemiesThatFight = new List<int>();
        enemiesThatWantFight = new List<int>();
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
        for (int i = 0; i < numOfSpawnEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemy1);
            EnemyMove enemyMove = newEnemy.GetComponent<EnemyMove>();
            enemyMove.enemyController = this;
            enemyMove.targetCell = targetCell;
            enemyMove.targetFlowField = currentFlowField;
            enemyMove.ownUsedCells = createdUsedCells;
            enemyMove.playerCamera = playerCamera;
            newEnemy.transform.position = platformController.GetSpawnPoint().position;
            yield return new WaitForSeconds(2f);
        }
        
        yield return null;
    }

    public void AddEnemyToFightQueue(int hashObject)
    {
        if (enemiesThatFight.Count >= 2)
        {
            enemiesThatWantFight.Add(hashObject);
        }
        else
        {
            enemiesThatFight.Add(hashObject);
        }
    }

    public void RemoveKilledEnemyOnFight(int hashObject)
    {
        if (enemiesThatFight.Remove(hashObject))
        {
            JoinWaitingEnemyToFight();
        }
        else if (enemiesThatWantFight.Remove(hashObject))
        {
        }
    }

    private void JoinWaitingEnemyToFight()
    {
        if (enemiesThatWantFight.Count == 0) { return; }
        enemiesThatFight.Add(enemiesThatWantFight[0]);
        enemiesThatWantFight.Remove(enemiesThatWantFight[0]);
    }

    public bool EnemyIsOnFight(int hashObject)
    {
        if (enemiesThatFight.Contains(hashObject))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddEnemyToDeadList()
    {
        numOfDeadEnemies += 1;
        if (numOfSpawnEnemies == numOfDeadEnemies)
        {
            platformController.TimeToLeavePlatform();
        }
        //Debug.Log("AddEnemyToDeadList");
    }
}
