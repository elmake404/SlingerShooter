using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [HideInInspector] public Vector3 targetPoint;

    private void Start()
    {
        targetPoint.y = -0.5f;
    }

    private void FixedUpdate()
    {
        MoveBullet();
        
    }
    

    private void MoveBullet()
    {
        Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPoint, 20f * Time.deltaTime);
        transform.position = nextPos;

        if (Vector3.Distance(nextPos, targetPoint) < 0.1f)
        {
            DamageHits.instance.AddHit();   
            Destroy(gameObject);
        }
    }
}
