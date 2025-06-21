using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMover : MonoBehaviour
{
	public enum EndOfPathBehavior { Stop, Loop, Destroy, Custom }

	[Header("Waypoint System")]
	[Tooltip("Biarkan kosong untuk memilih path secara acak dari EnemySpawner")]
	public WaypointSystem path; // Opsional: untuk konfigurasi manual
	public float remainingDistance = 0.3f;

	[Header("Movement Settings")]
	public float moveSpeed = 5f;
	public float pauseDurationAtWaypoint = 0f;

	[Header("End of Path Settings")]
	public int towerDamage = 10; // Jumlah damage ke tower saat mencapai akhir
	public EndOfPathBehavior endOfPathBehavior = EndOfPathBehavior.Destroy;

	[Header("Events")]
	public UnityEvent onReachWaypoint;
	public UnityEvent onReachEndOfPath;

	[Header("Spawner Integration")]
	public EnemySpawner spawner; // Opsional: referensi ke EnemySpawner

	private List<Transform> points = new List<Transform>();
	private int destPoint = 0;
	private NavMeshAgent agent;
	private bool isPaused;
	[HideInInspector] public bool reachedToEnd;

	// Dipanggil oleh EnemySpawner untuk mengatur path secara dinamis
	public void SetPath(WaypointSystem newPath)
	{
		path = newPath;
	}

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		if (agent == null)
		{
			Debug.LogError($"NavMeshAgent tidak ditemukan pada {gameObject.name}!");
			return;
		}

		if (path == null)
		{
			Debug.LogWarning($"WaypointSystem tidak diatur pada {gameObject.name}. Menunggu assignment dari EnemySpawner.");
			return;
		}

		points = path.waypoints;
		if (points == null || points.Count == 0)
		{
			Debug.LogError($"Tidak ada waypoint yang diatur pada WaypointSystem untuk {gameObject.name}!");
			return;
		}

		agent.speed = moveSpeed;
		agent.autoBraking = false;
		GotoNextPoint();
	}

	void Update()
	{
		if (isPaused || !agent.enabled) return;

		if (agent.remainingDistance < remainingDistance)
		{
			if (pauseDurationAtWaypoint > 0)
			{
				StartCoroutine(PauseAtWaypoint());
			}
			else
			{
				GotoNextPoint();
			}
		}
	}

	void GotoNextPoint()
	{
		if (points.Count == 0) return;

		if (destPoint >= points.Count)
		{
			HandleEndOfPath();
			return;
		}

		agent.destination = points[destPoint].position;
		onReachWaypoint.Invoke();
		destPoint++;
	}

	void HandleEndOfPath()
	{
		reachedToEnd = true;
		onReachEndOfPath.Invoke();

		// Kurangi kesehatan tower
		GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
		if (gameManager != null)
		{
			gameManager.Reduce_Tower_Health(towerDamage);
		}
		else
		{
			Debug.LogWarning($"GameManager tidak ditemukan untuk mengurangi kesehatan tower pada {gameObject.name}!");
		}

		// Beri tahu spawner bahwa musuh telah mencapai akhir
		if (spawner != null)
		{
			spawner.OnEnemyReachedEnd(gameObject);
		}

		// Hancurkan musuh menggunakan komponen Health
		Health health = GetComponent<Health>();
		if (health != null && health.targetType == TargetType.Enemy)
		{
			health.ApplyDamage(health.maxHealthValue); // Berikan damage cukup untuk menghancurkan
		}
		else if (endOfPathBehavior == EndOfPathBehavior.Destroy)
		{
			Destroy(gameObject); // Hancurkan langsung jika tidak ada komponen Health
		}

		switch (endOfPathBehavior)
		{
			case EndOfPathBehavior.Stop:
				agent.enabled = false;
				break;
			case EndOfPathBehavior.Loop:
				destPoint = 0;
				GotoNextPoint();
				break;
			case EndOfPathBehavior.Destroy:
				// Sudah ditangani di atas oleh Health atau Destroy
				break;
			case EndOfPathBehavior.Custom:
				break;
		}
	}

	IEnumerator PauseAtWaypoint()
	{
		isPaused = true;
		agent.isStopped = true;
		yield return new WaitForSeconds(pauseDurationAtWaypoint);
		agent.isStopped = false;
		isPaused = false;
		GotoNextPoint();
	}
}