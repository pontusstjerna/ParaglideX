﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IBlowable {

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private float speed = 6;
	public bool deployed;
	public bool flying;
	private Rigidbody flyingBody;
	private CapsuleCollider flyingCollider;
	private Quaternion startRotation;
	private AudioSource windSound;
	private Vector3 relativeVelocityAir;

	private const float maxWindVelocity = 20;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		flyingBody = GetComponent<Rigidbody> ();
		windSound = GetComponent<AudioSource> ();
		startRotation = flyingBody.rotation;
		Reference.blowables.Add (this);

		//Turn of the cursor while in fps
		Cursor.visible = false;

		//Start playing the wind sound
		windSound.Play ();
	}

	void Update(){
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}

		PlayWindSound ();
	}

	void FixedUpdate () {
		playerControl ();
		viewControl ();
		//print ("Vario: " + flyingBody.velocity.y + " Speed: " 
		  //     + new Vector2(flyingBody.velocity.x, flyingBody.velocity.z).magnitude);
	}

	void OnTriggerEnter(Collider collider){ //When hitting ground
		if (collider.gameObject.name == "Terrain") {
			setFlying (false);
		}
	}

	void OnTriggerExit(Collider collider){ //When leaving ground
		if (collider.gameObject.name == "Terrain" && deployed) {
			setFlying (true);
		}
	}
	public bool getDeployed(){
		return deployed;
	}

	public float GetMass(){
		return flyingBody.mass;
	}

	public Vector3 GetRelativeVelocityAir(){
		return relativeVelocityAir;
	}

	public void AddWind(Vector3 wind){ //Temporary solution. Area should change when wind is comming from different direction
		relativeVelocityAir = transform.InverseTransformVector (flyingBody.velocity - wind);
		flyingBody.AddForce(Math.GetWindForce(wind - flyingBody.velocity, Reference.AREA_PLAYER_FRONT, Reference.DRAG_COEFFICIENT_PLAYER_FRONT));
	}

	public Vector3 GetWorldPosition(){
		return flyingBody.position;
	}

	private void PlayWindSound(){
		float velAir = GetRelativeVelocityAir ().magnitude;
		print (velAir);
		windSound.volume = velAir / maxWindVelocity;
		windSound.pitch = GetWindPitch (velAir);
	}

	private float GetWindPitch(float velocityAir){
		float startPitchVelocity = 8;
		if (velocityAir > startPitchVelocity) {
			return velocityAir / startPitchVelocity;
		} else {
			return 1;
		}
	}

	private void unDeployedControl(){ //All the code for player control
		
		//If the player is on ground and no active glider
		if (controller.isGrounded) {
			//Sprint
			if (Input.GetKey (KeyCode.LeftShift)) {
				speed = 12;
			} else {
				speed = 6;
			}
			
			//Add the user inputs. X is sideways, Z is forward/backward.
			moveDirection = new Vector3 (Input.GetAxis ("sideways"), 0, Input.GetAxis ("forward"));
			
			//Translate the direction to world space
			moveDirection = transform.TransformDirection (moveDirection);
			
			//Apply the speed! 
			moveDirection *= speed;
			
		} else {//In the air
			moveDirection.y -= Reference.GRAVITY;
		}
		
		//Apply movement
		controller.Move (moveDirection * Time.deltaTime);
		
		//This should actually not be Space.World but Space.Paraglider, 
		//since if in a turn, you should look horizontally relative to the glider.
		transform.Rotate (0, Input.GetAxis ("mouseX") * Time.deltaTime * Reference.MOUSE_SENSITIVITY, 0, Space.World);
	}
	
	private void deployedControl(){
		//Walk forward
		//This should be modified so that when the running speed is to high, force is reduced
		flyingBody.AddRelativeForce (Vector3.forward * Input.GetAxis ("forward")*800);
	}
	
	private void playerControl(){
		if (!flying) {
			//Listen for deploy input
			if (Input.GetKeyUp (KeyCode.Space) && flyingBody.velocity.y < 0.5f && flyingBody.velocity.y > -0.5f) { //Deploy the glider. Kind of lags. 
				setDeployed(!deployed);
			}
			if (deployed) {
				deployedControl ();
			} else {
				unDeployedControl ();
			}
		}
	}
	
	private void setFlying(bool flying){//Sets the mode to flying or not
		
		//Switch flight physics
		this.flying = flying;
		flyingBody.freezeRotation = !flying;
		if(flyingBody.freezeRotation){//Rotate the player right when landing
			flyingBody.rotation = Quaternion.Euler (startRotation.eulerAngles.x, 
			                                        flyingBody.rotation.eulerAngles.y, startRotation.eulerAngles.z);
		}
	}
	
	private void setDeployed(bool deployed){ //Sets the mode to deployed or not
		
		//Swith ground handling physics
		this.deployed = deployed;
		controller.enabled = !deployed;
		flyingBody.useGravity = deployed;
		flyingBody.isKinematic = !deployed;
	}
	
	private void viewControl(){
		if (Input.GetKeyUp (KeyCode.V) && deployed) { //If v is pressed, change camera
			
			GameObject fpvCamera = transform.FindChild ("FpvCamera").gameObject;
			GameObject thirdPvCamera = transform.FindChild ("ThirdViewCamera").gameObject;
			fpvCamera.SetActive(!fpvCamera.activeSelf);
			thirdPvCamera.SetActive(!thirdPvCamera.activeSelf);
		}
	}

}
