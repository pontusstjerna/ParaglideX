using UnityEngine;
using System.Collections;

public class WindController : MonoBehaviour {
	public Vector3 wind;
	public float windStrength;

	// Use this for initialization
	void Start () {

		//Use only direction of vector and multiply with the wind strength in m/s.
		wind = wind.normalized * windStrength;
	}

	void FixedUpdate(){

		//Blow on all blowable objects
		foreach(IBlowable obj in Reference.blowables){
			obj.AddWind(wind);
		}
	}

	public Vector3 GetVelocity(){
		return wind;
	}
}
