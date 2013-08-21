using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(DistantPortalEnter))]
public class DistantPortalEditor : Editor
{
  List<string> m_portalNames;
  int m_portalIndex=-1;
  List<int> m_indexes;
  int m_index=-1;
  DistantPortalEnter targ;
  List<DistantPortalExitInfo> m_portalList;
  List<string> m_levelNames;
  int[] m_sceneIndexes;
  void OnEnable()
  {
    targ = target as DistantPortalEnter;
    UpdateSceneList();
    if (m_index >= 0)
    {
      UpdatePortalList();
    }
  }
  public override void OnInspectorGUI()
  {
    //base.OnInspectorGUI();
    UpdateSceneList();
    m_index = EditorGUILayout.IntPopup("Region", m_index, m_levelNames.ToArray(), m_sceneIndexes);
    if (m_index >= 0)
    {
      targ.m_targetScene = m_levelNames[m_index];
      if (GUI.changed)
        UpdatePortalList();
    }
    if (m_index > -1)
    {

      m_portalIndex = EditorGUILayout.IntPopup("Portal", m_portalIndex, m_portalNames.ToArray(), m_indexes.ToArray());
      if (m_portalIndex >= 0)
      {
        targ.m_targetPortalID = m_portalList[m_portalIndex].instanceID;
				targ.m_targetPortalName=m_portalList[m_portalIndex].instanceName;
				NodeInformation node=m_portalList[m_portalIndex].node;
        targ.m_targetNode = GraphNode.GetNodeByParameters(node.i,node.j,node.index, node.level);
      }
    }
    int[] status={-1,0,1,2,3};
    string[] names = { "Default", "Hidden","Shown", "Unstable", "Opened" };
    targ.defaultStatus = EditorGUILayout.IntPopup(targ.defaultStatus, names, status);
  }
  void UpdatePortalList()
  {
    LevelObjectsInfo info=LevelObjectsInfo.LoadLevelInfo(m_levelNames[m_index]);
		m_portalList=new List<DistantPortalExitInfo>();
		m_portalNames=new List<string>();
	  m_indexes=new List<int>();
		int i=0;
    foreach(CustomObjectInfo x in info.objectsInfo)
		{
			DistantPortalExitInfo portal=x as DistantPortalExitInfo;
			if(portal!=null)
			{
				m_portalList.Add(portal);
				m_portalNames.Add(portal.instanceName);
				if(targ.m_targetPortalName.Equals(portal.instanceName))
					m_portalIndex=i;
				m_indexes.Add(i++);
			}
		}
  }
  void UpdateSceneList()
  {
    m_levelNames = new List<string>(SceneDataSaver.ReadSceneNames());
    m_sceneIndexes = new int[m_levelNames.Count];

    for (int i = 0; i < m_levelNames.Count; i++)
    {
      m_sceneIndexes[i] = i;
    }
    m_index = m_levelNames.IndexOf(targ.m_targetScene);
    
  }
}
