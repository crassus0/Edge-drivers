using UnityEngine;
using System.Collections;
using System.IO;
[ExecuteInEditMode()]
public class BareerLevelControls : MonoBehaviour 
{

  public Mesh areaMesh;

  
  public GameObject areaPrefab;

  
  
  bool m_init=false;
  public BareerAreaControls[] m_areas;
  public byte[] m_bareers;
  byte[] m_graph;
  
  public int Level{get;set;}
  public int NumAreas=1;
  
  float m_areaWidth;
  float m_areaHeight;
//  static readonly float triangleHeight=2*Mathf.Sqrt(3);
//  static readonly float triangleWidth=4;

  float leftTime;
  
  public int triangleRow{get; set;}	
	
  
  public byte[] Bareers{get{return m_bareers;}}
  public byte[] Graph{get{return m_graph;}}
  public void InitBareers()
  {
//	Debug.Log(m_bareers[0]);
//	Debug.Log(m_bareers.Length);
	int oldTriangleRow=(int)Mathf.Sqrt(m_bareers.Length);
	triangleRow=NumAreas*BareerAreaControls.areaSize;
	byte [] newbareers=new byte[(triangleRow)*(triangleRow)];
	int min=(triangleRow>oldTriangleRow)?oldTriangleRow:triangleRow;
	for(int i=0; i<min; i++)
	{
	  for(int j=0; j<min; j++)
	  {
		newbareers[i+triangleRow*j]=m_bareers[i+oldTriangleRow*j];
	  }
	}
//	Debug.Log(triangleRow);
//    Debug.Log(oldTriangleRow);
	m_bareers=newbareers;
  }
  public void Init()
  {	

//	Debug.Log(Application.isPlaying);
//	if(m_init)return;
    PreInit();
	//Debug.Log(renderer.sharedMaterial);
    renderer.sharedMaterial=new Material(renderer.sharedMaterial);

	m_areaWidth=BareerAreaControls.areaWidth;
	m_areaHeight=BareerAreaControls.areaHeight;
	triangleRow=NumAreas*BareerAreaControls.areaSize;
	
	
	m_areas=new BareerAreaControls[NumAreas*NumAreas];
//	Debug.Log("Init");
	//if(Creator.creator.randomLevel||Application.platform!=RuntimePlatform.WindowsEditor)
      for(int i=0; i<NumAreas; i++)
	    for(int j=0; j<NumAreas; j++)
	    {
		  float x=(i)*m_areaWidth;
		  float y=(j)*m_areaHeight;
		  GameObject newArea=Instantiate(areaPrefab, new Vector3(x,0,y), new Quaternion()) as GameObject;
		  m_areas[i+NumAreas*j]=newArea.GetComponent<BareerAreaControls>();
		  BareerAreaControls.BareerAreaParameters parameters;
		  
		  parameters.xCoord=i;
		  parameters.yCoord=j;
		  parameters.parent=this;
		  parameters.basicMesh=areaMesh;

		  

			newArea.renderer.sharedMaterial=renderer.sharedMaterial;

		  newArea.transform.parent=transform;
		  newArea.GetComponent<BareerAreaControls>().Init(parameters);
		  newArea.hideFlags=HideFlags.HideInHierarchy|HideFlags.HideInInspector;
	    }
	SetGraph();
	m_init=true;
  }
  void RedrawAreas()
  {
	for(int i=0; i<NumAreas; i++)
	  for(int j=0; j<NumAreas; j++)
	  {
		BareerAreaControls.BareerAreaParameters parameters;
		parameters.xCoord=i;
		parameters.yCoord=j;
		parameters.parent=this;
		parameters.basicMesh=areaMesh;
		m_areas[i+NumAreas*j].Init(parameters);
	  }
  }
  public void OnChanged(int i, int j)
  {
	int areaX=i/8;
	int areaY=j/8;
	i=i-areaX*8;
	j=j-areaY*8;
//	Debug.Log(areaX+", "+areaY);
	m_areas[areaX+NumAreas*areaY].RedrawTriangle(i,j);
  }
  void SetGraph()
  {
	m_graph=new byte[triangleRow*triangleRow];
	for(int i=0; i<triangleRow; i++)
	  for(int j=0; j<triangleRow; j++)
	  {
		byte node=0;
		byte triangle=m_bareers[i+j*triangleRow];
		for(int k=0; k<3; k++)
		{
		  node+=(byte)((1-(triangle%4)/2)<<k);
		  triangle/=4;
		  
		}
		byte upperNode=(byte)(SetUpperNode(i,j)<<3);
		node+=upperNode;
		Graph[i+j*triangleRow]=node;
	  }
  }
  byte SetUpperNode(int i, int j)
  {
	if(j==triangleRow-1)return 0;
	byte node=0;
	byte triangle=(byte)(1-(m_bareers[i+j*triangleRow]%4)/2);
	node+=triangle;
	triangle=0;
	i=i-j%2;
	j++;
	if(i>=0)
	{
	  triangle=m_bareers[i+j*triangleRow];
	  triangle/=4;
	  triangle/=4;
	  triangle%=4;
	  triangle=(byte)(1-triangle/2);
	  triangle=(byte)(triangle<<1);
	}
	node+=triangle;
	i++;
	if(i<triangleRow)
	{
      triangle=m_bareers[i+j*triangleRow];
	  triangle/=4;
	  triangle%=4;
	  triangle=(byte)(1-triangle/2);
	  triangle=(byte)(triangle<<2);
	  node+=triangle;
	}	
	return node;
  }
  public void Deactivate()
  {
    animation.Play("HideLevel", PlayMode.StopAll);
  }
  public void Activate()
  {
    animation.Play("ShowLevel", PlayMode.StopAll);
  }
  void MUpdate()
  {
	DebugGraphWatch();
  }
  void DebugGraphWatch()
  {
	if(!m_init)return;
	if(renderer.material.color.a<1)return;
	float triangleWidth=BareerAreaControls.triangleWidth;
	float triangleHeight=BareerAreaControls.triangleHeight;
	for(int i=0; i<triangleRow; i++)
	  for(int j=0; j<triangleRow; j++)
	  {
		float xCoord=(i+0.5f*((j+1)%2+1))*triangleWidth;
		float yCoord=triangleHeight*(j+0.6666666f);
//		byte node=m_graph[i+triangleRow*j];
		Vector3 position=new Vector3(xCoord,0,yCoord);
		

        GraphNode x=GraphNode.GetNodeByCoords(position, Level);
		bool[]dirs=x.GetDirections();
		for(int k=0; k<6; k++)
		{
		  
		  Vector3 direction=new Vector3(Mathf.Cos(Mathf.PI*((1f/3f)*k)),0,Mathf.Sin(Mathf.PI*((1f/3f)*k)));
		  Color lineColor=(!dirs[k])?Color.red:Color.green;
		  Debug.DrawRay(position, direction*10, lineColor);

		}
		position=new Vector3(xCoord,0,triangleHeight*(j+1.33333333f));
//		direction=new Vector3(0,0,-triangleHeight/4);
        x=GraphNode.GetNodeByCoords(position, Level);
		dirs=x.GetDirections();
		for(int k=0; k<6; k++)
		{
		  
		  Vector3 direction=new Vector3(Mathf.Cos(Mathf.PI*((1f/3f)*k)),0,Mathf.Sin(Mathf.PI*((1f/3f)*k)));
		  Color lineColor=(!dirs[k])?Color.red:Color.green;
		  Debug.DrawRay(position, direction*10, lineColor);

		}
	 }
  }
  public void OnDeactivate()
	{
//	Debug.Log("Deactivated");
	  PreInit();
	}
  void PreInit()
  {
	
	int count=transform.GetChildCount();
	for(int i=0; i<count; i++)
	{
		DestroyImmediate(transform.GetChild(0).gameObject);
	}
  }
  void OnDestroy()
  {
	if(Application.isPlaying)return;
	transform.parent.GetComponent<Creator>().levels.Remove(this);
	if(EditorAdditionalGUI.EditorOptions!=null)
		EditorAdditionalGUI.EditorOptions.ActiveLevel=0;
  }
}
