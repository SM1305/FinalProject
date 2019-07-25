using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Tom : MonoBehaviour
{
    public GameObject joint0;
    public GameObject joint1;
    public GameObject endEffector;

    public GameObject target;

    public float length0;
    public float length1;
    public float l;

    public Vector3 joint0EulerZ;
    public Vector3 joint1EulerZ;


    void Update ()
    {
        //distance from shoulder to elbow
        length0 = Vector2.Distance(joint0.transform.position, joint1.transform.position);
        //distance from elbow to hand
        length1 = Vector2.Distance(joint1.transform.position, endEffector.transform.position);

        //l*l = x*x + y*y
        //l = distance from shoulder to target
        float l = Mathf.Sqrt(target.transform.position.x * target.transform.position.x + target.transform.position.y * target.transform.position.y);

        //a = cos*-1((length0*length0 + length1*length1 - l*l) / (2*length0 + 2*length1)
        //interior angle a
        float a = Mathf.Acos(((length0 * length0 + length1 * length1 - l * l) / (2 * length0 * length1)));

        //b = cos-1((length0*length0 + l*l - length1*length1) / (2*length0 + 2*l)
        //interior angle b
        float b = Mathf.Acos(((length0 * length0 + l * l - length1 * length1) / (2 * length0 * l)));

        //c = tan-1(x/y)
        //angle to target position
        float c = Mathf.Atan2(target.transform.position.y, target.transform.position.x);


        //for shoulder joint, convert radians to degrees, apply euler angles to joint
        joint0EulerZ = joint0.transform.eulerAngles;
        float bPlusC = b + c;
        float joint0deg = bPlusC * Mathf.Rad2Deg;
        joint0EulerZ.z = joint0deg;
        joint0.transform.localEulerAngles = joint0EulerZ;

        //for elbow joint, convert radians to degrees, apply euler angles to joint
        joint1EulerZ = joint1.transform.eulerAngles;
        float piPlusA = 3.14f + a;
        float joint1deg = piPlusA * Mathf.Rad2Deg;
        joint1EulerZ.z = joint1deg;
        joint1.transform.localEulerAngles = joint1EulerZ;
    }
}
