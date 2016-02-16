using System.Collections;
using UnityEngine;

public class Math {
	public static Vector3 getDrag(Vector3 velocity, float dragCoefficient, float airDensity, float area){
		float sqrMagnitude = velocity.sqrMagnitude;
		return -velocity.normalized * 0.5f * dragCoefficient * airDensity * area * sqrMagnitude;
	}
}

/*PHYSICS
 * v = velocity, t = time, a = average acceleration, F = force, m = mass
 * 
 * v = at
 * F = ma
 * 
 * Fd = drag, C = drag coefficient, p = air density, A = area exposed to air flow, v = velocity
 * 
 * Fd = ½*C*p*A*pow(2)
 */

/*
 * -12 = -10*t
 * F = 80*10 = 800N TOTAL
*/
