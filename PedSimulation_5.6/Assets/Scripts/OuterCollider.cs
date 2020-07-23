﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterCollider : MonoBehaviour {
	[SerializeField] PedSpawner pedSpawner;
	[SerializeField] Waypoint _point;

	void OnTriggerEnter(Collider other)
	{
		if((_point = other.GetComponent<Waypoint>()) != null)
		{
			if (!pedSpawner.spawnPoints.Contains(_point))
			{
				pedSpawner.spawnPoints.Add(_point);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<CharacterController>() != null)
		{
			pedSpawner.DeSpawn(other.gameObject);
		}
	}
}
