using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Portal))]

public class PortalEditor : Editor
{
  public override void OnInspectorGUI()
  {
    (target as Portal).PairPortal = EditorGUILayout.ObjectField("Destination", (target as Portal).PairPortal, typeof(Portal), !EditorUtility.IsPersistent(target)) as Portal;
    (target as Portal).Direction = EditorGUILayout.IntSlider("Direction", (target as Portal).Direction, -1,5); 
    EditorUtility.SetDirty(target);
  }
}
