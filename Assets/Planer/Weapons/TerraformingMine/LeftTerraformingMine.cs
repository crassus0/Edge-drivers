using UnityEngine;
using System.Collections;

public class LeftTerraformingMine : ButtonObject
{
  int cooldown = 0;
  public static GameObject minePrefab;
  public static Texture2D mineTexture;
  public bool IsActive { get; set; }
  public override string GetName()
  {
    return "LeftTerraformingMine";
  }
  public override Texture2D GetObjectTexture()
  {
    return mineTexture;
  }
  public override void Activate(bool isUp)
  {
    if (!isUp)
    {
      Armory.ShowUpgrades(Index);
      return;
    }
    Activated = true;
    if (cooldown > 0) return;
    TerraformingMine x = (Instantiate(minePrefab) as GameObject).GetComponent<TerraformingMine>();
    x.Node = ParentPlaner.Node;
    
    x.visible = true;
    x.states[0] = 6;
    x.states[1] = 3;
    x.states[2] = 6;
    x.steps = 5;
		x.ActivateOnStart=true;
    x.gameObject.SetActive(true);
    cooldown = 1;
  }
  protected override void OnLoop()
  {
    if (cooldown > 0) cooldown--;
    else Activated = false;
  }
}
