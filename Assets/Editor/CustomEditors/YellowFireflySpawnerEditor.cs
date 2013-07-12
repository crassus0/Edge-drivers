using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(YellowFireflySpawner))]
public class YellowFireflySpawnerEditor : Editor
{
  YellowFireflySpawner targ;
  public override void OnInspectorGUI()
  {
    targ=target as YellowFireflySpawner;
    targ.cooldown = EditorGUILayout.IntField("Cooldown", targ.cooldown);
    targ.Direction = EditorGUILayout.IntSlider("Direction", targ.Direction, 0, 5);
    int[] x={-1,1};
    string[] y = {"-1","1"};
    targ.spin=EditorGUILayout.IntPopup("Spin", targ.spin, y,x);  
    if(GUI.changed)
    {
      EditorUtility.SetDirty(targ);
      targ.transform.rotation=Quaternion.identity;
      targ.transform.Rotate(new Vector3(0,-60*targ.Direction,0));
    }
  }
}
