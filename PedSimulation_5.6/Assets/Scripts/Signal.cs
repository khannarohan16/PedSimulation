using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour {

	public GameObject crossing;

	public float initialDelay;
	public float activeTime;
	public float inactiveTime;

	void Start()
	{
		Invoke("Activate", initialDelay);
	}

	void Activate()
	{
		Waypoint[] waypoints = crossing.GetComponentsInChildren<Waypoint>();

		foreach (Waypoint point in waypoints)
		{
			point.canCross = true;
		}

		Invoke("Deactivate", activeTime);
	}

	void Deactivate()
	{
		Waypoint[] waypoints = crossing.GetComponentsInChildren<Waypoint>();

		foreach (Waypoint point in waypoints)
		{
			point.canCross = false;
		}

		crossing.SetActive(false);
		Invoke("Activate", inactiveTime);
	}
}
