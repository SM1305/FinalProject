using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public FootStates footStates;
    public bool isGrounded = false;

	
	//void Update ()
 //   {
 //       CheckGrounded();
	//}

 //   private void CheckGrounded()
 //   {
 //       RaycastHit groundHit;
 //       Debug.DrawRay(transform.position, Vector3.down /*(transform.TransformDirection(new Vector3 transform.position.up).normalized)*/, Color.magenta);
 //       if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 1) && isGrounded == false)
 //       {
 //           if (groundHit.transform.name == "Ground")
 //           {
 //               //hitMarker.transform.position = groundHit.point;
 //               Debug.DrawLine(transform.position, groundHit.point, Color.magenta);
 //               isGrounded = true;
 //               footStates.footState = FootStates.FootState.Attach;

 //               //if (Vector3.Distance(transform.position, groundHit.point) > 0.25f && isGrounded == true)
 //               //{
 //               //    isGrounded = false;
 //               //    footStates.footState = FootStates.FootState.Exit;
 //               //}
 //           }
 //           else
 //           {

 //           }
 //       }
 //       else if (isGrounded == true)
 //       {
 //           isGrounded = false;
 //           footStates.footState = FootStates.FootState.Exit;
 //       }
 //   }
}
