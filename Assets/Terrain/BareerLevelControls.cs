using UnityEngine;
using System.Collections;
using System.IO;
[ExecuteInEditMode()]
public class BareerLevelControls : MonoBehaviour
{

  public Mesh areaMesh;

	public static bool loadingLevel=false;
  public GameObject areaPrefab;



  bool m_init = false;
  public BareerAreaControls[] m_areas;
  public byte[] m_bareers;
  byte[] m_graph;
  public int Level { get; set; }
  public float SelectionPhaseDuration
  {
    get { return m_selectionPhase; }
    set { m_selectionPhase = value; }
  }
  [SerializeField]
  float m_selectionPhase = 1;
  public PhaseType SelectionPhaseType
  {
    get { return m_selectionPhaseType; }
    set { m_selectionPhaseType = value; }
  }
  [SerializeField]
  PhaseType m_selectionPhaseType;
  public float MovePhaseDuration
  {
    get { return m_movePhaseDuration; }
    set { m_movePhaseDuration = value; }
  }
  [SerializeField]
  float m_movePhaseDuration = 1;

  public int NumAreas = 1;

  float m_areaWidth;
  float m_areaHeight;
  float leftTime;

  public int triangleRow { get; set; }
  public byte[] Bareers { get { return m_bareers; } }
  public byte[] Graph { get { return m_graph; } }
  public void Awake()
  {
    Deactivate();
  }
  public void InitBareers()
  {
    int oldTriangleRow = (int)Mathf.Sqrt(m_bareers.Length);
    triangleRow = NumAreas * BareerAreaControls.areaSize;
    byte[] newbareers = new byte[(triangleRow) * (triangleRow)];
    int min = (triangleRow > oldTriangleRow) ? oldTriangleRow : triangleRow;
    int max = (triangleRow < oldTriangleRow) ? oldTriangleRow : triangleRow;
    for (int i = 0; i < min; i++)
    {
      for (int j = 0; j < min; j++)
      {
        newbareers[i + triangleRow * j] = m_bareers[i + oldTriangleRow * j];
      }
      if (oldTriangleRow < triangleRow)
        for (int j = min; j < max; j++)
        {
          newbareers[i + triangleRow * j] = 21;
        }
    }
    if (oldTriangleRow < triangleRow)
      for (int i = min; i < max; i++)
      {
        for (int j = 0; j < max; j++)
        {
          newbareers[i + triangleRow * j] = 21;
        }
      }
    m_bareers = newbareers;
  }
  public void Init()
  {
    //Debug.Log("init"+ Level);
    PreInit();
    GetComponent<Renderer>().sharedMaterial = new Material(GetComponent<Renderer>().sharedMaterial);
    m_areaWidth = BareerAreaControls.areaWidth;
    m_areaHeight = BareerAreaControls.areaHeight;
    triangleRow = NumAreas * BareerAreaControls.areaSize;
    hideFlags = 0;
    m_areas = new BareerAreaControls[NumAreas * NumAreas];
    for (int i = 0; i < NumAreas; i++)
      for (int j = 0; j < NumAreas; j++)
      {
        float x = (i) * m_areaWidth;
        float y = (j) * m_areaHeight;
        GameObject newArea = Instantiate(areaPrefab, new Vector3(x, 0, y), new Quaternion()) as GameObject;
        m_areas[i + NumAreas * j] = newArea.GetComponent<BareerAreaControls>();
        BareerAreaControls.BareerAreaParameters parameters;
        parameters.xCoord = i;
        parameters.yCoord = j;
        parameters.parent = this;
        //Debug.Log(this);
        parameters.basicMesh = areaMesh;
        newArea.GetComponent<Renderer>().sharedMaterial = GetComponent<Renderer>().sharedMaterial;
        newArea.transform.parent = transform;
        newArea.GetComponent<BareerAreaControls>().Init(parameters);
        newArea.hideFlags = 0;// HideFlags.HideInHierarchy | HideFlags.HideInInspector;
      }
    SetGraph();
    m_init = true;
  }
  void RedrawAreas()
  {
    for (int i = 0; i < NumAreas; i++)
      for (int j = 0; j < NumAreas; j++)
      {
        BareerAreaControls.BareerAreaParameters parameters;
        parameters.xCoord = i;
        parameters.yCoord = j;
        parameters.parent = this;
        parameters.basicMesh = areaMesh;
        m_areas[i + NumAreas * j].Init(parameters);
      }
  }
  public void OnChanged(int i, int j)
  {
    //Debug.Log("OnChanged");
    //Debug.Log(Application.loadedLevelName);
    int areaX = i / 8;
    int areaY = j / 8;
    i = i - areaX * 8;
    j = j - areaY * 8;
    if (m_init || !Application.isPlaying)
      m_areas[areaX + NumAreas * areaY].RedrawTriangle(i, j);
  }
  void SetGraph()
  {
    m_graph = new byte[triangleRow * triangleRow];
    for (int i = 0; i < triangleRow; i++)
      for (int j = 0; j < triangleRow; j++)
      {
        byte node = 0;
        byte triangle = m_bareers[i + j * triangleRow];
        for (int k = 0; k < 3; k++)
        {
          node += (byte)((1 - (triangle % 4) / 2) << k);
          triangle /= 4;
        }
        byte upperNode = (byte)(SetUpperNode(i, j) << 3);
        node += upperNode;
        Graph[i + j * triangleRow] = node;
      }
  }
  byte SetUpperNode(int i, int j)
  {
    if (j == triangleRow - 1) return 0;
    byte node = 0;
    byte triangle = (byte)(1 - (m_bareers[i + j * triangleRow] % 4) / 2);
    node += triangle;
    triangle = 0;
    i = i - j % 2;
    j++;
    if (i >= 0)
    {
      triangle = m_bareers[i + j * triangleRow];
      triangle /= 4;
      triangle /= 4;
      triangle %= 4;
      triangle = (byte)(1 - triangle / 2);
      triangle = (byte)(triangle << 1);
    }
    node += triangle;
    i++;
    if (i < triangleRow)
    {
      triangle = m_bareers[i + j * triangleRow];
      triangle /= 4;
      triangle %= 4;
      triangle = (byte)(1 - triangle / 2);
      triangle = (byte)(triangle << 2);
      node += triangle;
    }
    return node;
  }
  public void Deactivate()
  {
    gameObject.SetActive(false);
    if (Application.isPlaying)
      for (int i = 0; i < m_areas.Length; i++)
      {
        if (m_areas[i] != null)
          Destroy(m_areas[i].gameObject);
      }
    m_init = false;
  }
  public void Activate()
  {
    gameObject.SetActive(true);
    Init();
  }
  void OnDrawGizmosSelected()
  {
    DebugGraphWatch();
  }
  void DebugGraphWatch()
  {
    if (!m_init) return;
    if (Creator.Level != Level) return;
    //if (renderer.material.color.a < 1) return;
    float triangleWidth = BareerAreaControls.triangleWidth;
    float triangleHeight = BareerAreaControls.triangleHeight;
    for (int i = 0; i < triangleRow; i++)
      for (int j = 0; j < triangleRow; j++)
      {
        float xCoord = (i + 0.5f * ((j + 1) % 2 + 1)) * triangleWidth;
        float yCoord = triangleHeight * (j + 0.6666666f);
        Vector3 position = new Vector3(xCoord, 0, yCoord);
        GraphNode x = GraphNode.GetNodeByCoords(position, Level);
        WayStatus[] dirs = GraphTagMachine.GetDirections(x);
        for (int k = 0; k < 6; k++)
        {
          Vector3 direction = new Vector3(Mathf.Cos(Mathf.PI * ((1f / 3f) * k)), 0, Mathf.Sin(Mathf.PI * ((1f / 3f) * k)));
          Color lineColor = (dirs[k] != WayStatus.Free) ? Color.red : Color.green;
          Debug.DrawRay(position, direction * 10, lineColor);
        }
        position = new Vector3(xCoord, 0, triangleHeight * (j + 1.33333333f));
        x = GraphNode.GetNodeByCoords(position, Level);
        dirs = GraphTagMachine.GetDirections(x);
        for (int k = 0; k < 6; k++)
        {
          Vector3 direction = new Vector3(Mathf.Cos(Mathf.PI * ((1f / 3f) * k)), 0, Mathf.Sin(Mathf.PI * ((1f / 3f) * k)));
          Color lineColor = (dirs[k] != WayStatus.Free) ? Color.red : Color.green;
          Debug.DrawRay(position, direction * 10, lineColor);
        }
      }
  }
  public void OnDeactivate()
  {
    PreInit();
  }
  void PreInit()
  {

    int count = transform.childCount;
    for (int i = 0; i < count; i++)
    {
      DestroyImmediate(transform.GetChild(0).gameObject);
    }
  }
  void OnDestroy()
  {
    if (loadingLevel||Application.isPlaying) return;
    transform.parent.parent.GetComponent<Creator>().levels.Remove(this);
    if (EditorAdditionalGUI.EditorOptions != null)
      EditorAdditionalGUI.EditorOptions.ActiveLevel = 0;
  }
	public LevelInfo SerializeLevel()
	{
		LevelInfo x = new LevelInfo();
		x.name=name;
		x.m_bareers=m_bareers;
		x.SelectionPhaseDuration=SelectionPhaseDuration;
		x.SelectionPhaseType=SelectionPhaseType;
		x.MovePhaseDuration=MovePhaseDuration;
		x.NumAreas=NumAreas;
		return x;
	}

}
[System.Serializable]
public enum PhaseType
{
  OnAction,
  OnTime,
  Mixed
}
[System.Serializable]
public class LevelInfo
{
	public string name;
	public byte[] m_bareers;
	public float SelectionPhaseDuration;
	public PhaseType SelectionPhaseType;
	public float MovePhaseDuration;
	public int NumAreas;
	public BareerLevelControls Deserialize()
	{
		BareerLevelControls x = (Object.Instantiate(Creator.creator.levelPrefab) as GameObject).GetComponent<BareerLevelControls>();
		x.name=name;
		x.m_bareers=m_bareers;
		x.SelectionPhaseDuration=SelectionPhaseDuration;
		x.SelectionPhaseType=SelectionPhaseType;
		x.MovePhaseDuration=MovePhaseDuration;
		x.NumAreas=NumAreas;
		x.transform.parent=Creator.LeveltFolder.transform;
		return x;
	}
}