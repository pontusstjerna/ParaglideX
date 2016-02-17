using UnityEngine;
using System.Collections;

public class Glider : MonoBehaviour {

	private Player player;
	private MeshRenderer gliderRenderer;
	private Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		gliderRenderer = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Hide or show glider
		gliderRenderer.enabled = player.getDeployed ();

		if (player.getDeployed ()) {
			fly ();
		}
	}

	private void fly(){

		//This should be relative to glider, not world
		float yVel = transform.InverseTransformVector(body.velocity).y;
		//print (transform.InverseTransformVector(body.velocity));
		float zVel = transform.InverseTransformVector(body.velocity).z;

		Vector3 forward = new Vector3(0, 0, (((-yVel) * Reference.PLAYER_WEIGHT)*1)/1); //Fall makes speed

		Vector3 forwardDrag = Math.getDrag (transform.InverseTransformVector(body.velocity), Reference.DRAG_COEFFICIENT_FRONT, Reference.AIR_DENSITY_20,
		                                    Reference.AREA_FRONT);
		
		body.AddRelativeForce (forward + forwardDrag + getRelativeLift (yVel, Reference.PLAYER_WEIGHT));
	}

	private Vector3 getRelativeLift(float fallVelocity, float mass){
		return transform.InverseTransformVector(Vector3.up*((-fallVelocity*mass*35)/9)); //Speed makes lift
	}

	public bool flyAble(){ //If glider is above head
		return transform.rotation.eulerAngles.x < Reference.FLYABLE_ANGLE &&
			transform.rotation.eulerAngles.x > -Reference.FLYABLE_ANGLE;
	}
}
