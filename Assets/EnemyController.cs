using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlatformController platformController;
    [HideInInspector]public FlowField currentFlowField;

    private void OnEnable()
    {
        platformController = transform.parent.GetComponent<PlatformController>();
        StartCoroutine(PeriodicSpawnEnemy());
    }

    private IEnumerator PeriodicSpawnEnemy()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
        {
            GameObject newEnemy = PoolGameObjects.poolGameObjects.GetObjectInPool(PooledItemID.enemy1);
            if (newEnemy != null)
            {
                newEnemy.GetComponent<EnemyMove>().targetFlowField = currentFlowField;
                newEnemy.transform.position = platformController.GetSpawnPoint().position;

            }
            
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }



}
