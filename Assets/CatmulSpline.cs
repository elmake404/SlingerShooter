using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmulSpline : MonoBehaviour
{
    public Transform[] pointsToSpline;
    private List<Transform> generatedNewPoint;
    [HideInInspector] public float currentLength;

    private void Start()
    {
        generatedNewPoint = new List<Transform>(pointsToSpline);
        //MakeSpline();
    }

    public Vector3 GetSplinePoint(float t)
    {
        int p0, p1, p2, p3;
        p1 = (int)t + 1;
        p2 = p1 + 1;
        p3 = p2 + 1;
        p0 = p1 - 1;

        t = t - (int)t;

        float tt = t * t;
        float ttt = tt * t;

        float q1 = -ttt + 2.0f * tt - t;
        float q2 = 3.0f * ttt - 5.0f * tt + 2.0f;
        float q3 = -3.0f*ttt + 4.0f*tt + t;
        float q4 = ttt - tt;

        float tx = 0.5f * (generatedNewPoint[p0].position.x * q1 + generatedNewPoint[p1].position.x * q2 + generatedNewPoint[p2].position.x * q3 + generatedNewPoint[p3].position.x * q4);
        float tz = 0.5f * (generatedNewPoint[p0].position.z * q1 + generatedNewPoint[p1].position.z * q2 + generatedNewPoint[p2].position.z * q3 + generatedNewPoint[p3].position.z * q4);

        return new Vector3(tx, 0f, tz);
    }

    public void MakeSpline(Transform endPoint)
    {
        generatedNewPoint.Add(endPoint);
        currentLength = (float)generatedNewPoint.Count - 3f;

        for (float t = 0f; t < (float)generatedNewPoint.Count - 3f; t += 0.1f)
        {
            Vector3 pos = GetSplinePoint(t);
            Debug.DrawRay(pos, Vector3.up, Color.green, Mathf.Infinity);
        }
    }


}
