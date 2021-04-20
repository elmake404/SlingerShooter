using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsToDestroy : MonoBehaviour
{
    
    private Rigidbody[] rigidbodies;
    private bool isDestroyed = false;
    private TowerControl towerControl;

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        towerControl = transform.parent.gameObject.GetComponent<TowerControl>();
    }

    public void InitExplosion(Vector3 sourceExplosion)
    {
        if (isDestroyed == true) { return; }
        isDestroyed = true;
        towerControl.platformController.AddTowerToCrushed();

        towerControl.MakeTowerDestroyed(sourceExplosion);
        
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
            rigidbodies[i].useGravity = true;
            rigidbodies[i].AddExplosionForce(10f, sourceExplosion, 10f, 10f, ForceMode.Impulse);
        }
        StartCoroutine(DisableDestructedObjects());
    }

    private IEnumerator DisableDestructedObjects()
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            Destroy(rigidbodies[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
