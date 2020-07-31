using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaypointTool : EditorWindow 
{
	[MenuItem("Tools/Waypoint Editor")]
	public static void Open()
	{
		GetWindow<WaypointTool>();
	}

	[SerializeField] Transform waypointRoot;

	void OnGUI()
	{
		SerializedObject obj = new SerializedObject(this);

		EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

		
		if (waypointRoot == null)
		{
			EditorGUILayout.HelpBox("Root Transform must be initialized", MessageType.Warning);
		}
		else
		{
			EditorGUILayout.BeginVertical("box");
			DrawButtons();
		}

		obj.ApplyModifiedProperties();
	}

	void DrawButtons()
	{
		//Default button to always create waypoint in end
		if(GUILayout.Button("Create Waypoint"))
		{
			CreateWaypoint();
		}

		//Buttons to create waypoint before / after the selected waypoint or delete the waypoint
		if(Selection.activeGameObject!=null && Selection.activeGameObject.GetComponent<Waypoint>())
		{
			if (GUILayout.Button("Create Waypoint Before"))
			{
				CreateWaypointBefore();
			}
			if (GUILayout.Button("Create Waypoint After"))
			{
				CreateWaypointAfter();
			}
			if (GUILayout.Button("Delete Waypoint"))
			{
				DeleteWaypoint();
			}
			if(GUILayout.Button("Add Branch"))
			{
				AddBranch();
			}
		}
	}

	void AddBranch()
	{
		GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
		
		Waypoint branchedFrom = Selection.activeGameObject.GetComponent<Waypoint>();
		branchedFrom.branches.Add(waypoint);

		waypoint.previousWaypoint = branchedFrom;
		waypoint.isBranch = true;

		waypoint.transform.position = branchedFrom.transform.position;
		waypoint.transform.forward = branchedFrom.transform.forward;

		Selection.activeGameObject = waypoint.gameObject;
	}

	void CreateWaypoint()
	{
		GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
		if (waypointRoot.childCount > 1)
		{
			waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
			waypoint.previousWaypoint.nextWaypoint = waypoint;

			waypoint.transform.position = waypoint.previousWaypoint.transform.position;
			waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
		}

		Selection.activeGameObject = waypoint.gameObject;
	}

	void CreateWaypointBefore()
	{
		GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
		Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

		newWaypoint.transform.position = selectedWaypoint.transform.position;
		newWaypoint.transform.forward = selectedWaypoint.transform.forward;

		if(selectedWaypoint.previousWaypoint != null)
		{
			newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
			newWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
		}

		selectedWaypoint.previousWaypoint = newWaypoint;
		newWaypoint.nextWaypoint = selectedWaypoint;

		newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

		Selection.activeGameObject = newWaypoint.gameObject;
	}

	void CreateWaypointAfter()
	{
		GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
		Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

		newWaypoint.transform.position = selectedWaypoint.transform.position;
		newWaypoint.transform.forward = selectedWaypoint.transform.forward;

		if (selectedWaypoint.nextWaypoint != null)
		{
			newWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
			newWaypoint.nextWaypoint.previousWaypoint = newWaypoint;
		}

		selectedWaypoint.nextWaypoint = newWaypoint;
		newWaypoint.previousWaypoint = selectedWaypoint;

		newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

		Selection.activeGameObject = newWaypoint.gameObject;
	}

	void DeleteWaypoint()
	{
		Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
		
		if (selectedWaypoint.isBranch)
		{
			selectedWaypoint.previousWaypoint.branches.Remove(selectedWaypoint);
		}
		else
		{
			if (selectedWaypoint.previousWaypoint != null)
			{
				selectedWaypoint.previousWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
			}
			if (selectedWaypoint.nextWaypoint != null)
			{
				selectedWaypoint.nextWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
			}
		}
		

		DestroyImmediate(selectedWaypoint.gameObject);
	}
}
