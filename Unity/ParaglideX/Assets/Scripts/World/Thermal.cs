using UnityEngine;
using System.Collections;

public class Thermal : MonoBehaviour {

    ArrayList inThermal;

	// Use this for initialization
	void Start () {
        inThermal = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        foreach(IBlowable blowable in inThermal)
        {
            blowable.AddWind(Vector3.up * 300);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        IBlowable blowable = (IBlowable)collider.GetComponent(typeof(IBlowable));

        inThermal.Add(blowable);
    } 

    void OnTriggerExit(Collider collider)
    {
        IBlowable blowable = (IBlowable)collider.GetComponent(typeof(IBlowable));

        inThermal.Remove(blowable);
    }
  
}
