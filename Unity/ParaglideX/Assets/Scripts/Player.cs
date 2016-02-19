using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private float speed = 6;
	public bool deployed;
	public bool flying;
	private Rigidbody flyingBody;
	private CapsuleCollider flyingCollider;
	private Quaternion startRotation;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		flyingBody = GetComponent<Rigidbody> ();
		startRotation = flyingBody.rotation;

		//Turn of the cursor while in fps
		Cursor.visible = false;
	
	}

	void FixedUpdate () {
		playerControl ();
		viewControl ();
		print ("Vario: " + flyingBody.velocity.y +
			" Speed: " + transform.InverseTransformVector(flyingBody.velocity).z);
	}

	void OnTriggerEnter(Collider collider){ //When hitting ground
		if (collider.gameObject.name == "Terrain") {
			setFlying (false);
		}
	}

	void OnTriggerExit(Collider collider){ //When leaving ground
		if (collider.gameObject.name == "Terrain") {
			setFlying (true);
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
			flyingBody.rotation = Quaternion.Euler (flyingBody.rotation.eulerAngles.x, 
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

	public bool getDeployed(){
		return deployed;
	}
}
