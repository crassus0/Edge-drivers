using UnityEngine;
using System.Collections;

public static class GraphTagMachine
{

  public static WayStatus[] GetDirections(IAutoMove planer)
  {
    return GetDirections(planer.GetNode());
  }
  public static WayStatus[] GetDirections(GraphNode node)
  {
    byte[] bareers = node.GetNodeGraph(true);
    //Debug.Log(nodeGraph);

    WayStatus[] directions = new WayStatus[6];
    for (int i = 0; i < 6; i++)
      directions[i] = WayStatus.Blocked;
    if (node.Index == 0)
    {

      if ((bareers[0] % 4) / 2 == 0)
      {
        directions[1] = WayStatus.Free;
        directions[2] = WayStatus.Free;
      }

      if ((bareers[1] % 4) / 2 == 0)
      {
        directions[3] = WayStatus.Free;
        directions[4] = WayStatus.Free;
      }

      if ((bareers[2] % 4) / 2 == 0)
      {
        directions[5] = WayStatus.Free;
        directions[0] = WayStatus.Free;
      }
    }
    else
    {
      if ((bareers[0] % 4) / 2 == 0)
      {
        directions[4] = WayStatus.Free;
        directions[5] = WayStatus.Free;
      }
      if ((bareers[1] % 4) / 2 == 0)
      {
        directions[2] = WayStatus.Free;
        directions[3] = WayStatus.Free;
      }
      if ((bareers[2] % 4) / 2 == 0)
      {
        directions[1] = WayStatus.Free;
        directions[0] = WayStatus.Free;
      }
    }
    return directions;
  }
	public static GraphNode GetNodeByDirection(GraphNode node, int direction)
	{
		//direction=GetDirection(node, direction);
		if(GetDirections(node)[direction]==WayStatus.Free)
			node=node.GetNodeByDirection(direction);
		return node;
	}
	public static int GetDirection(GraphNode node, int direction)
	{
		if(node.Tag==NodeTag.Whirlwind)
		{
			direction=(direction+node.TagModifier+6)%6;
		}
		return direction;
	}
	public static bool GetTagStatus(NodeTag tag)
  {
    string colorString;
    switch (tag)
    {
      case NodeTag.BlueColoured:
      {
        colorString="BlueColor";
        break;
      }
      case NodeTag.RedColoured:
      {
        colorString="RedColor";
        break;
      }
      case NodeTag.GreenColoured:
      {
        colorString="GreenColor";
        break;
      }
		  case NodeTag.Whirlwind:
			{
				return false;
			}
      default:
      {
        return true;
      }
    }
    
    return PlayerSaveData.GetColorStatus(colorString);
   
  }
}
public enum NodeTag
{
   None,
   Whirlwind,
   RedColoured,
   BlueColoured,
   GreenColoured
}
public enum WayStatus
{
  Free, Blocked, Unavailable
}