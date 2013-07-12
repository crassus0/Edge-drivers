using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DistantPortalExit))]
public class DistantPortalExitEditor : Editor 
{
  public override void OnInspectorGUI ()
  {
    (target as DistantPortalExit).Direction = EditorGUILayout.IntSlider("Direction", (target as DistantPortalExit).Direction, -1,5); 
    EditorUtility.SetDirty(target);
  }
}
