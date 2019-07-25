using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetStep3D : MonoBehaviour
{
    public GameObject leftFoot, rightFoot;

    private LegLocomotionPlacement3D legLocomotionScript;

    public bool testBool;

    public GameObject otherFoot;

    public GameObject footcolposcube;
    private Vector3 footCollisionPoint;

    public GameObject spine;

    private FootStates footStates;
    private Placement footPlacement;

    public GameObject target;
    public GameObject curveTrajectory;
    private BezierTrajectoryBubble3D bCurveFootTrajectory;
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
        legLocomotionScript = spine.GetComponent<LegLocomotionPlacement3D>();

        bCurveFootTrajectory = curveTrajectory.GetComponent<BezierTrajectoryBubble3D>();
        controlPoints[0] = curveTrajectory.transform.GetChild(0).gameObject;
        controlPoints[1] = curveTrajectory.transform.GetChild(1).gameObject;
        controlPoints[2] = curveTrajectory.transform.GetChild(2).gameObject;
        controlPoints[3] = curveTrajectory.transform.GetChild(3).gameObject;

        isGroundedRayLength = 0.4f;

        footStates = GetComponent<FootStates>();
        footPlacement = spine.GetComponent<Placement>();
        bCurveFollower = target.GetComponent<BezierCurveFollower>();

        footStates.footState = FootStates.FootState.onStart;
    }


    private void Update()
    {
        if (legLocomotionScript.locomotionState == LegLocomotionPlacement3D.LocomotionState.Walking)
        {
            Step();
        }
    }

    void Step()
    {
        switch (footStates.footState)
        {
            case FootStates.FootState.onStart:
                break;

            case FootStates.FootState.Attach:
                footStates.footState = FootStates.FootState.Support;
                break;

            case FootStates.FootState.Support:

                PivotHipsAround();
                break;

            case FootStates.FootState.PreExit:
                break;

            case FootStates.FootState.Exit:
                isGrounded = false;
                PositionBezierPoints();
                footStates.footState = FootStates.FootState.Swing;
                break;

            case FootStates.FootState.Swing:
                if (Vector3.Distance(transform.position, controlPoints[3].transform.position) < 1f)
                {
                    if (this.gameObject == leftFoot)
                    {
                        FootStates otherFootScript = rightFoot.GetComponent<FootStates>();
                        otherFootScript.footState = FootStates.FootState.Exit;
                    }
                    else if (this.gameObject == rightFoot)
                    {
                        FootStates otherFootScript = leftFoot.GetComponent<FootStates>();
                        otherFootScript.footState = FootStates.FootState.Exit;
                    }

                    CheckGrounded();
                }
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

        cp3Pos = legLocomotionScript.nextTarget.transform.position;
        controlPoints[3].transform.position = legLocomotionScript.nextTarget.transform.position;

        cp2Pos = new Vector3(controlPoints[3].transform.position.x, legLocomotionScript.hips.transform.position.y - 3, controlPoints[3].transform.position.z);
        controlPoints[2].transform.position = cp2Pos;

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


    void PivotHipsAround()
    {
        Debug.Log(transform.name + " - IS PIVOTING SPINE");

        spine.transform.RotateAround(footCollisionPoint, spine.transform.right, legLocomotionScript.proportionalStrideLength * 5 * Time.deltaTime);
        spine.transform.rotation = Quaternion.identity;
    }


    private void CheckGrounded()
    {
        RaycastHit groundHit;
        Debug.DrawRay(transform.position, Vector3.down /*(transform.TransformDirection(new Vector3 transform.position.up).normalized)*/, Color.magenta);
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, isGroundedRayLength * 4))
        {
            Debug.Log("raycast hit something.... it has hit: " + groundHit.transform.name);

            if (groundHit.transform.name == "Ground")
            {
                footCollisionPoint = groundHit.point;
                footcolposcube.transform.position = footCollisionPoint;
                Debug.Log("raycast hit 'Ground'");
                //Debug.DrawLine(transform.position, new Vector3(groundHit.point.x, groundHit.point.y - 1, groundHit.point.z), Color.white);

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

