using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLegIntersections3D : MonoBehaviour
{
    public GameObject hip;
    public GameObject knee;
    public GameObject foot;


    public CCD ccd;
    public GameObject ccdTarget;
    public GameObject ccdOriginalTarget;


	void Update ()
    {
        ScanLegForIntersections();
    }

    private bool ScanLegForIntersections()
    {
        RaycastHit thighHit;
        if (Physics.Linecast(hip.transform.position, knee.transform.position, out thighHit))
        {
            ccdTarget.transform.position = thighHit.point;

            ccd.target = ccdTarget;

            return true;
        }
        else
        {
            RaycastHit shinHit;
            if (Physics.Linecast(knee.transform.position, foot.transform.position, out shinHit))
            {
                ccdTarget.transform.position = shinHit.point;

                ccd.target = ccdTarget;

                return true;
            }



            ccd.target = ccdOriginalTarget;

            return false;
        }
    }
}
