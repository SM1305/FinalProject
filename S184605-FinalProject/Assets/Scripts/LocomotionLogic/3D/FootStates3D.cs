using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStates3D : MonoBehaviour
{
    public enum FootState { Attach, Support, PreExit, Exit, Swing };
    public FootState footState;
}