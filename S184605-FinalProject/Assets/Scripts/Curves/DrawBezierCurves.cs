using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBezierCurves : MonoBehaviour
{
    public GameObject controlPoint0;
    public GameObject controlPoint1;
    public GameObject controlPoint2;
    public GameObject controlPoint3;

    public int numberOfPoints;
    public Vector3[] linePositions;

    public bool drawLinearCurve;
    public bool drawQuadraticCurve;
    public bool drawCubicCurve;

    void Start()
    {
        linePositions = new Vector3[numberOfPoints];
    }

    void Update()
    {
        if (numberOfPoints <= 1)
        {
            Debug.LogWarning("There are only: <" + numberOfPoints + "> points. Consider using more!");
        }

        DrawLines();

        if (drawCubicCurve)
            DrawCubicCurve();
        else if (drawQuadraticCurve)
            DrawQuadraticCurve();
        else if (drawLinearCurve)
            DrawLinearCurve();
    }

    //sets position of each subsequent segment in the curve, using 2 control points
    void DrawLinearCurve()
    {
        for (int i = 1; i < numberOfPoints + 1; i++)
        {
            float floatNumberOfPoints = numberOfPoints;
            float t = i / floatNumberOfPoints;
            linePositions[i - 1] = CalculateLinearCurvePoints(t, controlPoint0.transform.position, controlPoint1.transform.position);
        }
    }

    //returns position value of point on linear Bezier curve, found from linearly interpolating between 2 control points
    public Vector3 CalculateLinearCurvePoints(float t, Vector3 point0, Vector3 point1)
    {
        //curve = Point0 + t * (PointI – Point0)
        Vector3 point = point0 + t * (point1 - point0);

        return point;
    }

    //sets position of each subsequent segment in the curve, using 3 control points
    void DrawQuadraticCurve()
    {
        for (int i = 1; i < numberOfPoints + 1; i++)
        {
            float floatNumberOfPoints = numberOfPoints;
            float t = i / floatNumberOfPoints;
            linePositions[i - 1] = CalculateQuadraticCurvePoints(t, controlPoint0.transform.position, controlPoint1.transform.position, controlPoint2.transform.position);
        }
    }

    //returns position value of point on Quadratic Bezier curve, found from interpolating two linear Bezier curves
    public Vector3 CalculateQuadraticCurvePoints(float t, Vector3 point0, Vector3 point1, Vector3 point2)
    {
        //curve = (1-t)^2 * Point0 + 2 * (1-t) * t * PointI + t^2 * PointII
        Vector3 point = (Mathf.Pow((1 - t), 2) * point0) + (2 * (1 - t) * t * point1) + (Mathf.Pow(t, 2) * point2);

        return point;
    }

    //sets position of each subsequent segment in the curve, using 4 control points
    void DrawCubicCurve()
    {
        for (int i = 1; i < numberOfPoints + 1; i++)
        {
            float floatNumberOfPoints = numberOfPoints;
            float t = i / floatNumberOfPoints;
            linePositions[i - 1] = CalculateCubicCurvePoints(t, controlPoint0.transform.position, controlPoint1.transform.position, controlPoint2.transform.position, controlPoint3.transform.position);
        }
    }

    //returns position value of point on cubic Bezier curve, found from linearly interpolating two quadratic Bezier curves which are found from linear interpolation of 3 pairs of 2 control points
    public Vector3 CalculateCubicCurvePoints(float t, Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        //curve = (1-t)^3 * Point0 + 3 * (1-t)^2 * t * PointI + 3(1-t) * t^2 * PointII + t^2 * PointIII 
        Vector3 point = (Mathf.Pow((1-t), 3) * point0) + (3 * Mathf.Pow(1-t, 2) * t * point1) + (3 * (1-t) * Mathf.Pow(t, 2) * point2) + (Mathf.Pow(t, 3) * point3);

        return point;
    }

    //draw lines between each line position
    void DrawLines()
    {
        for (int i = 0; i < linePositions.Length - 1; i++)
        {
            Debug.DrawLine(linePositions[i], linePositions[i + 1], Color.white);
        }
    }

    //draw spheres at line positions
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < linePositions.Length; i++)
        {
            Gizmos.DrawWireSphere(linePositions[i], 0.1f);
        }
    }
}







    