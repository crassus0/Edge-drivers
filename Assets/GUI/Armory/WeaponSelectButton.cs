using UnityEngine;
using System.Collections;

public class WeaponSelectButton : MonoBehaviour, IButton
{
  public string weaponName;
  public void OnPressed(bool isUp)
  {

    if (isUp)
    {
      Armory.AddWeapon(weaponName);
    }
  }
}
