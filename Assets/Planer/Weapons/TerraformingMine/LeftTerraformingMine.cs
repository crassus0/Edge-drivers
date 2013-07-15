using UnityEngine;
using System.Collections;

public class LeftTerraformingMine : ButtonObject
{
  public static GameObject minePrefab;
  public static Texture2D mineTexture;
  public override string GetName()
  {
    return "LeftTerraformingMine";
  }
  public override Texture2D GetObjectTexture()
  {
    return mineTexture;
  }
  public override void Activate()
  {
    TerraformingMine x = (Instantiate(minePrefab) as GameObject).GetComponent<TerraformingMine>();
    x.Node = ParentPlaner.Node;
    
    x.visible = true;
    x.states[0] = 6;
    x.states[1] = 3;
    x.states[2] = 6;
    x.steps = 5;
    x.gameObject.SetActive(true);
  }
  protected override void OnLoop()
  {
    
  }
}
