using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ColouredNode))]
public class ColouredNodeEditor : Editor
{
  public void OnEnable()
  {
    ColouredNode targ = target as ColouredNode;
    targ.m_visualiser.renderer.sharedMaterial = new Material(targ.m_visualiser.renderer.sharedMaterial);
  }
  public override void OnInspectorGUI()
  {
    int[] colorFields = { 0, 1, 2 };
    string[] colorNames = { "Red", "Green", "Blue" };
    ColouredNode targ = target as ColouredNode;
    targ.color = EditorGUILayout.IntPopup("Color", targ.color, colorNames, colorFields);
    targ.ChangeVisual();
  }
}
