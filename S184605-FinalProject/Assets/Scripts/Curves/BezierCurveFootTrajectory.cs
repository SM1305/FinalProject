using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveFootTrajectory : MonoBehaviour
{
    private FeetStep feetStep;

    public bool drawOverTime;
    private float time = 0;
    public float speed;

    public bool isCurvePathClear = true;

    public GameObject controlPoint0;
    public GameObject controlPoint1;
    public GameObject controlPoint2;
    public GameObject controlPoint3;

    public int numberOfPoints;
    public Vector3[] linePositions;


    void Start()
    {
        linePositions = new Vector3[numberOfPoints];
    }

    void Update()
    {
        if (numberOfPoints <= 20)
        {
            Debug.LogWarning("There are only: <" + numberOfPoints + "> points. Consider using more!");
        }

        if (drawOverTime)
        {
            time = Mathf.Clamp(time += (Time.deltaTime * speed), 0, 1);
            DrawCubicCurveOverTime();
        }
        else
        {
            DrawCubicCurve();
        }

        DrawLines();
    }


    public bool DrawCubicCurve()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            float floatNumberOfPoints = numberOfPoints;
            float t = i / floatNumberOfPoints;
            linePositions[i] = CalculateCurvePoints(t, controlPoint0.transform.position, controlPoint1.transform.position, controlPoint2.transform.position, controlPoint3.transform.position);

            if (i <= numberOfPoints - 1 && i != 0)
            {
                RaycastHit lineIntersects;
                if (Physics.Linecast(linePositions[i - 1], linePositions[i], out lineIntersects))
                {
                    Debug.Log("Bezier has contacted: " + lineIntersects.transform.name);

                    if (lineIntersects.normal.y > 0.75f)
                    {
                        //can walk
                        Debug.Log(lineIntersects.transform.name + "'s surface is walkable");
                    }

                    if (lineIntersects.normal.y <= 0.75f)
                    {
                        //need to reposition
                        Debug.Log(lineIntersects.transform.name + "'s surface is not walkable. need to reposition");
                        return false;
                    }
                }
            }
        }
        return true;
    }


    void DrawCubicCurveOverTime()
    {
        for (int i = 1; i < numberOfPoints + 1; i++)
        {
            float floatNumberOfPoints = numberOfPoints;
            float t = i / floatNumberOfPoints;
            linePositions[i - 1] = CalculateCurvePoints(t * time, controlPoint0.transform.position, controlPoint1.transform.position, controlPoint2.transform.position, controlPoint3.transform.position);
        }
    }

    public Vector3 CalculateCurvePoints(float t, Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        //curve = (1-t)^3 * Point0 + 3 * (1-t)^2 * t * PointI + 3(1-t) * t^2 * PointII + t^2 * PointIII 
        Vector3 point = (Mathf.Pow((1 - t), 3) * point0) + (3 * Mathf.Pow(1 - t, 2) * t * point1) + (3 * (1 - t) * Mathf.Pow(t, 2) * point2) + (Mathf.Pow(t, 3) * point3);

        return point;
    }

    void DrawLines()
    {
        for (int i = 0; i < linePositions.Length - 1; i++)
        {
            Debug.DrawLine(linePositions[i], linePositions[i + 1], Color.white);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < linePositions.Length; i++)
        {
            Gizmos.DrawWireSphere(linePositions[i], 0.1f);
        }
    }
}

