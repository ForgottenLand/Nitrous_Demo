using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public GameObject explosion;
	
	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        
		GameObject instantiatedExplosion = Instantiate(explosion, pos, rot) as GameObject;
        Destroy(gameObject);
    }
	
}
