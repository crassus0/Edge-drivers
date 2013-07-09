using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{
  int m_selectedIndex=-1;
  string[] m_names;
  ObjectSpawner targ;
  bool m_hasAutoMove=false;
  void OnEnable()
  {
    targ = target as ObjectSpawner;
    m_names = EditorAdditionalGUI.EditorOptions.objectNames;
    if (targ.prefab != null)
    {
      m_hasAutoMove = (targ.prefab.GetComponent<CustomObject>() as IAutoMove != null);
      for (int i = 0; i < m_names.Length; i++)
      {
        if (targ.prefab.name.Equals(m_names[i] + "Prefab"))
          m_selectedIndex = i;
      }
    }
  }
  public override void OnInspectorGUI()
  {
    
    targ.cooldown = EditorGUILayout.IntField("Cooldown", targ.cooldown);
    int[] arr = new int[m_names.Length];
    for (int i = 0; i < arr.Length; i++)
      arr[i] = i;
    m_selectedIndex = EditorGUILayout.IntPopup("Type spawned", m_selectedIndex, m_names, arr);
    if (m_hasAutoMove)
    {
      targ.Direction = EditorGUILayout.IntSlider("Direction", targ.Direction, 0, 5);
    }
    if (GUI.changed)
    {
      targ.prefab = AssetDatabase.LoadAssetAtPath("Assets/ObjectPrefabs/" + m_names[m_selectedIndex] + "Prefab.prefab", typeof(GameObject)) as GameObject;
      m_hasAutoMove = (targ.prefab.GetComponent<CustomObject>() as IAutoMove != null);
    }
  }
}
