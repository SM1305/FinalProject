using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCD : MonoBehaviour
{
    //string as a developer note in inspector, should multiple individual chains of same model be controlled
    public string ThisScriptControls;

    public bool dampen = false;
    public float dampingValue = 0.005f;

    public Joints[] joints;

    public GameObject target;
    public GameObject[] allTargets;
    private Vector3 targetPosition;

    //if the target is reached
    public bool reached = false;

    //current number of attempts to reach target
    public int currentAttempts = 0;
    //maximum number of attempts to reach target before stopping
    public int MaxAttempts = 10;

    //acceptable reached error
    public float minError = 0.1f;


    void Start()
    {
        targetPosition = target.transform.position;
    }

    void Update()
    {
        //the target has moved, reset attepmts to 0 and change targetPosition
        if (target.transform.position != targetPosition)
        {
            currentAttempts = 0;
            targetPosition = target.transform.position;
        }

        //uncomment to use with multi-end CCD
        /* 
        if (allTargets != null)
            foreach (GameObject target in allTargets)
            {
                if (target.transform.position != targetPosition)
                {
                    currentAttempts = 0;
                    targetPosition = target.transform.position;
                }
            }
        */

        //iterates through all joints each frame
        if (reached == false && currentAttempts <= MaxAttempts)
        {
            for (int i = joints.Length - 1; i >= 0; i--) //start from the end effector, move through each joint towards the root joint
            {
                for  (int j = joints.Length - 1; j > i - 1 && j < joints.Length; j--) //second loop returns to the furthest joint from the root on each loop. Gives pattern '5' - '5,4' - '5,4,3' - '5,4,3,2' - '5,4,3,2,1'
                {
                    Vector3 vectorCurrJointToEndEffector = (joints[joints.Length - 1].transform.position - joints[i].transform.position).normalized; //normalised vector from current joint to end effector position
                    Vector3 vectorCurrJointToTarget = (target.transform.position - joints[i].transform.position).normalized; //normalised vector from current joint to targetposition

                    float cosineAngle = Mathf.Clamp(Vector3.Dot(vectorCurrJointToEndEffector, vectorCurrJointToTarget), -1.0f, 1.0f); //dot product gives the cosine of the angle for the corrective rotation
                                                                                                                                      //clamping to value between -1 and 1 removes any floating errors

                    Vector3 crossProduct = Vector3.Cross(vectorCurrJointToEndEffector, vectorCurrJointToTarget).normalized; //normalised cross product gives the axis on which to rotate the joint

                    float toRotateDegrees = Mathf.Acos(cosineAngle);  //calculate joint rotation, in radians

                    if (dampen) 
                    {
                        toRotateDegrees = dampingValue; //apply damping to joint rotation
                    }

                    toRotateDegrees = toRotateDegrees * Mathf.Rad2Deg; //get rotation in degrees

                    
                    joints[i].transform.rotation = Quaternion.AngleAxis(toRotateDegrees, crossProduct) * joints[i].transform.rotation; //apply joint rotation
                                                                                                                                       //joints[i].transform.Rotate(crossProduct, toRotateDegrees, Space.World);
                }
            }

            currentAttempts++;
        }

        //find distance error from end effector to target
        float currentError = Vector3.Distance(joints[joints.Length - 1].transform.position, target.transform.position);

        //if end effector is within the tolerable error of the target position, a solution has been found
        if (currentError < minError)
        {
            reached = true;
        }
        else
        {
            reached = false;
        }
    }
}