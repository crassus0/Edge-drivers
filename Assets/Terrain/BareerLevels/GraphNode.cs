using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class GraphNode: System.IComparable<GraphNode>
{
  [SerializeField]
  int m_i;

  [SerializeField]
  int m_j;

  [SerializeField]
  int m_level;

  [SerializeField]
  int m_index;

  [SerializeField]
  List<CustomObject> m_objects;

  [SerializeField]
  List<CustomObject> m_toLeave;

  [SerializeField]
  List<CustomObject> m_toEnter;

  
  public int Level{get{return m_level;}}
  public int Index{get{return m_index;}}
  public int X{get{return m_i;}}
  public int Y{get{return m_j;}}
  static List<GraphNode> s_usedNodes;
  static HashSet<CustomObject> s_interact;
  public static readonly float graphDistance=32/Mathf.Sqrt(3);
  int nodeGraph;
  GraphNode[] adjacent=new GraphNode[3];
  int triangleRow;
  bool setGraph=false;
	
  public void ChangeState(byte[] states, List<BareerLevelControls> levels)
  {
    
    triangleRow = BareerAreaControls.areaSize * levels[m_level].NumAreas;
    if (m_index > 0) return;
    if (m_i < 0 || m_i >= triangleRow || m_j < 0 || m_j >= triangleRow) return;
    nodeGraph = levels[m_level].m_bareers[m_i + m_j * triangleRow];
    int oldNodeGraph = nodeGraph;
    int[] oldStates = new int[3];
    for (int i = 0; i < 3; i++)
    {
      oldStates[i] = oldNodeGraph % 4;
      oldNodeGraph /= 4;
    }
    bool[] changed = new bool[3];
    for (int i = 0; i < 3; i++)
      changed[i] = false;
    for (int i = 0; i < 3; i++)
    {
      byte curentState = (byte)((nodeGraph >> (i * 2)) % 4);
      if (curentState == states[i] || states[i] == 6) continue;
      byte zeroState = (byte)~(3 << (i * 2));
      byte changingState = states[i];
      if (changingState == 4)
        changingState = 3;
      nodeGraph = nodeGraph & zeroState;

      nodeGraph = nodeGraph | (changingState << (i * 2));
      changed[i] = true;
    }
    levels[m_level].m_bareers[m_i + m_j * triangleRow] = (byte)nodeGraph;
    levels[m_level].OnChanged(m_i, m_j);
  }
  public GraphNode()
  {
    m_i = 0;
    m_j = 0;
    m_index = 0;
    m_level = 0;
  }
  public int Distance(GraphNode x)
  {
    if (x.m_level != m_level) return int.MaxValue;
    return (int)(Vector3.Distance(NodeCoords(), x.NodeCoords()) / (graphDistance)) + 1;
  }

  public void Enter(CustomObject enteringObject)
  {
    int index = s_usedNodes.BinarySearch(this);
    GraphNode savedNode = this;
    if (index < 0)
    {
      s_usedNodes.Insert(~index, this);
    }
    else
    {
      savedNode = s_usedNodes[index];

    }
    savedNode.m_objects.Add(enteringObject);
    s_interact.Add(enteringObject);
    if (!setGraph)
      savedNode.SetNodeGraph();
  }
  public static  void InteractAll()
  {
    foreach (CustomObject enteringObject in s_interact)
    {
      GraphNode savedNode = enteringObject.GetNode();
      foreach (CustomObject x in savedNode.m_objects)
      {
        if (CustomObject.ReferenceEquals(x, enteringObject)) continue;
        enteringObject.Interact(x, InteractType.Enter);
        x.Interact(enteringObject, InteractType.Stay);
        if (enteringObject.GetNode() != savedNode)
          break;
      }
    }
    s_interact.Clear();	
  }
  public void Leave(CustomObject leavingObject)
  {
    m_objects.Remove(leavingObject);
  }
  public Vector3 NodeCoords()
  {
    float triangleHeight = BareerAreaControls.triangleHeight;
    float triangleWidth = BareerAreaControls.triangleWidth;
    Vector3 coords = new Vector3();
    coords.z = (m_j + 1 + 0.3333333f * (2 * m_index - 1)) * triangleHeight;
    coords.x = triangleWidth * (m_i + 0.5f * (Utility.Mod2(m_j + 1) + 1));
    return coords;
  }
  public Vector3[] BoundsCoords()
  {
    if (Index > 0) return null;
    float triangleHeight = BareerAreaControls.triangleHeight;
    float triangleWidth = BareerAreaControls.triangleWidth;
    Vector3[] bounds = new Vector3[6];
    Vector3 coords = Vector3.zero;
    coords.z = (m_j + 1) * triangleHeight;
    coords.x = triangleWidth * (m_i + 0.5f * (Utility.Mod2(m_j + 1) + 1));
    bounds[0] = coords;
    bounds[0].x += 0.5f * triangleWidth;
    bounds[1] = coords;
    bounds[1].x -= 0.5f * triangleWidth;
    coords.z = (m_j + 1) * triangleHeight;
    coords.x = triangleWidth * (m_i + 0.5f * (Utility.Mod2(m_j + 1) + 1));
    bounds[2] = coords;
    bounds[2].x -= 0.5f * triangleWidth;
    bounds[3] = coords;
    bounds[3].z -= triangleHeight;
    bounds[4] = coords;
    bounds[4].x += 0.5f * triangleWidth;
    bounds[5] = coords;
    bounds[5].z -= triangleHeight;
    return bounds;
  }
  int[] NodeDirections(GraphNode node)//directions which leads to node
  {
    GraphNode[] adj = GetAdjacent();
    int dir = -1;
    for (int i = 0; i < 3; i++)
    {
      if (adj[i].Equals(node))
        dir = i;
    }
    if (dir < 0)
      return null;
    int[] dirs = new int[2];
    if (m_index == 0)
    {
      dirs[0] = (2 * dir + 1) % 6;
      dirs[1] = (2 * dir + 2) % 6;
    }
    else
    {
      dirs[0] = (2 * (2 - dir)) % 6;
      dirs[1] = (2 * (2 - dir) + 1) % 6;
    }
    return dirs;
  }
  
  public static GraphNode GetNodeByCoords(Vector3 coords, int level)
  {
    float triangleHeight = BareerAreaControls.triangleHeight;
    float triangleWidth = BareerAreaControls.triangleWidth;
    int j = Utility.ToInt(coords.z / triangleHeight);
    int i = Utility.ToInt((coords.x - 0.5f * Utility.Mod2(j + 1) * triangleWidth) / triangleWidth);
    int index = 0;
    float tg = -Mathf.Sqrt(3);
    float inEq = coords.z - (j + 1) * triangleHeight - tg * (coords.x - (i + 0.5f * Utility.Mod2(j + 1)) * triangleWidth);

    if (inEq < 0)
    {

      i -= Utility.Mod2(j);
      j--;
      index = 1;
    }
    tg = Mathf.Sqrt(3);
    inEq = (coords.z - (j + 1) * triangleHeight) - tg * (coords.x - (i + 1 + 0.5f * Utility.Mod2(j + 1)) * triangleWidth);

    if (inEq < 0)
    {

      i -= Utility.Mod2(j);
      i++;
      j--;
      index = 1;
    }
    return GetNodeByParameters(i, j, index, level);
  }
  public bool[] GetDirections()
  {
    if (!setGraph)
      SetNodeGraph();
    bool[] directions = new bool[6];
    for (int i = 0; i < 6; i++)
      directions[i] = false;
    int bareers = nodeGraph;
    if (m_index == 0)
    {
      if ((bareers % 4) / 2 == 0)
      {
        directions[1] = true;
        directions[2] = true;
      }
      bareers /= 4;
      if ((bareers % 4) / 2 == 0)
      {
        directions[3] = true;
        directions[4] = true;
      }
      bareers /= 4;
      if ((bareers % 4) / 2 == 0)
      {
        directions[5] = true;
        directions[0] = true;
      }
    }
    else
    {
      if ((bareers % 4) / 2 == 0)
      {
        directions[4] = true;
        directions[5] = true;
      }
      bareers /= 4;
      if ((bareers % 4) / 2 == 0)
      {
        directions[2] = true;
        directions[3] = true;
      }
      bareers /= 4;
      if ((bareers % 4) / 2 == 0)
      {
        directions[1] = true;
        directions[0] = true;
      }
    }
    return directions;
  }
  public int GetHitDirection(int direction)
  {
    int newDir = 0;
    if (m_index == 0)
    {
      newDir = direction - 1 + 2 * (direction % 2);
      newDir += 3;
    }
    else
    {
      newDir = direction + 1 - 2 * (direction % 2);
      newDir += 3;
    }
    return newDir % 6;
  }
  public GraphNode GetUpperNode()
  {
    if (m_level == 3) return null;
    return GetNodeByCoords(NodeCoords(), m_level + 1);
  }
  public GraphNode GetLowerNode()
  {
    if (m_level == 1) return null;
    return GetNodeByCoords(NodeCoords(), m_level - 1);
  }
  public GraphNode GetNodeByDirection(int direction)
  {
    GraphNode[] adj = GetAdjacent();
    int newInd;
    if (m_index == 0)
    {
      newInd = ((direction + 5) % 6) / 2;
      return adj[newInd];
    }
    else
    {
      newInd = (5 - direction) / 2;
      return adj[newInd];
    }
  }

  public float NodeValue(evaluator ev)
  {
    float val = 0;
    foreach (CustomObject x in m_objects)
    {
      val += ev(x);
    }
    return val;
  }
  //technicals
  void SetNodeGraph()
  {
    if (m_index == 0)
    {
      if (m_i >= 0 && m_i < triangleRow && m_j >= 0 && m_j < triangleRow)
        nodeGraph = Creator.GetBareerMap(m_level)[m_i + m_j * triangleRow];
      else
        nodeGraph = (byte)Random.Range(0, 64);
    }
    else
    {
      int node = 0;
      GraphNode[] adj = GetAdjacent();
      adj[0].SetNodeGraph();
      node += (adj[0].nodeGraph) % 4;
      adj[1].SetNodeGraph();
      node += ((adj[1].nodeGraph >> 4) % 4) << 2;
      adj[2].SetNodeGraph();
      node += ((adj[2].nodeGraph >> 2) % 4) << 4;
      nodeGraph = node;
    }
    setGraph = true;
  }
  public static GraphNode GetNodeByParameters(int i, int j, int index, int level)
  {
    GraphNode node = new GraphNode();
    node.m_i = i;
    node.m_j = j;
    node.m_level = level;
    node.m_index = index;
    int findIndex = s_usedNodes.BinarySearch(node);
    GraphNode newNode;

    if (findIndex < 0)
    {
      newNode = node;
      newNode.m_objects = new List<CustomObject>();
    }
    else
    {
      newNode = s_usedNodes[findIndex];
    }
    newNode.triangleRow = BareerAreaControls.areaSize * Creator.creator.levels[level].NumAreas;
    return newNode;
  }

  GraphNode[] GetAdjacent()
  {
    if (adjacent[0] == null)
      adjacent[0] = GetNodeByParameters(m_i, m_j, 1 - m_index, m_level);
    int j = m_j;
    int i = m_i - Utility.Mod2(m_j);
    j += (2 * m_index - 1);
    adjacent[1] = GetNodeByParameters(i, j, 1 - m_index, m_level);
    i++;
    adjacent[2] = GetNodeByParameters(i, j, 1 - m_index, m_level);
    return adjacent;
  }
  public override bool Equals(object o)
  {
    if (o == null) return false;
    GraphNode x = o as GraphNode;
    if (x == null) return false;
    return (m_i == x.m_i && m_j == x.m_j && m_level == x.m_level && m_index == x.m_index);
  }
  public int CompareTo(GraphNode t)
  {
    int comparer = t.m_level - m_level;
    if (comparer != 0)
      return Utility.Sign(comparer);
    comparer = t.m_index - m_index;
    if (comparer != 0)
      return Utility.Sign(comparer);
    comparer = t.m_i - m_i;
    if (comparer != 0)
      return Utility.Sign(comparer);
    comparer = t.m_j - m_j;
    return Utility.Sign(comparer);
  }
  public override int GetHashCode()
  {
    return m_i ^ m_j;
  }
  public override string ToString()
  {
    return (m_i + "," + m_j + "," + m_index + "," + m_level);
  }
  static GraphNode()
  {
    s_usedNodes = new List<GraphNode>();
    s_interact = new HashSet<CustomObject>();

  }
  public delegate float evaluator(CustomObject x);
  public struct NodeInformation
  {
    public int i;
    public int j;
    public int index;
    public int level;
  }
}
