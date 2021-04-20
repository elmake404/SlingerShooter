using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [HideInInspector] public PlatformController platformController;
    public ObjectsToDestroy objectsToDestroy;
    public Collider obstacleColider;
    public Transform[] pointsToSpawnShooters;
    public GameObject enemyShooter;
    [HideInInspector] public CameraControll playerCamera;
    private Coroutine spawnEnemyCoroutine;
    private List<CurrentEnemyShootControl> spawnedEnemies;

    void Start()
    {
        spawnedEnemies = new List<CurrentEnemyShootControl>();
        platformController = transform.parent.GetComponent<PlatformController>();
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemyShooter());

    }

    public void MakeTowerDestroyed(Vector3 sourceExplosion)
    {
        StopCoroutine(spawnEnemyCoroutine);
        DisableObstacleCollider();

        if (spawnedEnemies.Count != 0)
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (spawnedEnemies[i].isDead == true) { continue; }
                spawnedEnemies[i].InitDeathThisGuyOnCrashBuild(sourceExplosion);
            }
        }
    }
        
    private void DisableObstacleCollider()
    {
        obstacleColider.enabled = false;
    }


    private void AddEnemyToSpawnedList(CurrentEnemyShootControl currentEnemy)
    {
        spawnedEnemies.Add(currentEnemy);
    }

    public void DeleteEnemyFromSpawnedList(CurrentEnemyShootControl currentEnemy)
    {
        int currentHash = currentEnemy.GetHashCode();
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (currentHash == spawnedEnemies[i].GetHashCode())
            {
                spawnedEnemies.RemoveAt(i);
                break;
            }
        }
    }

    private IEnumerator SpawnEnemyShooter()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < pointsToSpawnShooters.Length; i++)
        {
            GameObject newInstance = Instantiate(enemyShooter, pointsToSpawnShooters[i].transform.position, Quaternion.identity);
            CurrentEnemyShootControl enemyShootControl = newInstance.GetComponent<CurrentEnemyShootControl>();
            enemyShootControl.playerCamera = playerCamera;
            AddEnemyToSpawnedList(enemyShootControl);
            yield return new WaitForSeconds(10f);
        }

        yield return null;
    }
}
