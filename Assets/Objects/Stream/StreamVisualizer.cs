using UnityEngine;
using System.Collections;
public class StreamVisualizer : MonoBehaviour 
{
	public Stream parent;
  public float MaxSpeed = 10E30f;
	void Update()
	{

		int rotationSpeed=parent.power;
		ParticleSystem.Particle [] particless=new  ParticleSystem.Particle[particleSystem.particleCount];
		particleSystem.GetParticles(particless);
		for(int i=0; i<particless.Length; i++)
		{
			Vector3 position=particless[i].position;

			position.y=0;
			if(position.x<0)
				position.x=-position.x;
			particless[i].position=position;
			Vector3 direction=new Vector3(0,0,0);
			int basicScale=32;
			float speedRate=-(basicScale-position.x)*(basicScale-position.x);
			direction.x=(speedRate/(basicScale*basicScale)+1.2f)*10*parent.power;
			
			
		  direction.z=-position.z*0.2f*parent.power;
      if (direction.magnitude > MaxSpeed)
        direction = direction.normalized * MaxSpeed;
			if(position.x>basicScale-1)
			{
				direction.z=-direction.z;
				//direction.x=-direction.x;
				particless[i].size=basicScale-position.x;
				if(particless[i].size<=0)
				{	
					particless[i].size=1;
					particless[i].position=new Vector3(0, 0, Random.Range(-0.75f,0.75f)*16);
				}
			}
			Vector3 dispertion=new Vector3(Random.Range(-1f, 1f),0,Random.Range(-1f, 1f))*direction.magnitude;
			direction=direction*Random.Range(-1,5)+dispertion;
			particless[i].velocity=direction*rotationSpeed;
			
			//particles[i].position
			//Debug.DrawRay(position+transform.position, direction, Color.red);
		}
		particleSystem.SetParticles(particless, particless.Length);
	}
	
}
