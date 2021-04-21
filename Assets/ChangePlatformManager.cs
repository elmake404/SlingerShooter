using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlatformManager : MonoBehaviour
{
    [HideInInspector] public EnemyController currentEnemyController;
    public List<PlatformController> platformsController;
    [HideInInspector] public PlayerController playerController;
    private int currentPlatformIndex;
    private List<Vector3> pointsForPath;

    private void Awake()
    {
        EnableFirstPlatform();
        playerController = FindObjectOfType<PlayerController>();
        
    }

    private void EnableFirstPlatform()
    {
        platformsController[0].enabled = true;
        platformsController[0].changePlatformManager = this;
        currentPlatformIndex = 0;
        GetFirstPointAtNextPlatform();
        GeneraMeshedSplineObject();
    }

    private void EnableNextPlatform()
    {
        currentPlatformIndex += 1;
        platformsController[currentPlatformIndex].enabled = true;
        platformsController[currentPlatformIndex].changePlatformManager = this;
    }

    public void GiveCommandToPlayer()
    {
        if (platformsController.Count-1 < currentPlatformIndex + 1) { return; }
        Transform endPoint = GetFirstPointAtNextPlatform();
        platformsController[currentPlatformIndex].currentSpline.MakeSpline(endPoint);
        playerController.currentSpline = platformsController[currentPlatformIndex].currentSpline;
        playerController.StartMovePlayer();
        EnableNextPlatform();
    }
    
    private Transform GetFirstPointAtNextPlatform()
    {
        return platformsController[currentPlatformIndex + 1].GetComponentInChildren<CatmulSpline>().transform.GetChild(1);
    }

    private List<Transform> GetAllPointsRoad()
    {
        List<Transform> allPathPoints = new List<Transform>();

        for (int i = 0; i < platformsController.Count; i++)
        {
            if (i == 0)
            {
                for (int k = 0; k < platformsController[i].currentSpline.pointsToSpline.Length - 1; k++)
                {
                    allPathPoints.Add(platformsController[i].currentSpline.pointsToSpline[k]);
                }
            }
            else
            {
                for (int z = 1; z < platformsController[i].currentSpline.pointsToSpline.Length - 1; z++)
                {
                    allPathPoints.Add(platformsController[i].currentSpline.pointsToSpline[z]);
                }
            }
            
        }

        return allPathPoints;
    }

    private void GeneraMeshedSplineObject()
    {
        GameObject newMeshedSpline = new GameObject();
        MeshFilter meshFilter = newMeshedSpline.AddComponent<MeshFilter>();
        newMeshedSpline.AddComponent<MeshRenderer>();

        pointsForPath = CatmulSpline.GetEquidistantPoints(0.5f, GetAllPointsRoad());
        meshFilter.mesh = SplineMesh.GetGeneratedMesh(pointsForPath, 1f);
        Vector3 offset = Vector3.zero;
        offset.y = 0.1f;
        newMeshedSpline.transform.position += offset;
    }
}
