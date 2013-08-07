using UnityEngine;
using System.Collections;

public class WarmingHole : CustomObject
{
  public GameObject m_warmingPrefab;
  public WarmingHole PairHole;
  public int cooldown;
  int turn=0;
  int step = 0;

  public float[] intensity;
  public int direction;
  public override void OnStart()
  {
    OnUpdate = OnUpdated;
  }
  void OnUpdated()
  {
    if (turn++ == cooldown)
    {
      WarmingControls warm= (Instantiate(m_warmingPrefab) as GameObject).GetComponent<WarmingControls>();
      turn = 0;
      
      warm.m_warmingConcentration=intensity[(step++)%intensity.Length];
      warm.Direction = direction;
      warm.Node = Node.GetNodeByDirection(direction);
      warm.MaxRotateAngle = 3;
      warm.Init();
      warm.m_ai.SetTarget(PairHole.Node);
      warm.OnUpdate();
    }
  }
	public override CustomObjectInfo SerializeObject ()
	{
		WarmingHoleInfo x = new WarmingHoleInfo();
		x.PairHole=PairHole.ObjectID;
		x.cooldown=cooldown;
		x.node=Node;
		x.instanceID=ObjectID;
		return x;
	}
}
[System.Serializable]
public class WarmingHoleInfo:CustomObjectInfo
{
  public int PairHole;
  public int cooldown;
	WarmingHole hole;
	public override CustomObject Deserialize ()
	{
		hole=CreateInstance() as WarmingHole;
		hole.cooldown=cooldown;
		return hole;
	}
	public override void EstablishConnections ()
	{
		hole.PairHole=GetObjectByID(PairHole) as WarmingHole;
	}
	public override string GetName ()
	{
		return "WarmingHole";
	}
}