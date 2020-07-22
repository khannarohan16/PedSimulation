using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedSpawner : MonoBehaviour {

	public GameObject pedPrefab;    //todo make a list of different prefabs
	[SerializeField] int noToSpawn;

	void Start()
	{
		StartCoroutine("SpawnPeds");
	}

	IEnumerator SpawnPeds()
	{
		int count = 0;
		while(count <= noToSpawn)
		{
			GameObject obj = Instantiate(pedPrefab);
			Transform child = transform.GetChild(Random.Range(0, transform.childCount));
			obj.GetComponent<CharacterController>().wayPoint = child.GetComponent<Waypoint>();
			
			//Vector3 pos = transform.TransformPoint(child.position);

			print("Actual:" + child.position);
			print("World" + transform.TransformPoint(child.position));
			print("World" + transform.TransformPoint(child.localPosition));

			//obj.transform.position = transform.TransformPoint(child.position);

			count++;

			yield return new WaitForEndOfFrame();
		}
	}
}
