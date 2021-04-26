using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public GameObject fireParticlesInstance;
    private int maxParticlesCount = 5;
    private List<int> storeFiredIndexVertice;
    private List<GameObject> fireParticles;
    private bool isFired = false;
    private Mesh storedMesh;
    private Coroutine spreadFireRoutine;

    private void Start()
    {
        storeFiredIndexVertice = new List<int>();
        fireParticles = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (isFired != true) { return; }
        if (fireParticles.Count == 0) { return; }
        skinnedMesh.BakeMesh(storedMesh);
        Vector3[] vertices = storedMesh.vertices;

        for (int i = 0; i < fireParticles.Count; i++)
        {
            fireParticles[i].transform.position = transform.TransformPoint(vertices[i]);
        }
    }

    public void SetFireOnEnemy(Vector3 explodePos)
    {
        isFired = true;
        storedMesh = new Mesh();
        skinnedMesh.BakeMesh(storedMesh);
        //Debug.Log(mesh.normals.Length);
        
        Vector3[] storedNormals = storedMesh.normals;
        Vector3[] storedVertices = storedMesh.vertices;


        for (int i = 0; i < storedNormals.Length; i++)
        {
            Vector3 verticePosOnWorld = transform.TransformPoint(storedVertices[i]);
            Vector3 directionToExplodePos = explodePos - verticePosOnWorld;
            directionToExplodePos.Normalize();
            float dot = Mathf.Abs(Vector3.Dot(storedNormals[i], directionToExplodePos));
            //Mathf.Abs(dot);
            if (dot < 0.05f)
            {
                storeFiredIndexVertice.Add(i);
                //Debug.DrawRay(transform.TransformPoint(storedVertices[i]), storedNormals[i] / 4, Color.red, Mathf.Infinity);
            }
        }

        spreadFireRoutine = StartCoroutine(CreateFireParticles());
    }

    private IEnumerator CreateFireParticles()
    {
        for (int i = 0; i < maxParticlesCount; i++)
        {
            fireParticles.Add(Instantiate(fireParticlesInstance));
            yield return new WaitForSeconds(0.4f);
        }


        yield return null;
    }
    public void DestroyParticles()
    {
        if (spreadFireRoutine != null)
        {
            StopCoroutine(spreadFireRoutine);
        }
        
        for (int i = 0; i < fireParticles.Count; i++)
        {
            Destroy(fireParticles[i]);
        }
    }


}

