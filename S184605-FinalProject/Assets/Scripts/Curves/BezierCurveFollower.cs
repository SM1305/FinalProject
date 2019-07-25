using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveFollower : MonoBehaviour
{
    public GameObject curveToFollow;
    private GameObject[] controlPoints = new GameObject[4];

    public float interpolation;
    public float speed;
    public bool followCurve = false;


    void Start ()
    {
        controlPoints[0] = curveToFollow.transform.GetChild(0).gameObject;
        controlPoints[1] = curveToFollow.transform.GetChild(1).gameObject;
        controlPoints[2] = curveToFollow.transform.GetChild(2).gameObject;
        controlPoints[3] = curveToFollow.transform.GetChild(3).gameObject;
    }

	void Update ()
    {
	    if (followCurve)
        {
            if (interpolation <= 1)
            {
                interpolation += Time.deltaTime * speed;

                transform.position = /*controlPoint0*/  (Mathf.Pow((1 - interpolation), 3) * controlPoints[0].transform.position) + 
                                     /*controlPoint1*/  (3 * Mathf.Pow(1 - interpolation, 2) * interpolation * controlPoints[1].transform.position) + 
                                     /*controlPoint2*/  (3 * (1 - interpolation) * Mathf.Pow(interpolation, 2) * controlPoints[2].transform.position) + 
                                     /*controlPoint3*/  (Mathf.Pow(interpolation, 3) * controlPoints[3].transform.position);               
            }
            else
            {
                followCurve = false;
                interpolation = 0;
            }
        }
	}
}
