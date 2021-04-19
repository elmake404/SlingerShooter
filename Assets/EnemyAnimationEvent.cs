using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    public void AddHitDamage()
    {
        DamageHits.instance.AddHit();
        
    }
}
