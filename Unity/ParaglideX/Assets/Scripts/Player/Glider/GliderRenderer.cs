using UnityEngine;
using System.Collections;

public class GliderRenderer : MonoBehaviour {

	private Animation animations;

	// Use this for initialization
	void Start () {
		animations = GetComponent<Animation> ();
		animations ["GliderArmature|LeftBrakeArmatureAction"].AddMixingTransform (GetComponent<Transform> ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("brakeL") > 0) {
			LeftBrakeFrame ();
		} else if (Input.GetAxis("brakeR") > 0) {
			RightBrakeFrame ();
		} else {
			animations.Stop ();
		}
	}

	private void LeftBrakeFrame(){
		//1 = 30 frames because time is in seconds.
		animations["GliderArmature|LeftBrakeArmatureAction"].time = Input.GetAxis("brakeL");
		animations.Play ();
	}

	private void RightBrakeFrame(){
		//1 = 30 frames because time is in seconds.
		animations["GliderArmature|LeftBrakeArmatureAction"].time = 1.1f + Input.GetAxis("brakeR");
		animations.Play ();
	}
}
