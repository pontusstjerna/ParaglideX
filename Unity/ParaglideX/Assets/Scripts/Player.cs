using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private float speed = 6;
	public bool deployed = true;
	private Rigidbody flyingBody;
	private CapsuleCollider flyingCollider;
	private bool onGround = false;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		flyingBody = GetComponent<Rigidbody> ();
		flyingCollider = GetComponent<CapsuleCollider> ();

		//Turn of the cursor while in fps
		Cursor.visible = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		//If the player is on ground and no active glider
		if (controller.isGrounded) {
			//Sprint
			if (Input.GetKey (KeyCode.LeftShift)) {
				speed = 12;
			}else{
				speed = 6;
			}

			if(Input.GetKeyUp (KeyCode.Space)){ //Deploy the glider. Kind of lags. 
				deployed = !deployed;
			}

			//Add the user inputs. X is sideways, Z is forward/backward.
			moveDirection = new Vector3 (Input.GetAxis ("sideways"), 0, Input.GetAxis("forward"));

			//Translate the direction to world space
			moveDirection = transform.TransformDirection (moveDirection);

			//Apply the speed! 
			moveDirection *= speed;

		} else {//In the air
			if(deployed && controller.enabled && !onGround){ //Flying
				switchGroundState();
			}else{ //Falling without glider
				moveDirection.y -= Reference.GRAVITY;
			}
		}

		//Apply movement
		controller.Move(moveDirection * Time.deltaTime);

		if (!deployed) {//Turn the whole player when not deployed

			//This should actually not be Space.World but Space.Paraglider, 
			//since if in a turn, you should look horizontally relative to the glider.
			transform.Rotate (0, Input.GetAxis ("mouseX") * Time.deltaTime * Reference.MOUSE_SENSITIVITY, 0, Space.World);
		}
	}

	//When hitting something
	void OnCollisionEnter(Collision collisionObj){
		if (collisionObj.gameObject.name == "Terrain") {
			switchGroundState();
			onGround = true;
		}
	}

	private void switchGroundState(){ //Just a preliminary solution, planning on making this better.

		//Switch flight physics
		controller.enabled = !controller.enabled;
		flyingBody.isKinematic = !flyingBody.isKinematic;
		flyingBody.useGravity = !flyingBody.useGravity;
		flyingCollider.enabled = !flyingCollider.enabled;
	//	onGround = true;
	}

	public bool getDeployed(){
		return deployed;
	}
}
