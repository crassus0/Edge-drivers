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
		{
			int status=EditorUtility.DisplayDialogComplex("Save scene", "Do you wish to save current scene", "Yes", "No", "Cancel");
			if(status==0)
			  CreatorEditor.SaveLevel();
			if(status<2)
			  AddScene(); 
		}
  }
  void AddScene()
  {
		CreatorEditor.LoadLevel("SafeHouse");
		Creator.creator.SceneName=sceneName;
		CreatorEditor.SaveLevel();
		Close();
  }
}
