using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public const float PLAYER_WEIGHT = 80f;
	public const float GRAVITY = 9.807f;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private float speed = 6;

	private const float MOUSE_SENSITIVITY = 0.3f;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		//If the player is on ground
		if (controller.isGrounded) {

			//Add the user inputs. X is sideways, Z is forward/backward.
			moveDirection = new Vector3 (Input.GetAxis ("sideways"), 0, Input.GetAxis("forward"));

			//Translate the direction to world space
			moveDirection = transform.TransformDirection (moveDirection);

			//Apply the speed!
			moveDirection *= speed;

		} else {//Apply gravity!
			moveDirection.y -= GRAVITY;
		}

		//Apply movement
		controller.Move(moveDirection * Time.deltaTime);

		transform.Rotate (0, Input.GetAxis ("mouseX")*MOUSE_SENSITIVITY, 0);
	}
}
