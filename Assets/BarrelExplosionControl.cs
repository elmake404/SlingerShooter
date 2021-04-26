using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosionControl : MonoBehaviour
{
    public GameObject particles;
    public LayerMask layerMask;
    private float radiusExplosion = 8f;
    private bool isExplosion = false;
    

    public void MakeBarrelExplosion()
    {
        if (isExplosion == true) { return; }
        isExplosion = true;
        GameObject particlesInstance = PlayExplosionParticles();
        StartCoroutine(DeleteObjectAfterPlay(particlesInstance));
        GetComponent<MeshRenderer>().enabled = false;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int toDestructObjectLayer = LayerMask.NameToLayer("ObjectToDestroy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);
        if (colliders.Length != 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.layer == enemyLayer)
                {
                    colliders[i].transform.parent.GetComponent<CurrentEnemyControl>().InitDeathOnBarrelExplosion(transform.position);
                }
                else if (colliders[i].gameObject.layer == toDestructObjectLayer)
                {
                    colliders[i].transform.parent.GetComponent<ObjectsToDestroy>().InitExplosion(transform.position);
                }
            }
        }
    }

    private GameObject PlayExplosionParticles()
    {
        GameObject instance = Instantiate(particles);
        instance.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        return instance;
    }

    private IEnumerator DeleteObjectAfterPlay(GameObject particleInstance)
    {
        float duration = particles.GetComponent<ParticleSystem>().main.duration;
        yield return new WaitForSeconds(duration);
        Destroy(particleInstance);
        Destroy(gameObject);
        yield return null;
    }
}
