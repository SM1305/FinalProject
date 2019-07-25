using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFootStep : MonoBehaviour
{
    public FootPlacement footPlacement;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            footPlacement.footStep = FootPlacement.FootStep.RedAttach;
        }
    }
}
