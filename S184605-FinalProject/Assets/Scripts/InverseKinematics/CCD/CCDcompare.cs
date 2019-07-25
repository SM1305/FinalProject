using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDcompare : MonoBehaviour
{
    //string as a developer note in inspector, should multiple individual chains of same model be controlled
    public string ThisScriptControls;

    public bool dampen = false;
    public float dampingValue = 0.005f;

    public Joints[] joints;

    public GameObject target;
    private Vector3 targetPosition;

    //private Quaternion currentJointRotation;
    //private Quaternion nextJointRotation;

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

    void LateUpdate()
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
                /*for (int j = joints.Length - 1; j > i - 1 && j < joints.Length; j--) //second loop returns to the furthest joint from the root on each loop. Gives pattern '5' - '5,4' - '5,4,3' - '5,4,3,2' - '5,4,3,2,1'
                {*/
                    //Debug.Log("I: " + i + " | J: " + j + " | Current Joint: " + joints[i].name);

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



                    /* 
                     * THERE IS ONLY PAIN AND SUFFERING FOR YOU FROM HERE 
                     */

                    //currentJointRotation = joints[i].transform.rotation;
                    //nextJointRotation = joints[i].transform.rotation;


                    //Quaternion angleDifference = nextJointRotation * Quaternion.Inverse(joints[i].initialRotation); 

                    //if (angleDifference.x < joints[i].initialRotation.x - joints[i].XaxisMin || angleDifference.x > joints[i].initialRotation.x + joints[i].XaxisMax ||
                    //    angleDifference.y < joints[i].initialRotation.y - joints[i].YaxisMin || joints[i].initialRotation.y + angleDifference.y > joints[i].YaxisMax ||
                    //    angleDifference.z < joints[i].initialRotation.z - joints[i].ZaxisMin || joints[i].initialRotation.z + angleDifference.z > joints[i].ZaxisMax)
                    //{
                    //    joints[i].transform.rotation = currentJointRotation;
                    //}



                    /*
                     * 
                     */

                    //clamp current joint rotation within set limits
                    Vector3 currentJointEulerAngles = joints[i].joint.transform.localRotation.eulerAngles; //get rotation

                    //ensure rotation angles stay relative regardless of rotation extent
                    //X
                    if (currentJointEulerAngles.x > 180f)
                    {
                        currentJointEulerAngles.x -= 360f;
                    }
                    if (currentJointEulerAngles.x < -180f)
                    {
                        currentJointEulerAngles.x += 360f;
                    }
                    currentJointEulerAngles.x = Mathf.Clamp(currentJointEulerAngles.x, joints[i].XaxisMin, joints[i].XaxisMax); //clamp x

                    //Y
                    if (currentJointEulerAngles.y > 180f)
                    {
                        currentJointEulerAngles.y -= 360f;
                    }
                    if (currentJointEulerAngles.y < -180f)
                    {
                        currentJointEulerAngles.y += 360f;
                    }
                    currentJointEulerAngles.y = Mathf.Clamp(currentJointEulerAngles.y, joints[i].YaxisMin, joints[i].YaxisMax); //clamp y

                    //Z
                    if (currentJointEulerAngles.z > 180f)
                    {
                        currentJointEulerAngles.z -= 360f;
                    }
                    if (currentJointEulerAngles.z < -180f)
                    {
                        currentJointEulerAngles.z += 360f;
                    }
                    currentJointEulerAngles.z = Mathf.Clamp(currentJointEulerAngles.z, joints[i].ZaxisMin, joints[i].ZaxisMax); //clamp z

                    joints[i].joint.transform.localEulerAngles = currentJointEulerAngles; //set clamped rotation


                    /*
                     * 
                    // */

                    //Vector3 currentJointEulerAngles = joints[i].transform.localRotation.eulerAngles; //get rotation

                    //if (joints[i].XisRestricted)
                    //{
                    //    if (currentJointEulerAngles.x > 180f)
                    //        currentJointEulerAngles.x -= 360f;
                    //    currentJointEulerAngles.x = Mathf.Clamp(currentJointEulerAngles.x, joints[i].XaxisMin, joints[i].XaxisMax);
                    //}

                    //if (joints[i].YisRestricted)
                    //{
                    //    if (currentJointEulerAngles.y > 180f)
                    //        currentJointEulerAngles.y -= 360f;
                    //    currentJointEulerAngles.y = Mathf.Clamp(currentJointEulerAngles.y, joints[i].YaxisMin, joints[i].YaxisMax);
                    //}

                    //if (joints[i].ZisRestricted)
                    //{
                    //    if (currentJointEulerAngles.z > 180f)
                    //        currentJointEulerAngles.z -= 360f;
                    //    currentJointEulerAngles.z = Mathf.Clamp(currentJointEulerAngles.z, joints[i].ZaxisMin, joints[i].ZaxisMax);
                    //}

                    //joints[i].transform.localEulerAngles = currentJointEulerAngles;








                    /*
                     * 
                     */

                    //Vector3 angleDifference = expectedRotation - baseRotation; //baseRotation is just the initial rotation from which the bone limits are calculated.

                    //if (angleDifference.X < boneLimits.minRotation.X || angleDifference.Y < boneLimits.minRotation.Y || angleDifference.Z < boneLimits.minRotation.Z || angleDifference.X > boneLimits.maxRotation.X || angleDifference.Y > boneLimits.maxRotation.Y || angleDifference.Z > boneLimits.maxRotation.Z)
                    //    return currentRotation;

                    //return expectedRotation;

   //             }
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

    //private void Constrain()
    //{
    //    foreach (Joints joint in joints)
    //    {
    //        Vector3 currentJointEulerAngles = joint.joint.transform.localRotation.eulerAngles; //get rotation

    //        if (currentJointRotation.x > 5)
    //        {

    //        }
    //    }
    //}
}







