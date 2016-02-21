using UnityEngine;
using System.Collections;

public class WindController : MonoBehaviour {
	public Vector3 wind;
	public float windStrength;
	private float precision = 0.1f;

	// Use this for initialization
	void Start () {

		//Use only direction of vector and multiply with the wind strength in m/s.
		wind = wind.normalized * windStrength;
	}

	void FixedUpdate(){

		//Blow on all blowable objects
		foreach(IBlowable obj in Reference.blowables){
			obj.AddWind(GetWindAtPos(obj.GetWorldPosition()));
			print ("Added wind: " + GetWindAtPos(obj.GetWorldPosition()));
		}
	}

	public Vector3 GetVelocity(IBlowable obj){
		return GetWindAtPos(obj.GetWorldPosition ());
	}

	private Vector3 GetWindAtPos(Vector3 pos){
		float maxSoarAltitude = 40 * windStrength;

		//A cheap solution for upwind to decrease when going higher
		return GetTerrainGradient (pos)*windStrength*(1 - (HeightToGround(pos)/maxSoarAltitude));
	}

	private Vector3 GetTerrainGradient(Vector3 worldPos){

		//The gradient's distance value, the smaller value, the bigger precision
		Vector3 deltaWindDir = wind.normalized*0.1f;

		//The difference in horizontal movement
		Vector3 hPos = new Vector3(worldPos.x + deltaWindDir.x, worldPos.y, worldPos.z + deltaWindDir.z);

		//The current position relative to terrain
		Vector3 currentGroundPos = new Vector3(worldPos.x, Terrain.activeTerrain.SampleHeight(worldPos), worldPos.z);

		//The position with the differented horizontal positions
		Vector3 deltaGroundPos = new Vector3 (hPos.x, Terrain.activeTerrain.SampleHeight(hPos), hPos.z);

		return ((deltaGroundPos - currentGroundPos).normalized);
	}

	public float HeightToGround(Vector3 pos){
		return pos.y - Terrain.activeTerrain.SampleHeight (pos);
	}
}
