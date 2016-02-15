using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private float speed = 6;
	public bool deployed = true;
	private Rigidbody flyingBody;
	private CapsuleCollider flyingCollider;
	private Glider glider;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		flyingBody = GetComponent<Rigidbody> ();
		flyingCollider = GetComponent<CapsuleCollider> ();
		glider = transform.FindChild ("Glider").GetComponent<Glider> ();

		//Turn of the cursor while in fps
		Cursor.visible = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		playerControl ();
		print ("Vario: " + flyingBody.velocity.y);
	}

	//When hitting something
	void OnCollisionEnter(Collision collisionObj){
		if (collisionObj.gameObject.name == "Terrain") { //When the flying collider hits the ground
			setFlying(false);
		}
	}

	private void playerControl(){ //All the code for player control

		if (controller.enabled) {

			//If the player is on ground and no active glider
			if (controller.isGrounded) {
				//Sprint
				if (Input.GetKey (KeyCode.LeftShift)) {
					speed = 12;
				} else {
					speed = 6;
				}
			
				if (Input.GetKeyUp (KeyCode.Space)) { //Deploy the glider. Kind of lags. 
					deployed = !deployed;
				}
			
				//Add the user inputs. X is sideways, Z is forward/backward.
				moveDirection = new Vector3 (Input.GetAxis ("sideways"), 0, Input.GetAxis ("forward"));
			
				//Translate the direction to world space
				moveDirection = transform.TransformDirection (moveDirection);
			
				//Apply the speed! 
				moveDirection *= speed;
			
			} else {//In the air
				if (deployed && controller.enabled && glider.flyAble ()) { //Flying
					setFlying (true);
				} else { //Falling without glider
					moveDirection.y -= Reference.GRAVITY;
				}
			}
		
			//Apply movement
			controller.Move (moveDirection * Time.deltaTime);
		
			if (!deployed) {//Turn the whole player when not deployed
			
				//This should actually not be Space.World but Space.Paraglider, 
				//since if in a turn, you should look horizontally relative to the glider.
				transform.Rotate (0, Input.GetAxis ("mouseX") * Time.deltaTime * Reference.MOUSE_SENSITIVITY, 0, Space.World);
			}
		}
	}

	private void setFlying(bool flying){//Sets the mode to flying or not

		//Switch flight physics
		controller.enabled = !flying;
		flyingBody.isKinematic = !flying;
		flyingBody.useGravity = flying;
		flyingCollider.enabled = flying;
	}

	public bool getDeployed(){
		return deployed;
	}
}
