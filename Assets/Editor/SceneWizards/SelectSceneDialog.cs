using UnityEngine;
using System.Collections;
using UnityEditor;

public class SelectSceneDialog : ScriptableWizard
{
  int m_index = -1;
  void OnGUI()
  {
    maxSize = new Vector2(10, 5);
    string[] names = new string[EditorBuildSettings.scenes.Length + 1];
    int[] indexes = new int[names.Length];
    names[0] = " ";
    indexes[0] = -1;
    //Debug.Log(EditorBuildSettings.scenes.Length);
    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
    {
      string name = DeleteSceneDialog.GetSceneNameFromPath(EditorBuildSettings.scenes[i].path);
      //Debug.Log(name);
      names[i + 1] = name;
      indexes[i + 1] = i;
    }
    m_index = EditorGUILayout.IntPopup(m_index, names, indexes);
    if (m_index >= 0)
    {
      
      EditorGUILayout.BeginHorizontal();
      {
        if (GUILayout.Button("Load"))
        {
          if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
          {
            LoadSelectedScene();
            Close();
          }
        }
      }
      EditorGUILayout.EndHorizontal();
    }
  }
  void LoadSelectedScene()
  {
    EditorApplication.OpenScene(EditorBuildSettings.scenes[m_index].path);
  }
}
