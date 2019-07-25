using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BipedMovementInput : MonoBehaviour
{
    public Rigidbody hips;

    [Header("[Velocity]")]
    public float velocity = 0f;
    public float maxVelocity = 0f;
    private float minVelocity = 2.9f;
    public float Acceleration = 0f;
    [Header("[Turning]")]
    public float turnStrength = 0f;
    public float maxTurnStrength = 0f;
    public float turnAcceleration = 0f;


    void Start()
    {
        velocity = 0.29f;
    }

    //handles input for acceleration, deceleration, turning. Fixed Update applies turn value to hip joint (biped root joint)
	void Update ()
    {
        //accelerate
        if ((Input.GetKey(KeyCode.W) && velocity < maxVelocity))       
            velocity += Acceleration * Time.deltaTime;
        //decelerate
        else if ((Input.GetKey(KeyCode.S) && velocity > minVelocity)) 
           velocity -= Acceleration * Time.deltaTime;


        //left
        if (Input.GetKey(KeyCode.A))
            Mathf.Clamp(turnStrength -= turnAcceleration * Time.deltaTime, 0, 1.1f);
        //right
        else if (Input.GetKey(KeyCode.D))
            Mathf.Clamp(turnStrength += turnAcceleration * Time.deltaTime, 0, 1.1f);
        //not left or right, decay
        else
            turnStrength = Mathf.Lerp(turnStrength, 0f, 10f * Time.deltaTime);


        //keep within bounds
        if (turnStrength >= maxTurnStrength)
            turnStrength = maxTurnStrength;
        if (turnStrength <= -maxTurnStrength)
            turnStrength = -maxTurnStrength;

        if (Acceleration >= maxVelocity)
            Acceleration = maxVelocity;
        if (Acceleration <= 0)
            Acceleration = 0;

        if (velocity >= maxVelocity)
            velocity = maxVelocity;
        if (velocity <= minVelocity)
            velocity = minVelocity;
    }

    //apply rotation value to hips
    private void FixedUpdate()
    {
        hips.AddRelativeTorque(Vector3.up * turnStrength * Time.deltaTime);
    }
}
