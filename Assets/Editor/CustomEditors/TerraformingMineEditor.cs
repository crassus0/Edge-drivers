using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerraformingMine))]
[CanEditMultipleObjects]
public class TerraformingMineEditor:Editor
{
  bool showArray = true;
  int[] states = { 0, 1, 2, 3, 6 };
  string[] stateNames = { "Cut", "Soft", "Absolute", "Strong", "Non changed" };

  public override void OnInspectorGUI()
  {
    TerraformingMine targ = target as TerraformingMine;
    showArray = EditorGUILayout.Foldout(showArray, "Strings");
    string zeroName = targ.Node.Index == 0 ? "  Top" : "  Bottom";
    if (showArray)
    {
      targ.states[0] =(byte) EditorGUILayout.IntPopup(zeroName, targ.states[0], stateNames, states);
      targ.states[1] = (byte)EditorGUILayout.IntPopup("  Left", targ.states[1], stateNames, states);
      targ.states[2] = (byte)EditorGUILayout.IntPopup("  Right", targ.states[2], stateNames, states);
    }
    
    targ.steps = EditorGUILayout.IntSlider("Steps", targ.steps, -1, 50);
    targ.ActivateOnStart = EditorGUILayout.Toggle("Activate on start", targ.ActivateOnStart);
  }
}

