using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
public class AddSceneDialog : ScriptableWizard
{
  string sceneName = "NewScene";
  void OnGUI()
  {
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("SceneName");
    sceneName=EditorGUILayout.TextArea(sceneName);
    EditorGUILayout.EndHorizontal();
    if (GUILayout.Button("Create"))
      AddScene(); 
  }
  void AddScene()
  {
    EditorApplication.OpenScene("Assets/Levels/BasicScene.unity");
    Directory.CreateDirectory("Assets/Levels/" + sceneName);
    EditorApplication.SaveScene("Assets/Levels/" + sceneName + "/" + sceneName + ".unity");
    var original = EditorBuildSettings.scenes;

    var newSettings = new EditorBuildSettingsScene[original.Length + 1];

    System.Array.Copy(original, newSettings, original.Length);

    var sceneToAdd = new EditorBuildSettingsScene("Assets/Levels/" + sceneName + "/" + sceneName + ".unity", true);

    newSettings[newSettings.Length - 1] = sceneToAdd;

    EditorBuildSettings.scenes = newSettings;
    SceneDataSaver.SaveSceneData();
    this.Close();
  }
}
