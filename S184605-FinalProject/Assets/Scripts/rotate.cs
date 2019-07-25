using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public GameObject spine;
    public float pivotSpeed;
    public float pivotAngle;
    public float pivotRadius;



    //public float radius = 2f;      //Distance from the center of the circle to the edge
    //public float currentAngle = 0f; //Our angle, this public for debugging purposes
    //private float speed = 0f;      //Rate at which we'll move around the circumference of the circle
    //public float timeToCompleteCircle = 1.5f; //Time it takes to complete a full circle

    void Start ()
    {
        //speed = (Mathf.PI * 2) / timeToCompleteCircle;
    }
	
	void Update ()
    {
        //spine.transform.RotateAround(transform.position, transform.forward, -pivotSpeed * Time.deltaTime);

        //transform.localPosition = Quaternion.AngleAxis(pivotAngle * Time.deltaTime, Vector3.forward) * spine.transform.localPosition;



        //pivotAngle += pivotSpeed * Time.deltaTime;

        //Vector3 offset = new Vector3(Mathf.Sin(pivotAngle), Mathf.Cos(pivotAngle)) * pivotRadius;
        //transform.position = transform.position + offset;

        //spine.transform.RotateAround(transform.position, transform.forward, pivotSpeed * Time.deltaTime);




        //speed = (Mathf.PI * 2) / timeToCompleteCircle; //For level design purposes
        //currentAngle += Time.deltaTime * speed; //Changes the angle
        //float newX = radius * Mathf.Cos(currentAngle);
        //float newY = radius * Mathf.Sin(currentAngle);
        //transform.position = new Vector3(newX, newY, transform.position.z);




        //spine.transform.position = RotatePointAroundPivot(transform.position, pivotSquare.transform.position, Quaternion.Euler(0, 0, angle));
    }
 
 
// public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle)
//{
//    return angle * (point - pivot) + pivot;
//}

}
