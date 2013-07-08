using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(DistantPortal))]
public class DistantPortalEditor : Editor
{
  string[] m_portalNames;
  int m_portalIndex=-1;
  int[] m_indexes;
  int m_index=-1;
  DistantPortal targ;
  List<DistantPortal.DistantPortalSaveData> m_portalList;
  List<string> m_levelNames;
  int[] m_sceneIndexes;
  void OnEnable()
  {
    targ = target as DistantPortal;
    SceneDataSaver.SaveSceneData();
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

      m_portalIndex = EditorGUILayout.IntPopup("Portal", m_portalIndex, m_portalNames, m_indexes);
      if (m_portalIndex >= 0)
      {
        targ.m_targetPortalID = m_portalList[m_portalIndex].portalID;
        targ.m_targetNode = m_portalList[m_portalIndex].node;
      }
    }
    int[] status={-1,0,1,2,3};
    string[] names = { "Default", "Hidden","Shown", "Unstable", "Opened" };
    targ.defaultStatus = EditorGUILayout.IntPopup(targ.defaultStatus, names, status);
  }
  void UpdatePortalList()
  {
    
    m_portalList = SceneDataSaver.LoadSceneData(m_levelNames[m_index]);
    m_portalNames = new string[m_portalList.Count];
    m_indexes = new int[m_portalList.Count];
    for (int i = 0; i < m_portalList.Count; i++)
    {
      m_portalNames[i] = m_portalList[i].name;
      m_indexes[i] = i;
      if (targ.m_targetPortalID == m_portalList[i].portalID)
      {
        m_portalIndex = i;
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
