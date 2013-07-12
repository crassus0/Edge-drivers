using UnityEngine;
using System.Collections;



public class DistantPortalEnter : CustomObject
{
  public string m_targetScene="";
  public GraphNode m_targetNode;
  public int direction;
  public int m_targetPortalID;
  public int Status
  {
    get { return PlayerSaveData.GetSceneStatus(m_targetScene); }
    set
    {
      if (Status < value)
      {
        PlayerSaveData.SaveDiscoveredScene(m_targetScene, value);
        SetActiveVisualiser(Status);
      }
    }
  }
  public int defaultStatus = -1;
  public override void OnStart()
  {
    if (Status < defaultStatus)
      Status = defaultStatus;
    //Status = PlayerSaveData.GetSceneStatus(m_targetScene);
    Interact = OnInteract;
    SetActiveVisualiser(Status);
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    //Debug.Log("Interact");
    if (m_targetScene == null) return;
    if (Status < 2) return;
    if (type == InteractType.Stay)
    {

      PlanerCore x = obj as PlanerCore;
      if (x != null)
      {
        if (Status == 2)
          Status = 1;
        
        if (object.ReferenceEquals(x, Creator.Player))
        {
          //PlayerSaveData.SaveDiscoveredScene(m_targetScene, 2);
          PlayerSaveData.Save(x, m_targetNode, direction, true);
          Application.LoadLevel(m_targetScene);
          Destroy(Creator.creator);
        }
        else
        {
          Destroy(x.gameObject);
        }
      }
    }
  }
  void SetActiveVisualiser(int status)
  {
    if(m_visualiser!=null)
      m_visualiser.gameObject.SetActive(status>0);

  }

}
