using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmJoinsInformation : MonoBehaviour
{
    public Vector3 startOffset;
    public Vector3 rotationAxis;


    private void Awake()
    {
        startOffset = transform.localPosition;
    }
}
