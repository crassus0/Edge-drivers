using UnityEngine;
using System.Collections;

public class BasicMineActivator : ButtonObject
{
  int cooldown = 0;
  int number = -1;

  public static GameObject minePrefab;
  public static Texture2D mineTexture;
  public bool IsActive { get; set; }
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
    GameObject mine = Instantiate(minePrefab) as GameObject;
    mine.transform.localScale = ParentPlaner.transform.localScale * 0.5f;
    mine.GetComponent<BasicMine>().Init(ParentPlaner);
    if (number > 0) number--;
    cooldown = 1;
    Activated = true;
  }

  public override Texture2D GetObjectTexture()
  {
    return mineTexture;
  }
  protected override void OnLoop()
  {
    //Debug.Log("Loop"+ cooldown);
    if (cooldown > 0) cooldown--;
    else Activated = false;
    
  }
  public override string GetName()
  {
    return "BasicMine";
  }
}
