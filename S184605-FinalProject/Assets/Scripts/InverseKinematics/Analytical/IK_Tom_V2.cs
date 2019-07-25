using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Tom_V2 : MonoBehaviour
{
    [Header("Arm Joints")]
    public GameObject joint0;
    public GameObject joint1;
    public GameObject endEffector;
    private float length0;
    private float length1;
    private float length2;
    private float a, b, c;

    [Header("Arm Action")]
    public bool bendRight = true;
    public float smooth;
    private float velocity0 = 0;
    private float velocity1 = 0;

    [Header("Target")]
    public GameObject target;

    [Header("Debug")]
    public bool showGiz;


    private void Start()
    {
        length0 = Vector2.Distance(joint0.transform.position, joint1.transform.position);
        length1 = Vector2.Distance(joint1.transform.position, endEffector.transform.position);
    }

    void Update()
    {
        CalculateLimbIK();
    }

    void CalculateLimbIK()
    {
        length2 = Vector2.Distance(joint0.transform.position, target.transform.position);

        //interior angle a
        a = (Mathf.Acos(Mathf.Clamp(((length0 * length0) + (length1 * length1) - (length2 * length2)) / (2 * length0 * length1), -1.0f, 1.0f))) * Mathf.Rad2Deg;

        //interior angle b
        b = (Mathf.Acos(Mathf.Clamp(((length0 * length0) + (length2 * length2) - (length1 * length1)) / (2 * length0 * length2), -1.0f, 1.0f))) * Mathf.Rad2Deg;

        //angle from base of arm to target
        Vector2 difference = target.transform.position - joint0.transform.position;
        c = (Mathf.Atan2(difference.y, difference.x)) * Mathf.Rad2Deg;


        ////arm bend
        //if (bendRight)
        //{
        //    a = c + a;
        //    b = 180f + b;
        //}
        //else
        //{
        //    a = c - b;
        //    b = 180f - b;
        //}


        //apply rotations to arm joints
        Vector3 joint0Angle = joint0.transform.localEulerAngles;
        //joint0Angle.z = b + c;
        joint0Angle.z = Mathf.SmoothDampAngle(joint0Angle.z, (b + c), ref velocity0, smooth);
        joint0.transform.localEulerAngles = joint0Angle;

        Vector3 joint1Angle = joint1.transform.localEulerAngles;
        //joint1Angle.z = (Mathf.PI * Mathf.Rad2Deg) + a;
        joint1Angle.z = Mathf.SmoothDampAngle(joint1Angle.z, (Mathf.PI * Mathf.Rad2Deg) + a, ref velocity1, smooth);
        joint1.transform.localEulerAngles = joint1Angle;
    }

    private void OnDrawGizmos()
    {
        if (showGiz)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(joint0.transform.position, endEffector.transform.position);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(joint0.transform.position, joint1.transform.position);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(joint1.transform.position, endEffector.transform.position);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(endEffector.transform.position, target.transform.position);
        }
    }
}
