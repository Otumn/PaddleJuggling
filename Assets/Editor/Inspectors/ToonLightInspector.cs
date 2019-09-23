using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ToonDirectionalLight))]
[CanEditMultipleObjects]
public class ToonLightInspector : Editor
{
    private ToonDirectionalLight light { get => target as ToonDirectionalLight; }

    private void OnSceneGUI()
    {
        Handles.ArrowHandleCap(0, light.transform.position, light.transform.rotation, 1f, EventType.Repaint);
        Handles.SphereHandleCap(0, light.transform.position, Quaternion.identity, 0.2f, EventType.Repaint);
    }
}
