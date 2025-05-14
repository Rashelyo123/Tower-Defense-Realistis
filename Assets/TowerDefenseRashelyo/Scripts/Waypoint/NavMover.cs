// AliyerEdon@mail.com Christmas 2022
// Attach this component to your enemy actor to make it mover along waypoints 
// Used unity's navigation system to follow the targets (waypoints)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NavMover : MonoBehaviour
{


	List<Transform> points = new List<Transform>();

	private int destPoint = 0;
	private UnityEngine.AI.NavMeshAgent agent;

	[Space(5)]
	[Header("Waypoint System")]
	public string waypointName;
	WaypointSystem path;
	public float remainingDistance = 0.3f;

	[HideInInspector] public bool reachedToEnd;

	void Start()
	{

		path = GameObject.Find(waypointName).GetComponent<WaypointSystem>();

		points = path.waypoints;

		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;

		GotoNextPoint();
	}

	void GotoNextPoint()
	{

		// Returns if no points have been set up
		if (points.Count == 0)
			return;

		// Reached to the end of the waypoints
		if (destPoint == points.Count)
		{
			// if (GetComponent<AnimationList>().actor)
			// {
			// 	GetComponent<AnimationList>().actor.CrossFade(GetComponent<AnimationList>().fireClip);
			// }
			reachedToEnd = true;
			agent.enabled = false;
			return;
		}

		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		if (destPoint < points.Count)
			destPoint = destPoint + 1;


	}


	void Update()
	{
		// Choose the next destination point when the agent gets
		// close to the current one.
		if (agent.enabled)
		{
			if (agent.remainingDistance < remainingDistance)
				GotoNextPoint();
		}
	}
}
