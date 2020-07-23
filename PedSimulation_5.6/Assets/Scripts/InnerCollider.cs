using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerCollider : MonoBehaviour {
	[SerializeField] PedSpawner pedSpawner;
	void OnTriggerEnter(Collider other)
	{
		if(Time.time > 5f)
		{
			if (pedSpawner.spawnPoints.Contains(other.GetComponent<Waypoint>()))
			{
				pedSpawner.spawnPoints.Remove(other.GetComponent<Waypoint>());
			}
		}
	}
}
