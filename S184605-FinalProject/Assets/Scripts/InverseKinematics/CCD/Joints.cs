using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joints : MonoBehaviour
{
    public GameObject joint;

    public bool XisRestricted;
    public float XaxisMin;
    public float XaxisMax;

    public bool YisRestricted;
    public float YaxisMin;
    public float YaxisMax;

    public bool ZisRestricted;
    public float ZaxisMin;
    public float ZaxisMax;

    public Quaternion initialRotation;



    void Awake()
    {
        joint = gameObject;
    }

    private void Start()
    {
        initialRotation = joint.transform.rotation;
    }

    void LateUpdate()
    {
        //clamp current joint rotation within set limits
        //Vector3 currentJointEulerAngles = joint.transform.eulerAngles;

        //if (currentJointEulerAngles.x > 180f)
        //    currentJointEulerAngles.x -= 360f;
        //currentJointEulerAngles.x = Mathf.Clamp(currentJointEulerAngles.x, XaxisMin, XaxisMax);

        //if (currentJointEulerAngles.y > 180f)
        //    currentJointEulerAngles.y -= 360f;
        //currentJointEulerAngles.y = Mathf.Clamp(currentJointEulerAngles.y, YaxisMin, YaxisMax);

        //if (currentJointEulerAngles.z > 180f)
        //    currentJointEulerAngles.z -= 360f;
        //currentJointEulerAngles.z = Mathf.Clamp(currentJointEulerAngles.z, ZaxisMin, ZaxisMax);

        //joint.transform.localEulerAngles = currentJointEulerAngles;
    }
}
