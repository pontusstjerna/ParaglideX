using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public const float PLAYER_WEIGHT = 80f;
	public const float GRAVITY = -9.807f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		CharacterController controller = GetComponent<CharacterController> ();

		//If the player is on ground
		if (controller.isGrounded) {
			controller.Move (new Vector3 (1, 0, 0) * Time.deltaTime);
		} else {//Apply gravity!
			controller.Move(new Vector3(0,GRAVITY, 0)*Time.deltaTime);
		}
	}
}
