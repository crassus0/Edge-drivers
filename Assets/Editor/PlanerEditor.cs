using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(PlanerCore))]

public class PlanerEditor : Editor
{
  void OnEnable()
  {
    PlanerCore targetObject = (target as PlanerCore);
    int numChildren = targetObject.transform.childCount;
    for (int i = 0; i < numChildren; i++)
    {
      targetObject.transform.GetChild(i).hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
    }
  }
  public override void OnInspectorGUI()
  {

    PlanerCore targetObject = (target as PlanerCore);
    int numChildren = targetObject.transform.childCount;
    for (int i = 0; i < numChildren; i++)
    {
      //targetObject.transform.GetChild(i).hideFlags=HideFlags.HideInHierarchy|HideFlags.HideInInspector;
    }
    int direction = (EditorGUILayout.IntField("Direction", targetObject.Direction) + 6) % 6;
    int agility = EditorGUILayout.IntField("Agility", targetObject.Agility);
    if (GUI.changed)
    {
      targetObject.transform.rotation = Quaternion.identity;
      targetObject.transform.Rotate(new Vector3(0, -60 * direction, 0));
      targetObject.Direction = direction;
      targetObject.Agility = agility;

    }
    EditorUtility.SetDirty(target);
  }
  void OnSceneGUI()
  {
    PlanerCore targetObject = (target as PlanerCore);



    targetObject.transform.rotation = Quaternion.identity;
    targetObject.transform.Rotate(new Vector3(0, -60 * targetObject.Direction, 0));
  }

}
