using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCatmullRomCurves : MonoBehaviour
{
    public GameObject[] controlPoints;
    public int numberOfPoints;
    public Vector3[] linePositions;
    public Vector3[] tangents;


    private Vector3 m0; //prev tangent influence
    private Vector3 m1; //next tangent influence


    [Range(0.0f, 1.0f)]
    public float tension; //curve tension through control points

    Vector3 tangent; //segment tan
    Vector3 position; //segment pos
    float step; //time


    private void Start()
    {
        tension = 0.5f;

        //define length of arrays to hold segment positions/angles
        //multiply by number of connections to be drawn *between* control points (number of control points - 1)
        linePositions = new Vector3[numberOfPoints * 3];
        tangents = new Vector3[numberOfPoints * 3];
    }

    void Update()
    {
        for (int i = 0; i < controlPoints.Length - 1; i++) //loop connects current control point to the following control point for each control point in control point array
        {
            //calculate tangents for ith control point segment
            //tangent mk = (Pk+1 - Pk-1) / 2
            //if statements ensure that tangents for each segment are calculated from the correct control points /*if solution to contain 4+ control points, more elegant logic required*/
            if (i == 0)
            {
                m0 = (controlPoints[i + 1].transform.position - controlPoints[i].transform.position) * tension; //(p1-p0)/2
                m1 = (controlPoints[i + 2].transform.position - controlPoints[i].transform.position) * tension; //(p2-p0)/2
            }
            if (i == 1)
            {
                m0 = (controlPoints[i + 1].transform.position - controlPoints[i - 1].transform.position) * tension; //(p2-p0)/2
                m1 = (controlPoints[i + 2].transform.position - controlPoints[i].transform.position) * tension; //(p3-p1)/2
            }
            if (i == 2)
            {
                m0 = (controlPoints[i + 1].transform.position - controlPoints[i - 1].transform.position) * tension; //(p3-p1)/2
                m1 = (controlPoints[i + 1].transform.position - controlPoints[i].transform.position) * tension; //(p3-p2)/2
            }
            

            float pointsDist = 1.0f / (numberOfPoints - 1); //ensures that the spline reaches the next control point

            for (int j = 0; j < numberOfPoints; j++) //loop creates spline segments
            {
                step = j * pointsDist; //fraction of the distance between current and next control point (between 0 and 1)

                //calculate catmullrom
                position = CatmullRom(controlPoints[i].transform.position, controlPoints[i + 1].transform.position, m0, m1, step, out tangent);

                //apply spline segments
                linePositions[i * numberOfPoints + j] = position;
                tangents[i * numberOfPoints + j] = tangent;
            }
        }

        DrawLines();
    }


    public Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1, float step, out Vector3 tangent)
    {
        //tangent = (6t^2 - 6t)p0 + (3t^2 - 4t + 1)m0 + (-6t^2 + 6t)p1 + (3t^2 - 2t)m1
        tangent = ((6 * Mathf.Pow(step, 2) - 6 * step) * p0) + ((3 * Mathf.Pow(step, 2) - 4 * step + 1) * m0) + ((-6 * Mathf.Pow(step, 2) + 6 * step) * p1) + ((3 * Mathf.Pow(step, 2) - Mathf.Pow(step, 2)) * m1);

        //hermite = (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1     //points p0 and p1 m0 though m0 and m1 interpolated using t (between 0, 1)
        position = ((2 * Mathf.Pow(step, 3) - 3 * Mathf.Pow(step, 2) + 1) * p0) + (((Mathf.Pow(step, 3)) - 2 * Mathf.Pow(step, 2) + step) * m0) + ((-2 * Mathf.Pow(step, 3) + 3 * Mathf.Pow(step, 2)) * p1) + ((Mathf.Pow(step, 3) - Mathf.Pow(step, 2)) * m1);

        return position;
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
            Gizmos.DrawWireCube(linePositions[i], new Vector3(0.2f, 0.2f, 0.2f));
        }
    }
}
