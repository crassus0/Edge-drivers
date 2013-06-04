using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {
	public Creator creator;
	public GameObject buttonPrefab;
    public GameObject basicMinePrefab;
	public GameObject webMinePrefab;
	public Texture2D basicMineTexture;
	public Texture2D webMineTexture;
	
	void Awake () 
	{
	  BasicMineActivator.minePrefab=basicMinePrefab;
	  BasicMineActivator.mineTexture=basicMineTexture;
	  WebCatapultActivator.minePrefab=webMinePrefab;
	  WebCatapultActivator.mineTexture=webMineTexture;
	  ButtonObject.buttonPrefab=buttonPrefab.guiTexture;
	  creator.Init();
	}
	//TODO
}
