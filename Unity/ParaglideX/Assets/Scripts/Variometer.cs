using UnityEngine;
using System.Collections;

public class Variometer : MonoBehaviour {
	private AudioSource audio;
	private Rigidbody player;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		player = GameObject.Find ("Player").GetComponent<Rigidbody> ();
		audio.Play ();
		audio.pitch = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		if (player.velocity.y > 0) {
			audio.pitch = player.velocity.y / 5;
		} else {
			audio.pitch = 0;
		}
		//audio.Play ();
	}
}
