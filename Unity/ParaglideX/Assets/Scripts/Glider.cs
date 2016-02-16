using UnityEngine;
using System.Collections;

public class Glider : MonoBehaviour {

	private Player player;
	private MeshRenderer gliderRenderer;
	private Rigidbody body;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").GetComponent<Player> ();
		gliderRenderer = GetComponent<MeshRenderer> ();
		body = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Hide or show glider
		gliderRenderer.enabled = player.getDeployed ();

		if(!flyAble() && player.onGround()){
			startGlider();
		}else{
			fly ();
		}
	}

	private void fly(){
		float yVel = -body.velocity.y * Mathf.Cos (Mathf.Deg2Rad * transform.rotation.eulerAngles.x);
		float zVel = body.velocity.z;

		Vector3 forward = new Vector3(0, 0, ((yVel) * Reference.PLAYER_MASS) / 17); //Fall makes speed
		
		Vector3 forwardDrag = Math.getDrag (body.velocity, Reference.DRAG_COEFFICIENT_FRONT, Reference.AIR_DENSITY_20,
		                                    Reference.AREA_FRONT);

		body.AddForce (getLift (yVel, Reference.PLAYER_MASS));
		body.AddRelativeForce (forward + forwardDrag);
	}

	private void startGlider(){

	}

	private Vector3 getLift(float fallVelocity, float mass){
		return Vector3.up*((fallVelocity*mass*10)/17); //Speed makes lift
	}

	public bool flyAble(){ //If glider is above head
		return transform.rotation.eulerAngles.x < Reference.FLYABLE_ANGLE &&
			transform.rotation.eulerAngles.x > -Reference.FLYABLE_ANGLE;
	}
}
