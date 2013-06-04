using UnityEngine;
using System.Collections;

public static class Utility
{
	public static float GetAngle360(float x, float y)
    {
	  float ang;
	  //Debug.Log(x);
	  //Debug.Log (y);
//	  bool miny=false;
//	  bool minx=false;
	  
	  
	   
      if(Mathf.Abs(x)>=0.00001)
        ang=Mathf.Atan(Mathf.Abs(y/x))*180/Mathf.PI;
      else
        ang=90;
	  //Debug.Log(ang);
      if(x<0&&y<0)
	    ang-=180;
	  else if(x<0)
		ang=180-ang;
	  else if(y<0)
		ang=-ang;
	  //while(
	  //Debug.Log(ang);
	  return 90-ang;
    }
   public static int AllignToIntHorizontal(Allign allign)
   {
	 return allign==Allign.Right?1:0;
   }
   public static int ToInt(float x)
   {
	 return x>0?((int)x):((int)x-1);
   }
   public static byte Mod2(int x)
   {
	 return (byte)Mathf.Abs(x%2);
   }
   public static int Sign(float x)
   {
	 if(x<0)return -1;
	 if(x>0)return 1;
	  return 0;
   }
}
public enum Allign
{
  Right,
  Left,
  Top, 
  Bottom
}