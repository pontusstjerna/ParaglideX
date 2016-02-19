using UnityEngine;
using System.Collections;

public class Glider : MonoBehaviour {

	private Player player;
	private Rigidbody body;
	public float angleOfAttack = 8;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (player.getDeployed ()) {
			fly ();
		}

		pushBack ();

		//print (transform.InverseTransformDirection(body.velocity));
	}

	private void fly(){
		//The local velocity of the glider
		float xVel = transform.InverseTransformDirection(body.velocity).x;
		float yVel = transform.InverseTransformDirection(body.velocity).y;
		float zVel = transform.InverseTransformDirection(body.velocity).z;

		Vector3 forwardDrag = Math.getDrag (zVel*Vector3.forward, Reference.DRAG_COEFFICIENT_FRONT, Reference.AIR_DENSITY_20,
		                                    Reference.AREA_FRONT);
		Vector3 fallDrag = Math.getDrag (yVel * Vector3.up, Reference.DRAG_COEFFICIENT_UNDER, Reference.AIR_DENSITY_20,
			                   Reference.AREA_UNDER);

		//Drag from side-drifting. Speed is a stabilizer! THIS MAGICALLY CREATED CENTRIFUGAL-FORCES!!
		Vector3 sideDrag = Math.getDrag (xVel * Vector3.right * Mathf.Abs(zVel), Reference.DRAG_COEFFICIENT_SIDE, Reference.AIR_DENSITY_20,
			                   Reference.AREA_SIDE);

		//The fall drag is ALWAYS the opposite of gravity, depending on how much of the
		//glider is exposed to the air
		body.AddForce (fallDrag);

		//Add the forces
		body.AddRelativeForce (getGlide(yVel, Reference.PLAYER_WEIGHT) + forwardDrag + getLift (zVel) + sideDrag);

		//print("Added force: " + (getGlide(yVel, Reference.PLAYER_WEIGHT) + 	forwardDrag + getLift (zVel)));

		brake();
	}

	private Vector3 getLift(float relativeSpeed){

		//Lift ~ relativeVel^2 * angleOfAttack

		if (transform.InverseTransformDirection (body.velocity).z > Reference.STALL_LIMIT) { //Cheap stall check
			return (Vector3.up * Mathf.Pow(relativeSpeed,2)*angleOfAttack); //Speed makes lift
		} else {
			return Vector3.zero;
		}
	}

	private Vector3 getGlide(float fallVelocity, float mass){
		return Vector3.forward*-fallVelocity*mass; //Fall makes speed
	}

	public bool flyAble(){ //If glider is above head
		return transform.rotation.eulerAngles.x < Reference.FLYABLE_ANGLE &&
			transform.rotation.eulerAngles.x > -Reference.FLYABLE_ANGLE;
	}

	private void brake(){
		Vector3 brakeDrag = Math.getDrag (body.velocity, Reference.DRAG_COEFFICIENT_UNDER, Reference.AIR_DENSITY_20, 
			               Reference.AREA_BRAKE);

		Vector3 brakeRightPos = transform.TransformPoint (Vector3.right * 4 + Vector3.back);
		Vector3 brakeLeftPos = transform.TransformPoint (Vector3.left * 4 + Vector3.back);

		body.AddForceAtPosition (brakeDrag*Input.GetAxis("brakeR")*0.5f, brakeRightPos);

		body.AddForceAtPosition (brakeDrag*Input.GetAxis("brakeL")*0.5f, brakeLeftPos);
	}

	private void pushBack(){
		if (Input.GetKeyDown (KeyCode.Space) && !player.getDeployed()) {
			body.useGravity = false;
			body.AddRelativeForce(Vector3.back*10000);
			body.useGravity = true;
		}
	}
}
