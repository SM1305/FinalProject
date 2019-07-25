using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematics : MonoBehaviour
{
    public float[] angles;
    public GameObject[] Joints;
    public GameObject endEffector;
    public GameObject target;

    void Update()
    {
        float D1 = Vector2.Distance(Joints[0].transform.position, Joints[1].transform.position);
        //float D2 = Vector2.Distance(Joints[1].transform.position, Joints[2].transform.position);

        angles[0] = Joints[0].transform.rotation.z;
        angles[1] = Joints[1].transform.rotation.z;
        //angles[2] = Joints[2].transform.rotation.z;


        //angles[0] = Vector2.Angle(Joints[1].transform.position, Joints[0].transform.position);
        //angles[1] = Vector2.Angle(Joints[2].transform.position, Joints[1].transform.position);

        //angles[0] = Joints[0].transform.rotation.z;
        //angles[1] = Joints[1].transform.rotation.z;

        //target.transform.position = FK();

        FK();
    }


    //returns position of end effector, calculated from rotation of all joints in chain.
    public void FK()
    {
        //start at base joint
        Vector3 prevJoint = Joints[0].transform.position;
        //create  quaternion variable, rotation
        Quaternion rotation = transform.rotation;

        //for the number of joints in the FK chain
        for (int i = 1; i < Joints.Length; i++) 
        {

            rotation *= Quaternion.AngleAxis(angles[i - 1], Joints[i - 1].GetComponent<ArmJoinsInformation>().rotationAxis);

            Vector3 nextJoint = prevJoint + rotation * Joints[i].GetComponent<ArmJoinsInformation>().startOffset;

            prevJoint = nextJoint;
        }

        //return the end effector position
        target.transform.position = prevJoint;
    }
}
