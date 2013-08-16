using UnityEngine;
using System.Collections;

public class Anchor : CustomObject
{

	bool m_activated;
	public Texture2D showPicture;
  public override void OnStart ()
	{
		Interact=OnInteract;
	}
	void OnInteract(CustomObject obj, InteractType type)
	{
	  if(obj as PlanerCore==null)return;
	  gameObject.GetComponent<Anchor>().m_activated=true;
		Creator.creator.Pause();
  }
	bool CheckActive()
	{
		
		return m_activated;
	}
	void OnGUI()
	{
		if(!CheckActive())return;
		
		Vector2 leftTop=Camera.main.ViewportToScreenPoint(new Vector3(0.2f, 0.8f,0));
		Vector2 rightBottom=Camera.main.ViewportToScreenPoint(new Vector3(0.8f, 0.2f,0));
		Vector2 buttonLeft=Camera.main.ViewportToScreenPoint(new Vector3(0.4f, 0.8f));
		Vector2 buttonRight=Camera.main.ViewportToScreenPoint(new Vector3(0.6f, 0.9f));
		Rect position=Rect.MinMaxRect(leftTop.x, leftTop.y, rightBottom.x, rightBottom.y);
		Rect button=Rect.MinMaxRect(buttonLeft.x, buttonLeft.y, buttonRight.x, buttonRight.y);
		GUI.DrawTexture(position, showPicture);
		if(GUI.Button(button, "OK"))
		{
			m_activated=false;
			Creator.creator.Pause();
		}
	}
	public override System.Type SerializedType ()
	{
		return typeof(AnchorInfo);
	}
	public override CustomObjectInfo SerializeObject ()
	{
		AnchorInfo info=new AnchorInfo();
		info.BasicSerialization(this);
		info.showPicture=showPicture.name;
		return info;
	}
}
[System.Serializable]
public class AnchorInfo: CustomObjectInfo
{
	public string showPicture;
	public override CustomObject Deserialize ()
	{
		Anchor info=CreateInstance() as Anchor;
		info.showPicture=Resources.Load("InfoPictures/"+showPicture, typeof(Texture2D)) as Texture2D;
		return info;
	}
	public override void EstablishConnections ()
	{
	}
	public override string GetName ()
	{
		return "Anchor";
	}
	
}