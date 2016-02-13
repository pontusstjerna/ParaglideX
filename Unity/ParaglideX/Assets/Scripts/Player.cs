﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private float speed = 6;
	private bool deployed = false;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();

		//Turn of the cursor while in fps
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		//If the player is on ground
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

		} else {//Apply gravity!
			moveDirection.y -= Reference.GRAVITY;
		}

		//Apply movement
		controller.Move(moveDirection * Time.deltaTime);

		if (!deployed) {//Turn the whole player when not deployed

			//This should actually not be Space.World but Space.Paraglider, 
			//since if in a turn, you should look horizontally relative to the glider.
			transform.Rotate (0, Input.GetAxis ("mouseX") * Time.deltaTime * Reference.MOUSE_SENSITIVITY, 0, Space.World);
		}
	}

	public bool getDeployed(){
		return deployed;
	}
}
