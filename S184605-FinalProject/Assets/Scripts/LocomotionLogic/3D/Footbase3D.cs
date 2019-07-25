using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footbase3D : MonoBehaviour
{
    public GameObject heelJoint;
    public GameObject ballJoint;
    public GameObject toeJoint;

    public float checkLength;
    private float heelCheckLength;

    public float rotateSpeed;

    public LayerMask GroundLayer;

    private RaycastHit heelHit;
    private RaycastHit midHit;
    private RaycastHit toeHit;
       
    void Start()
    {
        heelCheckLength = checkLength + 0.1f;
    }

	void Update ()
    {
        if (CheckHeelContact() && CheckMidContact())
        {
            Vector3 averageNormal = (heelHit.normal + midHit.normal) * 0.5f;

            Quaternion heelTargetRotation = Quaternion.FromToRotation(heelJoint.transform.up, averageNormal) * transform.rotation;
            heelJoint.transform.rotation = Quaternion.Slerp(heelJoint.transform.rotation, heelTargetRotation, Time.deltaTime * rotateSpeed);

            Quaternion ballTargetRotation = Quaternion.FromToRotation(ballJoint.transform.up, averageNormal) * transform.rotation;
            ballJoint.transform.rotation = Quaternion.Slerp(ballJoint.transform.rotation, ballTargetRotation, Time.deltaTime * rotateSpeed);
        }
        else if (CheckHeelContact())
        {
            Quaternion targetRotation = Quaternion.FromToRotation(heelJoint.transform.up, heelHit.normal) * transform.rotation;
            heelJoint.transform.rotation = Quaternion.Slerp(heelJoint.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
        else if (CheckMidContact())
        {
            Quaternion targetRotation = Quaternion.FromToRotation(ballJoint.transform.up, midHit.normal) * transform.rotation;
            ballJoint.transform.rotation = Quaternion.Slerp(ballJoint.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        if (CheckToeContact())
        {
            Quaternion targetRotation = Quaternion.FromToRotation(toeJoint.transform.up, toeHit.normal) * transform.rotation;
            toeJoint.transform.rotation = Quaternion.Slerp(toeJoint.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    private bool CheckHeelContact()
    {
        if (Physics.Raycast(heelJoint.transform.position, Vector3.down, out heelHit, heelCheckLength, GroundLayer))
        {
            Debug.DrawLine(heelJoint.transform.position, heelHit.point, Color.red);

            if (heelHit.transform.name == "Ground")
            {
                return true;
            }

            return false;

        }

        else
        {
            return false;
        }
    }

    private bool CheckMidContact()
    {
        if (Physics.Raycast(ballJoint.transform.position, Vector3.down, out midHit, checkLength, GroundLayer))
        {
            Debug.DrawLine(ballJoint.transform.position, midHit.point, Color.red);

            if (midHit.transform.name == "Ground")
            {
                return true;
            }

            return false;

        }
        else return false;
    }


    private bool CheckToeContact()
    {
        if (Physics.Raycast(toeJoint.transform.position, Vector3.down, out toeHit, checkLength, GroundLayer))
        {
            Debug.DrawLine(toeJoint.transform.position, toeHit.point, Color.red);

            if (toeHit.transform.name == "Ground")
            {
                return true;
            }

            return false;

        }
        else return false;
    }
}
