using UnityEngine;
using System.Collections;

public class DebugGUIControls : MonoBehaviour {
	//PlanerCore player;
	// Use this for initialization
	double cooldown=5;
    void Start () {
	
	}
	double minFPS=90;
	// Update is called once per frame
	void Update () 
	{
	  string text=Creator.DebugMessage;
	  //Debug.Log(Creator.message);
	  if(cooldown>0){cooldown-=Time.deltaTime;}
	  else{
		double FPS=1/Time.deltaTime;
	  text= "Energy: ";
	  text=text+Creator.Energy.ToString();
	  text=text+"\n"+FPS+" FPS";
	  if(FPS<minFPS)
		minFPS=FPS;
	  text=text+"\n"+minFPS+" min FPS";
	  }
	  guiText.text=text;
	}
}
