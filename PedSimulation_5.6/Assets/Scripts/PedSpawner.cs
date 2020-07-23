using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedSpawner : MonoBehaviour {

	public List<GameObject> pedPrefab;    //To hold list of different pedestrians to spawn
	
	List<GameObject> activePeds;	//Once spawned hold them in this list ? probably not needed
	public List<GameObject> inactivePeds;  //After despqwning hold them in this

	public List<Waypoint> spawnPoints;	//list to be updated based on characters distance colliders

	[SerializeField] int noToSpawn;
	[SerializeField] float spawnInterval;

	void Start()
	{
		StartCoroutine("SpawnPeds");
	}

	IEnumerator SpawnPeds()
	{
		yield return new WaitForSeconds(.5f);
		int count = 0;
		while(count <= noToSpawn)
		{
			GameObject obj = Instantiate(pedPrefab[Random.Range(0,pedPrefab.Count)]);

			Waypoint randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
			obj.transform.position = randomPoint.GetPosition();
			obj.GetComponent<CharacterController>().wayPoint = randomPoint;

			count++;
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine("PeriodicSpawn");
	}

	IEnumerator PeriodicSpawn()
	{
		yield return new WaitForSeconds(spawnInterval);
		if(inactivePeds.Count != 0 && spawnPoints.Count != 0)
		{
			foreach (var item in inactivePeds)
			{
				Waypoint randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
				item.transform.position = randomPoint.GetPosition();
				item.SetActive(true);
				item.GetComponent<CharacterController>().wayPoint = randomPoint;
			}
		}
		StartCoroutine("PeriodicSpawn");
	}

	public void DeSpawn(GameObject p)
	{
		GameObject ped = p;
		inactivePeds.Add(ped);
		ped.SetActive(false);
	}
}
