using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStates : MonoBehaviour
{
    public enum FootState { onStart, Attach, Support, PreExit, Exit, Swing };
    public FootState footState;


    private void Start()
    {
        footState = FootState.onStart;
    }
}
