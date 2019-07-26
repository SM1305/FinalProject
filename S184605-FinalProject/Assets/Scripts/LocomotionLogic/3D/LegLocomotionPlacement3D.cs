using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  script is respondible for placement on the 'next step' location
 *  contains logic for defining the next foot to step after transition from standstill
 */

          
public class LegLocomotionPlacement3D : MonoBehaviour
{
    public enum LocomotionState { StandingStill, Walking };
    public LocomotionState locomotionState;

    public GameObject leftLegEndEffector;
    private FootStates leftLegFootStates;
    public GameObject rightLegEndEffector;
    private FootStates rightLegFootStates;
    public GameObject targetLeftFoot;
    public GameObject targetRightFoot;

    public GameObject nextTarget;

    public GameObject hips;
    public GameObject verticalRay;

    public BipedMovementInput userInput;
    public float proportionalStrideLength;
    public float proportionalPendulumRotateSpeed;

    public bool isRedStepping = false;

    public float spineSpeed;

    public bool showGiz = false;

    public bool isNewStep = false;


    private void Awake()
    {
        locomotionState = LocomotionState.StandingStill;

        leftLegFootStates = leftLegEndEffector.GetComponent<FootStates>();
        rightLegFootStates = rightLegEndEffector.GetComponent<FootStates>();
    }


    void Update()
    {
        ProportionalStrideInput();
                          
        PositionRaycastAhead();

        RaycastFootPlacement();

        LocomotionLogic();
    }

    //ahead position of 'next target' - changes based on user input
    void PositionRaycastAhead()
    {
        verticalRay.transform.position = hips.transform.position + hips.transform.forward * (proportionalStrideLength * 5);
    }

    //places 'next step'
    void RaycastFootPlacement()
    {
        RaycastHit hit;
        if (Physics.Raycast(verticalRay.transform.position, -verticalRay.transform.up, out hit))
        {
            Debug.DrawRay(verticalRay.transform.position, -verticalRay.transform.up * hit.distance, Color.yellow);
            nextTarget.transform.position = hit.point;
        }
        else
        {
            Debug.DrawRay(verticalRay.transform.position, -verticalRay.transform.up * 7, Color.red);
        }
    }

    //takes user input, converts to scale and applies to biped model
    void ProportionalStrideInput()
    {
        proportionalStrideLength = Mathf.InverseLerp(0.0f, userInput.maxVelocity, userInput.velocity);

        if (proportionalStrideLength > 0.31f)
        {
            locomotionState = LocomotionState.Walking;
        }
        else
        {
            locomotionState = LocomotionState.StandingStill;
            proportionalStrideLength = 0.3f;
        }
    }

    //determines which foot takes the first step after transitioning from standing still
    void LocomotionLogic()
    {
        switch (locomotionState)
        {
            case LocomotionState.StandingStill:
                Debug.Log("standing still");
                isNewStep = true;
                break;

            case LocomotionState.Walking:
                Debug.Log("walking");

                if (isNewStep)
                {
                    float leftDistance = Vector3.Distance(leftLegEndEffector.transform.position, nextTarget.transform.position);
                    float rightDistance = Vector3.Distance(rightLegEndEffector.transform.position, nextTarget.transform.position);

                    if (leftDistance <= rightDistance)
                    {
                        rightLegFootStates.footState = FootStates.FootState.Support;
                        leftLegFootStates.footState = FootStates.FootState.Exit;
                    }
                    else
                    {
                        rightLegFootStates.footState = FootStates.FootState.Exit;
                        leftLegFootStates.footState = FootStates.FootState.Support;
                    }

                    isNewStep = false;
                }

                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGiz)
        {
            //blue limb reach
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(leftLegEndEffector.transform.position, targetLeftFoot.transform.position);
            //red limb reach
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rightLegEndEffector.transform.position, targetRightFoot.transform.position);

            //hip to raycaster
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(hips.transform.position, verticalRay.transform.position);
            //next target position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(nextTarget.transform.position, 0.5f);
            //hip to next target
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(hips.transform.position, nextTarget.transform.position);

            //blue target
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(targetLeftFoot.transform.position, 0.5f);
            //red target
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetRightFoot.transform.position, 0.5f);
        }
    }
}