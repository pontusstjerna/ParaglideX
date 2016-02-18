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

		if (Input.GetKey (KeyCode.Space) && !player.getDeployed()) {
			body.AddRelativeForce(Vector3.back*1000);
		}

		if (player.getDeployed ()) {
			fly ();
		}

		print (transform.InverseTransformVector (body.velocity));
	}

	private void fly(){

		//The local velocity of the glider
		float yVel = transform.InverseTransformVector(body.velocity).y;
		//float zVel = transform.InverseTransformVector(body.velocity).z;

		Vector3 forwardDrag = Math.getDrag (transform.InverseTransformVector(body.velocity), Reference.DRAG_COEFFICIENT_FRONT, Reference.AIR_DENSITY_20,
		                                    Reference.AREA_FRONT);

		//Add the forces
		body.AddRelativeForce (getRelativeGlide(yVel, Reference.PLAYER_WEIGHT) + 
		                       forwardDrag + getRelativeLift (yVel, Reference.PLAYER_WEIGHT));
	}

	private Vector3 getRelativeLift(float fallVelocity, float mass){
		return transform.InverseTransformVector(Vector3.up*((-fallVelocity*mass*35)/9)); //Speed makes lift
	}

	private Vector3 getRelativeGlide(float fallVelocity, float mass){
		return new Vector3(0, 0, (((-fallVelocity) * mass)*1)/1); //Fall makes speed
	}

	public bool flyAble(){ //If glider is above head
		return transform.rotation.eulerAngles.x < Reference.FLYABLE_ANGLE &&
			transform.rotation.eulerAngles.x > -Reference.FLYABLE_ANGLE;
	}
}
