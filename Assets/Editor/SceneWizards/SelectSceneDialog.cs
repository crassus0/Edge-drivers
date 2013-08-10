using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class SelectSceneDialog : ScriptableWizard
{
  int m_index = -1;
	List<string> names;
	int[] indexes;
  void OnWizardUpdate()
	{
		names=new List<string>(SceneDataSaver.ReadSceneNames());
		indexes=new int[names.Count];
		for(int i=0; i<names.Count; i++)
		{
			indexes[i]=i;
		}
		m_index=names.IndexOf(Creator.creator.SceneName);
	}
	void OnGUI()
  {
		
		m_index=EditorGUILayout.IntPopup(m_index,names.ToArray(), indexes );
		if(GUILayout.Button("Load"))
		{
			int status=EditorUtility.DisplayDialogComplex("Save scene", "Do you wish to save current scene", "Yes", "No", "Cancel");
			if(status==0)
			  CreatorEditor.SaveLevel();
			if(status<2)
				CreatorEditor.LoadLevel(names[m_index]);
		}
  }

}
