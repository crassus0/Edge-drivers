using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {
	public Creator creator;
	public GameObject buttonPrefab;
    public GameObject basicMinePrefab;
	public GameObject webMinePrefab;
  public GameObject terraformingMinePrefab;
  public GameObject weaponIconPrefab;
	public Texture2D basicMineTexture;
	public Texture2D webMineTexture;
  public Texture2D cancelActionTexture;
  public Texture2D homePortalTexture;
  public Texture2D[] arcadeRotateButtons;
  public Texture2D leftTerraformingMineIcon;
	public Texture2D stayButtonTexture;
	void Awake () 
	{
    Armory.weaponPrototypePrefab = weaponIconPrefab;
	  BasicMineActivator.minePrefab=basicMinePrefab;
	  BasicMineActivator.mineTexture=basicMineTexture;
	  WebCatapultActivator.minePrefab=webMinePrefab;
	  WebCatapultActivator.mineTexture=webMineTexture;
	  ButtonObject.buttonPrefab=buttonPrefab.guiTexture;
    CancelAction.objectTexture = cancelActionTexture;
    HomePortal.homeTexture=homePortalTexture;
    ArcadeControlButton.arcadeControlsTexture = arcadeRotateButtons;
    LeftTerraformingMine.minePrefab = terraformingMinePrefab;
    LeftTerraformingMine.mineTexture = leftTerraformingMineIcon;
		StayButton.objectTexture=stayButtonTexture;
	}

	//TODO
}
