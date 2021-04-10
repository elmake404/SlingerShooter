using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionCollider : MonoBehaviour
{
    private int layerMaskEnemy;
    private Collider thisColider;

    private void Start()
    {
        layerMaskEnemy = LayerMask.NameToLayer("Enemy");
        //Debug.Log(layerMaskEnemy);
        thisColider = GetComponent<Collider>();
    }
    public void DisableActionCollider()
    {
        thisColider.enabled = false;
    }

    public void EnableActionColider()
    {
        thisColider.enabled = true;
    }



   
}
