using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GlobalMapPortal))]
public class GlobalMapPortalEditor : DistantPortalEditor
{
  bool showArray = false;

  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();
    showArray = EditorGUILayout.Foldout(showArray, "Path formers");
    TerraformingMine[] path = (target as GlobalMapPortal).m_path;
    if (showArray)
    {
      int arraySize = EditorGUILayout.IntField("    Size", (target as GlobalMapPortal).m_path.Length);
      if (arraySize != path.Length)
        NewArraySize(arraySize);
      for (int i = 0; i < arraySize; i++)
      {
        //Debug.Log(i);
        path[i] = EditorGUILayout.ObjectField("    Element " + i, path[i], typeof(TerraformingMine), true) as TerraformingMine;
      }
    }
  }
  public void NewArraySize(int size)
  {
    TerraformingMine[] mines = new TerraformingMine[size];
    if (size > (target as GlobalMapPortal).m_path.Length)
      size = (target as GlobalMapPortal).m_path.Length;
    Array.Copy((target as GlobalMapPortal).m_path, mines, size);
    if (size > 0)
      for (int i = size; i < mines.Length; i++)
      {
        mines[i] = mines[size - 1];
      }
    (target as GlobalMapPortal).m_path = mines;
  }
}
