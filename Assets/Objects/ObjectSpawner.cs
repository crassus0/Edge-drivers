using UnityEngine;
using System.Collections;
[ExecuteInEditMode()]
public class ObjectSpawner : CustomObject
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
  [SerializeField]
  int m_direction;
  public override void OnStart()
  {
    OnUpdate = OnUpdated;
    //throw new System.NotImplementedException();
  }
  protected virtual void OnUpdated()
  {
    if (!Node.HasObjectOfType(prefab.GetComponent<CustomObject>().GetType() )&&--step<=0)
    {
      
      if (prefab != null)
      {
        GameObject x = Instantiate(prefab) as GameObject;
        CustomObject y = x.GetComponent<CustomObject>();
        y.Node = Node;
        x.SetActive(true);
        if (y as IAutoMove != null)
          (y as IAutoMove).Direction = Direction;
      }
      step = cooldown;
    }
  }
  protected new void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    Gizmos.DrawIcon(transform.position, "Spawner.png");
    if (prefab != null&& (prefab.GetComponent<CustomObject>() as IAutoMove)!=null)
    {
      
      Gizmos.color=Color.yellow;
      float ang = (1f/3f)*Mathf.PI* m_direction;  
      Vector3 dest=Vector3.right*Mathf.Cos (ang)+Vector3.forward*Mathf.Sin(ang);
      //Debug.Log(dest);
      Gizmos.DrawRay(transform.position, dest*20);
    }
  }
}
