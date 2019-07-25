using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSceneWindow : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
    }
}
