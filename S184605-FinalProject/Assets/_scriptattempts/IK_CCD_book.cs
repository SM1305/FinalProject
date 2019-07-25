using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_CCD_book : MonoBehaviour
{
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

    
    void Start()
    {
        targetPosition = target.transform.position;
    }
    
    void Update() 
    {
        // the target has moved, reset attepmts to 0 and change targetPosition
        if (target.transform.position != targetPosition)
        {
            currentAttempts = 0;
            targetPosition = target.transform.position;
        }

        //iterates through all joints each frame
        if (reached == false && currentAttempts <= MaxAttempts)
        {
            for (int i = joints.Length - 1; i >= 0; i--) //start from the end effector, move through each joint towards the root joint
            {                
                Vector3 vectorCurrJointToEndEffector = (joints[joints.Length - 1].transform.position - joints[i].transform.position).normalized; //normalised vector from current joint to end effector position
                Vector3 vectorCurrJointToTarget = (target.transform.position - joints[i].transform.position).normalized; //normalised vector from current joint to targetposition

                float cosineAngle;
                float turnAngle;

                cosineAngle = Vector3.Dot(vectorCurrJointToEndEffector, vectorCurrJointToTarget); //dot product gives the cosine of the angle for the corrective rotation
                
                // IF THE DOT PRODUCT RETURNS 1.0, I DON'T NEED TO ROTATE AS IT IS 0 DEGREES
                if (cosineAngle < 1)
                {
                    Vector3 crossResult = Vector3.Cross(vectorCurrJointToEndEffector, vectorCurrJointToTarget).normalized; //normalised cross product gives the axis on which to rotate the joint

                    turnAngle = Mathf.Acos(cosineAngle) * Mathf.Rad2Deg; //calculate joint rotation in degrees

                    joints[i].transform.rotation = Quaternion.AngleAxis(turnAngle, crossResult) * joints[i].transform.rotation; //apply joint rotation
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
