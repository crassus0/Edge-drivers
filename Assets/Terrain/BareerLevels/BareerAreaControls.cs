using UnityEngine;
using System.Collections;

public class BareerAreaControls : MonoBehaviour 
{
	BareerLevelControls parent;
	int xCoord; 
    int yCoord;
	Mesh mesh;
	public static readonly int areaSize=8;
	public static readonly float triangleHeight=32*Mathf.Sqrt(3);
	public static readonly float triangleWidth=64;
	public static readonly float areaWidth=areaSize*triangleWidth;
	public static readonly float areaHeight=areaSize*triangleHeight;
    Vector2[] uvs;
	public void Init(BareerAreaParameters parameters)
	{
	  xCoord=parameters.xCoord;
	  yCoord=parameters.yCoord;
	  parent=parameters.parent;
//      Debug.Log(parent);
	  Mesh newMesh=new Mesh();
	  newMesh.vertices=parameters.basicMesh.vertices;
	  newMesh.triangles=parameters.basicMesh.triangles;
	  newMesh.uv=parameters.basicMesh.uv;
      newMesh.RecalculateNormals(); 
	  GetComponent<MeshFilter>().sharedMesh=newMesh;
	 
		mesh=GetComponent<MeshFilter>().sharedMesh;


//	  byte[] bareers = parent.Bareers;
	  uvs=mesh.uv;
	  for(int i=0; i<areaSize; i++)
		for(int j=0; j<areaSize; j++)
		{
		  InitTriangle(j, i);
		}
	  gameObject.hideFlags=HideFlags.HideInInspector|HideFlags.HideInHierarchy;
	}
	public void RedrawTriangle(int i, int j)
	{
//	  Debug.Log(xCoord+","+yCoord);
	  InitTriangle(i,j);
	}
	public void ChangeBareer(int x, int y, BareerIndex bareerIndex, bool newState)
	{
	  
	}
	void InitTriangle(int x, int y)
	{
	  int localCoord=x+areaSize*y;
//	  Debug.Log(parent);
	  int globalCoord=x+xCoord*areaSize+(y+yCoord*areaSize)*(areaSize*parent.NumAreas);
	  byte triangle=parent.Bareers[globalCoord];
	  for(int i=0; i<3; i++)
	  {
		int state=triangle%4;
		triangle/=4;
		for(int j=0; j<4; j++)
		{
		  uvs[12*localCoord+4*i+j].x=0.75f-0.25f*state+(int)(j/2)*0.25f;
		}		
	  }
	  mesh.uv=uvs;
	}
	public struct BareerAreaParameters
    {
      public Mesh basicMesh;
	  public BareerLevelControls parent;
	  public int xCoord;
	  public int yCoord;

    }
	public enum BareerIndex
    {
      top,
      left,
      right
    }
}
