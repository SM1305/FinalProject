using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetStep : MonoBehaviour
{
    public bool testBool;

    public GameObject otherFoot;

    public GameObject footCollisionPoint;

    public GameObject spine;

    public GameObject StepTrajectory;

    private FootStates footStates;
    private Placement footPlacement;

    public GameObject target;
    public GameObject curveTrajectory;
    private BezierCurveFootTrajectory bCurveFootTrajectory;
    private GameObject[] controlPoints = new GameObject[4];
    private BezierCurveFollower bCurveFollower;
    private bool plotBezier;

    private float pivotRadius;
    public float pivotSpeed;
    public float pivotAngle = 0;

    public bool isGrounded = false;
    private float isGroundedRayLength;



    private void Start()
    {
        bCurveFootTrajectory = curveTrajectory.GetComponent<BezierCurveFootTrajectory>();
        controlPoints[0] = curveTrajectory.transform.GetChild(0).gameObject;
        controlPoints[1] = curveTrajectory.transform.GetChild(1).gameObject;
        controlPoints[2] = curveTrajectory.transform.GetChild(2).gameObject;
        controlPoints[3] = curveTrajectory.transform.GetChild(3).gameObject;

        isGroundedRayLength = GetComponent<SphereCollider>().radius;

        footStates = GetComponent<FootStates>();
        footPlacement = spine.GetComponent<Placement>();
        bCurveFollower = target.GetComponent<BezierCurveFollower>();

        footStates.footState = FootStates.FootState.onStart;
    }


    private void Update()
    {
        CheckGrounded();
        Step();
    }

    void Step()
    {
        switch (footStates.footState)
        {
            case FootStates.FootState.onStart:
                break;

            case FootStates.FootState.Attach:
                //Debug.Log(transform.name + " - is attaching");
                //pivotRadius = Vector3.Distance(transform.position, spine.transform.position);
                //otherFoot.GetComponent<FeetStep>().footStates.footState = FootStates.FootState.PreExit;
                footStates.footState = FootStates.FootState.Support;
                break;

            case FootStates.FootState.Support:
                //Debug.Log(transform.name + " - is supporting");
                //target.transform.position = footCollisionPoint.transform.position;
                PivotHipsAround();
                break;

            case FootStates.FootState.PreExit:
                //Debug.Log(transform.name + " - is preExit");
                break;

            case FootStates.FootState.Exit:
                //Debug.Log(transform.name + " - is exiting collision");
                isGrounded = false;
                PositionBezierPoints();
                footStates.footState = FootStates.FootState.Swing;
                break;

            case FootStates.FootState.Swing:
                //Debug.Log(transform.name + " - is swinging");
                break;
        }
    }

    void PositionBezierPoints()
    {
        Vector3 cp0Pos;
        Vector3 cp1Pos;
        Vector3 cp2Pos;
        Vector3 cp3Pos;

        cp0Pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        controlPoints[0].transform.position = cp0Pos;

        cp1Pos = new Vector3(cp0Pos.x, cp0Pos.y + 2, cp0Pos.z);
        controlPoints[1].transform.position = cp1Pos;

        cp3Pos = footPlacement.nextTarget.transform.position;
        controlPoints[3].transform.position = footPlacement.nextTarget.transform.position;

        cp2Pos = new Vector3(controlPoints[3].transform.position.x, footPlacement.hips.transform.position.y - 3, controlPoints[3].transform.position.z);
        controlPoints[2].transform.position = cp2Pos;


        Debug.Log("FT cp1 posiition: " + controlPoints[1].transform.position);

        while (bCurveFootTrajectory.DrawCubicCurve() == false)
        {
            Debug.Log("is changing cp1 pos");
            cp1Pos.y++;
            cp1Pos.x--;
            controlPoints[1].transform.position = cp1Pos;

            cp2Pos.y++;
            cp2Pos.x++;
            controlPoints[2].transform.position = cp2Pos;
        }



        bCurveFollower.followCurve = true;
    }

    //public void RepositionControlPoints()
    //{
    //    cp1Pos.y = cp1Pos.y++;
    //    cp1Pos.x = cp1Pos.x--;
    //}

    void PivotHipsAround()
    {
        Debug.Log(transform.name + " - IS PIVOTING SPINE");

        spine.transform.RotateAround(transform.position, transform.right, -pivotSpeed * Time.deltaTime);
        spine.transform.rotation = Quaternion.identity;
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        footCollisionPoint.transform.position = collision.contacts[0].point;
    //        footStates.footState = FootStates.FootState.Attach;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        footStates.footState = FootStates.FootState.Exit;
    //    }
    //}

    private void CheckGrounded()
    {
        RaycastHit groundHit;
        Debug.DrawRay(transform.position, Vector3.down /*(transform.TransformDirection(new Vector3 transform.position.up).normalized)*/, Color.magenta);
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, isGroundedRayLength * 4))
        {
            Debug.Log("raycast hit something");

            if (groundHit.transform.name == "Ground")
            {
                Debug.Log("raycast hit 'Ground'");
                Debug.DrawLine(transform.position, new Vector3(groundHit.point.x, groundHit.point.y - 5, groundHit.point.z), Color.white);

                if (isGrounded == false)
                {
                    footStates.footState = FootStates.FootState.Attach;
                    isGrounded = true;
                }
            }
        }
        else if (isGrounded == true)
        {
            Debug.Log("raycast not hitting anything");
            footStates.footState = FootStates.FootState.Exit;
        }
    }
}

