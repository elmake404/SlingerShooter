using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMesh
{
    public static Mesh GetGeneratedMesh(List<Vector3> points, float roadWidth)
    {
        Mesh generatedMesh = new Mesh();
        Vector3[] verts = new Vector3[points.Count * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[2 * (points.Count - 1) * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < points.Count - 1)
            {
                forward += points[i + 1] - points[i];
            }
            if (i > 0)
            {
                forward += points[i] - points[i - 1];
            }
            forward.Normalize();
            Vector3 left = new Vector3(-forward.z, forward.y, forward.x);
            verts[vertIndex] = points[i] + left * roadWidth * 0.5f;
            verts[vertIndex + 1] = points[i] - left * roadWidth * 0.5f;

            float completetionPercent = i / (float)(points.Count - 1);
            uvs[vertIndex] = new Vector2(0, completetionPercent);
            uvs[vertIndex + 1] = new Vector2(1, completetionPercent);

            if (i < points.Count - 1)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = vertIndex + 2;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = vertIndex + 2;
                tris[triIndex + 5] = vertIndex + 3;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        generatedMesh.vertices = verts;
        generatedMesh.triangles = tris;
        generatedMesh.uv = uvs;

        return generatedMesh;
    }
}
