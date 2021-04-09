using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmulSpline : MonoBehaviour
{
    public Transform[] pointsToSpline;

    private void Start()
    {
        MakeSpline();
    }

    private Vector3 GetSplinePoint(float t)
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

        float tx = 0.5f * (pointsToSpline[p0].position.x * q1 + pointsToSpline[p1].position.x * q2 + pointsToSpline[p2].position.x * q3 + pointsToSpline[p3].position.x * q4);
        float tz = 0.5f * (pointsToSpline[p0].position.z * q1 + pointsToSpline[p1].position.z * q2 + pointsToSpline[p2].position.z * q3 + pointsToSpline[p3].position.z * q4);

        return new Vector3(tx, 0f, tz);
    }

    private void MakeSpline()
    {
        for (float t = 0f; t < (float)pointsToSpline.Length - 3f; t += 0.1f)
        {
            Vector3 pos = GetSplinePoint(t);
            Debug.DrawRay(pos, Vector3.up, Color.green, Mathf.Infinity);
        }
    }
}
