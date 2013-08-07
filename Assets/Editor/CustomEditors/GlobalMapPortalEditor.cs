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
    List<TerraformingMine> path = (target as GlobalMapPortal).m_path;
    if (showArray)
    {
      int arraySize = EditorGUILayout.IntField("    Size", (target as GlobalMapPortal).m_path.Count);
      if (arraySize != path.Count)
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
    if (size > (target as GlobalMapPortal).m_path.Count)
      size = (target as GlobalMapPortal).m_path.Count;
    Array.Copy((target as GlobalMapPortal).m_path.ToArray(), mines, size);
    if (size > 0)
      for (int i = size; i < mines.Length; i++)
      {
        mines[i] = mines[size - 1];
      }
    (target as GlobalMapPortal).m_path = new List<TerraformingMine>(mines);
  }
}
