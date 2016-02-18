using UnityEngine;
using System.Collections;

public class GliderRenderer : MonoBehaviour {

	private MeshRenderer gliderRenderer;
	private Player player;

	// Use this for initialization
	void Start () {
		gliderRenderer = GetComponent<MeshRenderer> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		gliderRenderer.enabled = player.getDeployed ();
	}
}
