using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepBlue : MonoBehaviour
{
    public FootStates footStates;
    public FootPlacement footPlacement;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            footStates.footState = FootStates.FootState.Attach;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            footStates.footState = FootStates.FootState.Swing;
        }
    }

    private void Update()
    {
        Step();
    }

    void Step()
    {
        switch (footStates.footState)
        {
            case FootStates.FootState.onStart:
                Debug.Log("onstart working NOT in update");
                break;

            case FootStates.FootState.Attach:
                Debug.Log("attach working NOT in update");
                footPlacement.targetRed.transform.position = footPlacement.nextTarget.transform.position;
                footStates.footState = FootStates.FootState.Support;
                break;

            case FootStates.FootState.Support:
                Debug.Log("support working NOT in update");

                break;

            case FootStates.FootState.Swing:
                Debug.Log("swing working NOT in update");
                break;
        }
    }

    void DefineControlPointPositions()
    {
        //controlPoint0.transform.position = END_EFFECTOR.transform.position;
        //controlPoint1.transform.position = HIP.transform.position;
        //controlPoint2.transform.position = rayhit.transform.position + y axis uplift;
        //controlPoint3.transform.position = rayhit.transform.position;
    }
}
