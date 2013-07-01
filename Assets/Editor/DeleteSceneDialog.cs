using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class DeleteSceneDialog : ScriptableWizard
{
  string m_sceneName;
  void OnGUI()
  {
    m_sceneName = EditorApplication.currentScene;
    m_sceneName = m_sceneName.Remove(m_sceneName.Length - 6);
    int i = m_sceneName.LastIndexOf("/");
    m_sceneName = m_sceneName.Remove(0, i + 1);
    if (m_sceneName == "BasicScene"||EditorBuildSettings.scenes.Length<=1)
    {
      EditorGUILayout.LabelField("Unable delete scene");
      if(GUILayout.Button("OK"))
        Close();
    }
    EditorGUILayout.LabelField("Are you sure? This action is unrevertable.");
    EditorGUILayout.BeginHorizontal();
    {
      if (GUILayout.Button("Yes"))
        DeleteCurentScene();
      if (GUILayout.Button("Cancel"))
        Close();
    }
    EditorGUILayout.EndHorizontal();
  }
  void DeleteCurentScene()
  {
    foreach (string x in Directory.GetFiles("Assets/Levels/" + m_sceneName))
    {
      File.Delete(x);
    }
    Directory.Delete("Assets/Levels/" + m_sceneName);
    
    string path="Assets/Levels/" + m_sceneName + "/" + m_sceneName + ".unity";
    List<EditorBuildSettingsScene> scenes= new List<EditorBuildSettingsScene>();

    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++ )
    {
      if (!EditorBuildSettings.scenes[i].path.Equals(path))
      {
        scenes.Add(EditorBuildSettings.scenes[i]);
      }
    }
    EditorBuildSettings.scenes = scenes.ToArray();
    EditorApplication.OpenScene(EditorBuildSettings.scenes[0].path);
  }
}
