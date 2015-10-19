using UnityEngine;
using System.Collections;



public class DistantPortalEnter : CustomObject
{
  public string m_targetScene="";
  public GraphNode m_targetNode;
  public int direction;
  public int m_targetPortalID;
	public string m_targetPortalName;
  public int Status
  {
    get { return PlayerSaveData.GetSceneStatus(m_targetScene); }
    set
    {
      if (Status < value)
      {
        defaultStatus=value;
        SetActiveVisualiser(value);
      }
    }
  }
  public int defaultStatus = -1;
  public override void OnStart()
  {
    m_visualiser.GetComponent<Renderer>().material = new Material(m_visualiser.GetComponent<Renderer>().material);

    if(defaultStatus<Status)
      defaultStatus=Status;
    //Status = PlayerSaveData.GetSceneStatus(m_targetScene);
    Interact = OnInteract;
    SetActiveVisualiser(defaultStatus);
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    if (m_targetScene == null) return;
    if (defaultStatus < 2) return;
    if (type == InteractType.Stay)
    {

      PlanerCore x = obj as PlanerCore;
      if (x != null&&x.State==0)
      {
          if (defaultStatus == 2)
            defaultStatus = 1;

          if (object.ReferenceEquals(x, Creator.Player))
          {
            PlayerSaveData.SaveDiscoveredScene(m_targetScene, defaultStatus);
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
        m_visualiser.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
      }
      if (status == 2)
      {
        m_visualiser.GetComponent<Renderer>().material.color = Color.red;
      }
      if (status == 3)
      {
        m_visualiser.GetComponent<Renderer>().material.color = Color.white;
      }
    }
  }
  void OnPlanerEnter(IPlanerLike planer)
  {
    //Debug.Log(PairPortal);
    PlanerCore x = planer as PlanerCore;
    x.RemoveUpdateFunc(OnPlanerEnter);
    x.EnteredPortal = true;
		x.SetNewDirection(direction, true);
    Creator.creator.LoadLevel(m_targetScene, m_targetPortalName);
  }
	public override CustomObjectInfo SerializeObject ()
	{
		DistantPortalEnterInfo x=new DistantPortalEnterInfo();
		x.m_targetNode=new NodeInformation(m_targetNode);
		x.m_targetPortalID=m_targetPortalID;
		x.m_targetScene=m_targetScene;
		x.targetPortalName=m_targetPortalName;
		x.direction=direction;
		x.m_sceneStatus=defaultStatus;
		x.BasicSerialization(this);
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof(DistantPortalEnterInfo);
	}
}
[System.Serializable]
public class DistantPortalEnterInfo:CustomObjectInfo
{
	public string m_targetScene;
  public NodeInformation m_targetNode;
  public int direction;
  public int m_targetPortalID;
	[System.Runtime.Serialization.OptionalField]
	public int m_sceneStatus;
	[System.Runtime.Serialization.OptionalField]
	public string targetPortalName;
	public override CustomObject Deserialize ()
	{
		DistantPortalEnter x = CreateInstance() as DistantPortalEnter;
		x.m_targetNode = GraphNode.GetNodeByParameters(m_targetNode.i, m_targetNode.j, m_targetNode.index, m_targetNode.level);
		x.m_targetPortalID=m_targetPortalID;
		x.m_targetScene=m_targetScene;
		x.direction=direction;
		x.defaultStatus=m_sceneStatus;
		x.m_targetPortalName=targetPortalName;
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