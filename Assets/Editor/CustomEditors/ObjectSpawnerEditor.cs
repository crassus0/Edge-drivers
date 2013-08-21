using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{

  ObjectSpawner targ;
  bool m_hasAutoMove=false;
  void OnEnable()
  {
    targ = target as ObjectSpawner;
    if (targ.prefab != null)
    {
      m_hasAutoMove = (targ.prefab.GetComponent<CustomObject>() as IAutoMove != null);
    }
  }
  public override void OnInspectorGUI()
  {
    
    targ.cooldown = EditorGUILayout.IntSlider("Cooldown", targ.cooldown, 0, 10);
    if (m_hasAutoMove)
    {
      targ.Direction = EditorGUILayout.IntSlider("Direction", targ.Direction, 0, 5);
    }
		targ.prefab=EditorGUILayout.ObjectField("Prefab",targ.prefab, typeof(GameObject), false) as GameObject;
		if(GUI.changed)
		{
			OnEnable();
		}
  }
}
