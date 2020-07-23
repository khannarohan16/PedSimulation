using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public Waypoint previousWaypoint;
	public Waypoint nextWaypoint;

	//Radius of waypoint area
	[Range(0,3)]
	public float radius = 2f;

	void Awake()
	{
		SphereCollider col = gameObject.AddComponent<SphereCollider>();
		col.radius = 1f;
		col.isTrigger = true;
		//gameObject.tag = "Waypoint";
	}
	
	public Vector3 GetPosition()
	{
		Vector3 minBound = transform.position + transform.right * radius;
		Vector3 maxBound = transform.position - transform.right * radius;

		Vector3 position = Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
		

		position = transform.position;
		

		return position;
 
	}
}
