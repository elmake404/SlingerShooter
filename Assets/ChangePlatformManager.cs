using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlatformManager : MonoBehaviour
{
    [HideInInspector] public EnemyController currentEnemyController;
    public List<PlatformController> platformsController;
    [HideInInspector] public PlayerController playerController;
    private int currentPlatformIndex;

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
        //GetFirstPointAtNextPlatform();
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
}
