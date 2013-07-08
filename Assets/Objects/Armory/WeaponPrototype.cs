using UnityEngine;
using System.Collections;

public class WeaponPrototype : CustomObject
{
  public string UpgradeName{get;set;}
  public int IsActive { get; set; }
  public ArmoryGenerator Armory { get; set; }
  public override void OnStart()
  {
    Interact = OnInteract;
    
  }
  public void OnInteract(CustomObject obj, InteractType type)
  {
    if(ReferenceEquals(obj, Creator.Player))
    {
      if (IsActive==0)
      {
        Activate();
        //TODO
      }
      else if(IsActive==1)
      {
        Deactivate();
        //TODO
      }
    }
  }
  public void OnActivate()
  {
    Armory.AddWeapon(UpgradeName);
    IsActive = 1;
    transform.GetChild(0).renderer.material.color = Color.green;
  }
  public void Deactivate()
  {
    Armory.RemoveWeapon(UpgradeName);
    IsActive = 0;
    transform.GetChild(0).renderer.material.color = Color.white;
  }
}
