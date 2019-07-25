﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public GameObject blueEndEffector;
    public GameObject redEndEffector;
    public GameObject targetBlue;
    public GameObject targetRed;

    public GameObject nextTarget;

    public GameObject hips;
    public GameObject verticalRay;

    public bool isRedStepping = false;

    public float strideLength;
    public float spineSpeed;

    public bool showGiz = false;


    private void Awake()
    {
        strideLength = Vector2.Distance(targetBlue.transform.position, targetRed.transform.position) * 1.4f;
    }

    void Update()
    {
        PositionRaycastAhead();

        //hips.transform.Translate(new Vector3(1, 0, 0) * spineSpeed * Time.deltaTime);

        RaycastFootPlacement();
    }

    void PositionRaycastAhead()
    {
        Vector3 raycastPos = verticalRay.transform.position;
        Vector3 hipPos = hips.transform.position;
        raycastPos.x = hipPos.x + strideLength;
        verticalRay.transform.position = raycastPos;
    }

    void RaycastFootPlacement()
    {
        RaycastHit hit;
        if (Physics.Raycast(verticalRay.transform.position, -verticalRay.transform.up, out hit))
        {
            Debug.DrawRay(verticalRay.transform.position, -verticalRay.transform.up * hit.distance, Color.yellow);
            nextTarget.transform.position = hit.point;
        }
        else
        {
            Debug.DrawRay(verticalRay.transform.position, -verticalRay.transform.up * 7, Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        if (showGiz)
        {
            //blue limb reach
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(blueEndEffector.transform.position, targetBlue.transform.position);
            //red limb reach
            Gizmos.color = Color.red;
            Gizmos.DrawLine(redEndEffector.transform.position, targetRed.transform.position);

            //hip to raycaster
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(hips.transform.position, verticalRay.transform.position);
            //next target position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(nextTarget.transform.position, 0.5f);
            //hip to next target
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(hips.transform.position, nextTarget.transform.position);

            //blue target
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(targetBlue.transform.position, 0.5f);
            //red target
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetRed.transform.position, 0.5f);
        }
    }
}
