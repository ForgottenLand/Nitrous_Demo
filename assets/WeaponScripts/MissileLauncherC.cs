using UnityEngine;
using System.Collections;

public class MissileLauncherC : MonoBehaviour {
	
	public Rigidbody projectile;
	public int speed = 10;
	
	void Update ()
	{
		if(Input.GetButtonDown ("Fire1"))
		{
			Rigidbody instantiatedProjectile = Instantiate( projectile, transform.position, transform.rotation) as Rigidbody;
			instantiatedProjectile.velocity = transform.TransformDirection( new Vector3(0,0,speed));
			Physics.IgnoreCollision(instantiatedProjectile.collider, transform.root.collider);
		}
	}
}
