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
		
		//body.AddForce (-body.velocity);

		body.velocity = new Vector3(0, body.velocity.y/4, -body.velocity.y*9/4);

		//print ("Player: " + playerBody.velocity + " Glider: " + body.velocity);
	}

	public bool flyAble(){
		return transform.rotation.eulerAngles.x < Reference.FLYABLE_ANGLE && transform.rotation.eulerAngles.x > -Reference.FLYABLE_ANGLE;
	}
}
