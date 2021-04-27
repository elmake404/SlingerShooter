using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmulSpline : MonoBehaviour
{
    public Transform[] pointsToSpline;
    private List<Transform> points;
    [HideInInspector] public float currentLength;

    private void Start()
    {
        points = new List<Transform>(pointsToSpline);
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

        float tx = 0.5f * (points[p0].position.x * q1 + points[p1].position.x * q2 + points[p2].position.x * q3 + points[p3].position.x * q4);
        float tz = 0.5f * (points[p0].position.z * q1 + points[p1].position.z * q2 + points[p2].position.z * q3 + points[p3].position.z * q4);

        return new Vector3(tx, 0f, tz);
    }

    public void MakeSpline(Transform endPoint)
    {
        points.Add(endPoint);
        currentLength = (float)points.Count - 3f;
    }

    public static List<Vector3> GetEquidistantPoints(float spacing ,List<Transform> points)
    {
        List<Vector3> equidistantPoints = new List<Vector3>();
        equidistantPoints.Add(points[0].position);
        Vector3 previousPoint = points[0].position;
        float dstLastEquidPoint = 0f;
        float lengthArrayPoints = (float)points.Count - 4f;
        float t = 0f;

        while (t < lengthArrayPoints)
        {
            t += 0.1f;
            Vector3 pointOnCurve = GetCurevedPoint(t, points);
            dstLastEquidPoint += Vector3.Distance(previousPoint, pointOnCurve);

            while (dstLastEquidPoint >= spacing)
            {
                float overShootDst = dstLastEquidPoint - spacing;
                Vector3 newEquidistantPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overShootDst;
                equidistantPoints.Add(newEquidistantPoint);
                dstLastEquidPoint = overShootDst;
                previousPoint = newEquidistantPoint;
                //Debug.DrawRay(newEquidistantPoint, Vector3.up, Color.green, Mathf.Infinity);
            }
            previousPoint = pointOnCurve;
        }
        return equidistantPoints;
    }

    public static Vector3 GetCurevedPoint(float t, List<Transform> points)
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
        float q3 = -3.0f * ttt + 4.0f * tt + t;
        float q4 = ttt - tt;

        float tx = 0.5f * (points[p0].position.x * q1 + points[p1].position.x * q2 + points[p2].position.x * q3 + points[p3].position.x * q4);
        float tz = 0.5f * (points[p0].position.z * q1 + points[p1].position.z * q2 + points[p2].position.z * q3 + points[p3].position.z * q4);

        return new Vector3(tx, 0f, tz);
    }
}
