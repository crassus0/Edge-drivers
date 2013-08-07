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
    m_visualiser.renderer.material = new Material(m_visualiser.renderer.material);
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
      if (x != null&&x.State==0)
      {
          if (Status == 2)
            Status = 1;

          if (object.ReferenceEquals(x, Creator.Player))
          {
            //PlayerSaveData.SaveDiscoveredScene(m_targetScene, 2);
            x.AddUpdateFunc(OnPlanerEnter);
            x.HasTarget = true;
          }
          else
          {
            Creator.DestroyObject(x);
          }
      }
    }
  }
  void SetActiveVisualiser(int status)
  {
    if (m_visualiser != null)
    {
      m_visualiser.gameObject.SetActive(status > 0);
      if (status == 1)
      {
        m_visualiser.renderer.material.color = new Color(1, 1, 1, 0.5f);
      }
      if (status == 2)
      {
        m_visualiser.renderer.material.color = Color.red;
      }
      if (status == 3)
      {
        m_visualiser.renderer.material.color = Color.white;
      }
    }
  }
  void OnPlanerEnter(IPlanerLike planer)
  {
    //Debug.Log(PairPortal);
    PlanerCore x = planer as PlanerCore;
    x.RemoveUpdateFunc(OnPlanerEnter);
    x.EnteredPortal = true;
    PlayerSaveData.Save(x, m_targetNode, direction, true);
    Application.LoadLevel(m_targetScene);
    Destroy(Creator.creator);
  }
	public override CustomObjectInfo SerializeObject ()
	{
		DistantPortalEnterInfo x=new DistantPortalEnterInfo();
		x.m_targetNode=m_targetNode;
		x.m_targetPortalID=m_targetPortalID;
		x.m_targetScene=m_targetScene;
		x.direction=direction;
		x.node=Node;
		x.instanceID=ObjectID;
		return x;
	}
}
public class DistantPortalEnterInfo:CustomObjectInfo
{
	public string m_targetScene;
  public GraphNode m_targetNode;
  public int direction;
  public int m_targetPortalID;
	public override CustomObject Deserialize ()
	{
		DistantPortalEnter x = CreateInstance() as DistantPortalEnter;
		x.m_targetNode=m_targetNode;
		x.m_targetPortalID=m_targetPortalID;
		x.m_targetScene=m_targetScene;
		x.direction=direction;
		return x;
	}
	public override void EstablishConnections ()
	{
	}
	public override string GetName ()
	{
		return "DistantPortalEnter";
	}
}