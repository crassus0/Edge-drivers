using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class WhirlwindVisualizer : MonoBehaviour 
{
	public Whirlwind parent;
	void Update()
	{
		int rotationSpeed=parent.spin;
		ParticleSystem.Particle[] particles=new  ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
		GetComponent<ParticleSystem>().GetParticles(particles);
		for(int i=0; i<particles.Length; i++)
		{
			Vector3 position=particles[i].position;
			position.y=0;
			particles[i].position=position;
			Vector3 direction=new Vector3(0,0,0);
			direction.x=-position.z;
			direction.z=position.x;
			Vector3 dispertion=new Vector3(Random.Range(-1f, 1f),0,Random.Range(-1f, 1f))*direction.magnitude;
			direction=direction*Random.Range(-1,5)+dispertion;
			particles[i].velocity=direction*rotationSpeed;
			
			//particles[i].position
			//Debug.DrawRay(position+transform.position, direction, Color.red);
		}
		GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
	}
	
}
