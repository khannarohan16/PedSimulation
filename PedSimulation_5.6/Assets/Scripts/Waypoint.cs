using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public Waypoint previousWaypoint;
	public Waypoint nextWaypoint;

	[HideInInspector]
	public BlockScript block;

	//Radius of waypoint area
	[Range(0,2)]
	public float radius = 1f;

	public List<Waypoint> branches;

	[Range(0f,1f)]
	public float branchFactor = 0.5f; //How likely is the pedsetrain taking branch
	public bool isBranch = false;   //To be set if the waypoint is branch of some node
	
	public bool isCrossing = false;	//To determine whether this waypoint is of crossing
	public bool canCross = true;	//Controlled by signal if the waypoint is of crossing

	void Awake()
	{
		SphereCollider col = gameObject.AddComponent<SphereCollider>();
		col.radius = 1f;
		col.isTrigger = true;
		if (!isCrossing)
		{
			block = GetComponentInParent<BlockScript>();
		}
	}
	
	public Vector3 GetPosition()
	{
		Vector3 deviation = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));

		Vector3 position = transform.position + deviation;
		
		return position;
	}
}
