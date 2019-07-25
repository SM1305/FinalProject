using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCD_2 : MonoBehaviour
{
    public int[] numbers;

    public GameObject[] joints;
    public GameObject target;
    private Vector3 targetPosition;

    //if the target is reached
    public bool reached = false;

    //current number of attempts to reach target
    public int currentAttempts = 0;
    //maximum number of attempts to reach target before stopping
    public int MaxAttempts = 10;

    //acceptable reached error
    public float minError = 0.1f;

    //the sine component for each joint
    private float sin;
    // The cosine component for each joint
    private float cos;
    //array of angles to rotate by (for each joint)
    private float theta;


    void Start()
    {
        targetPosition = target.transform.position;
    }


    void Update() //iterates each joint each frame
    {
        // the target has moved, reset attepmts to 0 and change targetPosition
        if (target.transform.position != targetPosition)
        {
            currentAttempts = 0;
            targetPosition = target.transform.position;
        }

        if (reached == false && currentAttempts <= MaxAttempts)
        {
            for (int i = joints.Length - 1; i >= 0; i--) //start from the end effector and work towards root joint
            {
                Vector3 vectorCurrJointToEndEffector = joints[joints.Length - 1].transform.position - joints[i].transform.position; //vector from current joint to end effector position
                Vector3 vectorCurrJointToTarget = target.transform.position - joints[i].transform.position; //vector from current joint to targetposition


                //find the components using dot product
                cos = Vector3.Dot(vectorCurrJointToEndEffector, vectorCurrJointToTarget) / (vectorCurrJointToEndEffector.magnitude * vectorCurrJointToTarget.magnitude);
                //find the components using cross product
                sin = Vector3.Cross(vectorCurrJointToEndEffector, vectorCurrJointToTarget).magnitude / (vectorCurrJointToEndEffector.magnitude * vectorCurrJointToTarget.magnitude);


                //the axis of rotation is unit vector along the cross product 
                Vector3 rotationAxis = (Vector3.Cross(vectorCurrJointToEndEffector, vectorCurrJointToTarget)) / (vectorCurrJointToEndEffector.magnitude * vectorCurrJointToTarget.magnitude);


                // find the angle between vectorCurrJointToEndEffector and vectorCurrJointToTarget (and clamp values of cos to avoid errors)
                theta = Mathf.Acos(Mathf.Max(-1, Mathf.Min(1, cos)));

                // obtain an angle value between -pi and pi, and then convert to degrees
                theta = (float)SimpleAngle(theta) * Mathf.Rad2Deg;
                // rotate the ith joint along the axis by theta degrees in the world space.
                joints[i].transform.Rotate(rotationAxis, theta, Space.World);

            }

            currentAttempts++;
        }

        // find the difference in the positions of the end effector and the target
        float xError = Mathf.Abs(joints[joints.Length - 1].transform.position.x - target.transform.position.x);
        float yError = Mathf.Abs(joints[joints.Length - 1].transform.position.y - target.transform.position.y);
        float zError = Mathf.Abs(joints[joints.Length - 1].transform.position.z - target.transform.position.z);

        // if target is within reach (within epsilon) then the process is done
        if (xError < minError && yError < minError && zError < minError)
        {
            reached = true;
        }
        else
        {
            reached = false;
        }
    }

    // function to convert an angle to its simplest form (between -pi to pi radians)
    double SimpleAngle(double theta)
    {
        theta = theta % (2.0 * Mathf.PI);
        if (theta < -Mathf.PI)
            theta += 2.0 * Mathf.PI;
        else if (theta > Mathf.PI)
            theta -= 2.0 * Mathf.PI;
        return theta;
    }
}
