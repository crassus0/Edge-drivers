using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(Trigger))]
public class TriggerEditor : Editor
{
	int index;
	string[] objectList=
	{
		"PlanerCore",
		"RedFirefly",
		"BasicMine",
		"TerraformingMine",
		"Warming",
		"GreenFirefly",
		"YellowFirefly",
		"WebMine"
		
	};
	int[] indexes;
	void OnEnable()
	{
		Trigger targ = target as Trigger;
		indexes=new int[objectList.Length];
		for(int i=0; i<indexes.Length; i++)
		{
			indexes[i]=i;
		}
		List<string> list=new List<string>(objectList);
		index=list.IndexOf(targ.TargetTriggerName);
	}
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		Trigger targ = target as Trigger;
    index=EditorGUILayout.IntPopup("Trigger activator type", index, objectList, indexes);
		targ.TargetTriggerName=objectList[index];
		
	}
}