// RotationConstraint constrains the relative rotation of a
// Transform. You select the constraint axis in the editor and
// specify a min and max amount of rotation that is allowed
// from the default rotation
//////////////////////////////////////////////////////////////

//enum ConstraintAxis
//{
//    X = 0,
//    Y,
//    Z
//}

//public var axis : ConstraintAxis;           // Rotation around this axis is constrained
//public var min : float;                     // Relative value in degrees
//public var max : float;                     // Relative value in degrees
//private var thisTransform : Transform;
//private var rotateAround : Vector3;
//private var minQuaternion : Quaternion;
//private var maxQuaternion : Quaternion;
//private var range : float;

//function Start()
//{
//    thisTransform = transform;

//    // Set the axis that we will rotate around
//    switch (axis)
//    {
//        case ConstraintAxis.X:
//            rotateAround = Vector3.right;
//            break;

//        case ConstraintAxis.Y:
//            rotateAround = Vector3.up;
//            break;

//        case ConstraintAxis.Z:
//            rotateAround = Vector3.forward;
//            break;
//    }

//    // Set the min and max rotations in quaternion space
//    var axisRotation = Quaternion.AngleAxis(thisTransform.localRotation.eulerAngles[axis], rotateAround);
//    minQuaternion = axisRotation * Quaternion.AngleAxis(min, rotateAround);
//    maxQuaternion = axisRotation * Quaternion.AngleAxis(max, rotateAround);
//    range = max - min;
//}

//// We use LateUpdate to grab the rotation from the Transform after all Updates from
//// other scripts have occured
//function LateUpdate()
//{
//    // We use quaternions here, so we don't have to adjust for euler angle range [ 0, 360 ]
//    var localRotation = thisTransform.localRotation;
//    var axisRotation = Quaternion.AngleAxis(localRotation.eulerAngles[axis], rotateAround);
//    var angleFromMin = Quaternion.Angle(axisRotation, minQuaternion);
//    var angleFromMax = Quaternion.Angle(axisRotation, maxQuaternion);

//    if (angleFromMin <= range  angleFromMax <= range )
//        return; // within range
//    else
//    {
//        // Let's keep the current rotations around other axes and only
//        // correct the axis that has fallen out of range.
//        var euler = localRotation.eulerAngles;
//        if (angleFromMin > angleFromMax)
//            euler[axis] = maxQuaternion.eulerAngles[axis];
//        else
//            euler[axis] = minQuaternion.eulerAngles[axis];

//        thisTransform.localEulerAngles = euler;
//    }
//}