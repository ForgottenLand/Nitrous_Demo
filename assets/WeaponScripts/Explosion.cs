using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	
	public float explosionTime = 1.0f;
	public float explosionRadius = 5.0f;
	public float explosionPower = 2000.0f;
	
	void Start ()
	{
		Destroy (gameObject, explosionTime);
	
		//Find all nearby colliders
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius);
		
		//Apply a force to all surrounding rigid bodies.
		foreach (Collider hit in colliders) {
			if (hit.rigidbody) {
				hit.rigidbody.AddExplosionForce (explosionPower, transform.position, explosionRadius);
			}
		}

		//If we have a particle emitter attached, emit particles for .5 seconds
		/*if( particleEmitter )
	{
		particleEmitter.emit = true;
		yield WaitForSeconds( 0.5 );
		particleEmitter.emit = false;
	}*/
	}
}

