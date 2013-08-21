using UnityEngine;
using System.Collections;
[ExecuteInEditMode()]
public class ObjectSpawner : CustomObject, IActivatable
{
  public GameObject prefab;
  protected int step=0;
  public int cooldown=1;
  public int Direction 
  {
    get { return m_direction;}
    set 
    {
      m_direction=value%6;
      if (m_direction < 0)
        m_direction += 6;
    }
  }
  public bool ActivateOnStart { get { return false; } }
  [SerializeField]
  int m_direction=0;
  public override void OnStart()
  {
    if(cooldown>0)
      OnUpdate = OnUpdated;
    //throw new System.NotImplementedException();
  }
  protected virtual void OnUpdated()
  {
    if (prefab == null) return;
    if (!Node.HasObjectOfType(prefab.GetComponent<CustomObject>().GetType() )&&--step<=0)
    {

      Spawn();
      step = cooldown;
    }
  }
  public void Activate()
  {
    Spawn();
  }
  void Spawn()
  {
    if (prefab != null)
    {
      GameObject x = Instantiate(prefab) as GameObject;
      x.name = x.name.Replace("Prefab(Clone)", "");
      CustomObject y = x.GetComponent<CustomObject>();
      y.Node = Node;
      x.SetActive(true);
      if (y as IAutoMove != null)
        (y as IAutoMove).Direction = Direction;
    }
  }
  protected void OnDrawGizmos()
  {
    Gizmos.DrawIcon(transform.position, "ObjectSpawner.png");
    if (prefab != null&& (prefab.GetComponent<CustomObject>() as IAutoMove)!=null)
    {
      
      Gizmos.color=Color.yellow;
      float ang = (1f/3f)*Mathf.PI* m_direction;  
      Vector3 dest=Vector3.right*Mathf.Cos (ang)+Vector3.forward*Mathf.Sin(ang);
      //Debug.Log(dest);
      Gizmos.DrawRay(transform.position, dest*20);
    }
  }
	public override CustomObjectInfo SerializeObject ()
	{
		ObjectSpawnerInfo x = new ObjectSpawnerInfo();
		if(Creator.prefabs.Find(z=>z.name.Equals(prefab.name))==null)
		{
			Creator.prefabs.Add(prefab);
		}
		
		x.BasicSerialization(this);
		x.prefabName=prefab.name.Remove(prefab.name.Length-6);
	  x.cooldown=cooldown;	
		x.direction=m_direction;
		return x;
	}
	public override System.Type SerializedType ()
	{
		return typeof(ObjectSpawnerInfo);
	}
}
public class ObjectSpawnerInfo:CustomObjectInfo
{
	public string prefabName;
	public int cooldown;
	public int direction;
	public override CustomObject Deserialize ()
	{
		ObjectSpawner x = CreateInstance() as ObjectSpawner;
		x.cooldown=cooldown;
		x.Direction=direction;
		prefabName=prefabName+"Prefab";
		x.prefab=Creator.prefabs.Find(z=>z.name.Equals(prefabName));
		if(x.prefab==null)
		{
			x.prefab=Resources.Load("Prefabs/"+prefabName) as GameObject;
		}
		return x;
	}
	public override void EstablishConnections ()
	{
	}
	public override string GetName ()
	{
		return "ObjectSpawner";
	}
}