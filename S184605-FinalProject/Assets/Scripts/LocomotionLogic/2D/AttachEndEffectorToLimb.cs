using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachEndEffectorToLimb : MonoBehaviour
{
    public GameObject attachPoint;

	void Update ()
    {
        transform.position = attachPoint.transform.position;
	}
}
