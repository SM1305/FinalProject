using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Unreachable : MonoBehaviour
{
    public GameObject joint0;
    public GameObject joint1;
    public GameObject endEffector;

    public GameObject target;

    public float Angle0;
    public float Angle1;

    public float length0;
    public float length1;
    public float length2;

    public Vector3 Euler0;
    public Vector3 Euler1;





    void Update ()
    {
        //find edge lengths of right angled triangle (formed from positions of arm joints and target position) 
        length0 = Vector2.Distance(joint0.transform.position, joint1.transform.position);
        length1 = Vector2.Distance(joint1.transform.position, endEffector.transform.position);
        length2 = Vector2.Distance(joint0.transform.position, target.transform.position);

        // Inner angle alpha
        float cosAngle0 = ((length2 * length2) + (length0 * length0) - (length1 * length1)) / (2 * length2 * length0);
        float angle0 = Mathf.Acos(cosAngle0) * Mathf.Rad2Deg;

        // Inner angle beta
        float cosAngle1 = ((length1 * length1) + (length0 * length0) - (length2 * length2)) / (2 * length1 * length0);
        float angle1 = Mathf.Acos(cosAngle1) * Mathf.Rad2Deg;

        // Angle from Joint0 and Target
        Vector2 diff = target.transform.position - joint0.transform.position;
        float atan = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        // So they work in Unity reference frame
        float jointAngle0 = atan - angle0; // Angle A
        float jointAngle1 = 180f - angle1; // Angle B


        //Apply rotations to joint objects
        Euler0 = joint0.transform.localEulerAngles;
        Euler0.z = jointAngle0;
        joint0.transform.localEulerAngles = Euler0;

        Euler1 = joint1.transform.localEulerAngles;
        Euler1.z = jointAngle1;
        joint1.transform.localEulerAngles = Euler1;
    }
}
