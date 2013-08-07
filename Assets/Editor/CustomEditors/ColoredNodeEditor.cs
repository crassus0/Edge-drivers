using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ColoredNode))]
public class ColoredNodeEditor : Editor
{
  public void OnEnable()
  {
    ColoredNode targ = target as ColoredNode;
    targ.m_visualiser.renderer.sharedMaterial = new Material(targ.m_visualiser.renderer.sharedMaterial);
  }
  public override void OnInspectorGUI()
  {
    int[] colorFields = { 0, 1, 2 };
    string[] colorNames = { "Red", "Green", "Blue" };
    ColoredNode targ = target as ColoredNode;
    targ.color = EditorGUILayout.IntPopup("Color", targ.color, colorNames, colorFields);
    targ.ChangeVisual();
  }
}
