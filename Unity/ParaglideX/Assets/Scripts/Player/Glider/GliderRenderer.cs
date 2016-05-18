using UnityEngine;
using System.Collections;

public class GliderRenderer : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {

		/*
		 *     void Start()
     {
          // First set both animations layer value to something different to each other. This allows you to play them both at the same time.
 
         animation["aim"].layer = 1;
         animation["dodge"].layer = 2;
 
         // Second set both their speed to 0.
 
         animation["aim"].speed = 0;
         animation["dodge"].speed = 0;
 
         // Thirdly play both the animations
 
         animation.Play("aim");
         animation.Play("dodge");
     }
 
     // Use the mouse input to set the normalized time of the animations to what you want. Something like this.
 
     public float maxMouse = 366;
     void Update()
         {
         float newTimeAim = Mathf.Min(maxMouse, Mathf.Max(0, Input.mousePosition.y)) / maxMouse;
         float newTimDodge = Mathf.Min(maxMouse, Mathf.Max(0, Input.mousePosition.x)) / maxMouse;
     
         animation["aim"].normalizedTime = newTimeAim ;
         animation["dodge"].normalizedTime = newTimDodge ;
     }
 
		 * */

	}

	private void LeftBrakeFrame(){

	}

	private void RightBrakeFrame(){

	}
}
