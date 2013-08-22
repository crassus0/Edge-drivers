using UnityEngine;
using System.Collections;

public class WebCatapultActivator : ButtonObject, IWeaponActivator
{
  int cooldown = 0;
  int number = -1;
  int range = 3;
  public static GameObject minePrefab;
  public static Texture2D mineTexture;
  public bool IsActive { get; set; }
	public int NumCharges {get;	set;}
  public override void Activate(bool isUp)
  {
    if (!isUp)
    {
      Armory.ShowUpgrades(Index);
      return;
    }
    if (cooldown > 0) return;
    if (number == 0) return;
    //	Debug.Log("hithit");
    minePrefab.GetComponent<CustomObject>().Level = ParentPlaner.Level;
    //	Debug.Log(ParentPlaner.transform.position);
    //	Debug.Log(ParentPlaner.prevNode.NodeCoords());
    GameObject mine = Instantiate(minePrefab) as GameObject;
    mine.transform.localScale = ParentPlaner.transform.localScale * 0.5f;
    mine.GetComponent<WebMine>().Init(ParentPlaner, range);
    if (number > 0) number--;
    cooldown =1;
    Activated = true;
  }

  public override Texture2D GetObjectTexture()
  {
    return mineTexture;
  }
  protected override void OnLoop()
  {
    //Debug.Log("Loop"+ cooldown);
    if (cooldown > 0)
      cooldown--;
    else
      Activated = false;
   
  }
  public override string GetName()
  {
    return "Web Catapult";
  }
	static WebCatapultActivator()
	{
		PlayerSaveData.SaveCharges("WebCatapultActivator",  MineController.MaxNumCharges);
	}
}
