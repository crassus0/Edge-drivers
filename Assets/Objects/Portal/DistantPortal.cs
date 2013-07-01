using UnityEngine;
using System.Collections;



public class DistantPortal : CustomObject
{
  public string m_targetScene="";
  public GraphNode m_targetNode;
  public int m_targetPortalID;
  
  public override void OnUpdate()
  {

  }
  public override void Interact(CustomObject obj, InteractType type)
  {
    //Debug.Log("Interact");
    if (m_targetScene == null) return;
    if (type == InteractType.Stay)
    {

      PlanerCore x = obj as PlanerCore;
      if (x != null)
      {
        if (object.ReferenceEquals(x, Creator.Player))
        {
          
          PlayerSaveData.Save(x, m_targetNode, true);
          Application.LoadLevel(m_targetScene);
        }
        else
        {
          Destroy(x.gameObject);
        }
      }
    }
  }
  
  public DistantPortalSaveData GetData()
  {
    DistantPortalSaveData x;
    x.name = name;
    x.node = Node;
    x.portalID = this.GetInstanceID();
    return x;
  }
  [System.Serializable]
  public struct DistantPortalSaveData
  {
    public GraphNode node;
    public int portalID;
    public string name;
  }
}
