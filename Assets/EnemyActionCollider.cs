using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionCollider : MonoBehaviour
{
    private Collider thisColider;

    private void Start()
    {
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
