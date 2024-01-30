using StealthTD.Enemy;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyMoveController : MonoBehaviour
{
	#region Private Fields

	[Inject(Id = "PlayerTransform")]
	private readonly Transform playerTransform;

	[Inject]
	private readonly NavMeshAgent navMeshAgent;

	[Inject]
	private readonly EnemySharedData sharedData;

	#endregion Private Fields

	#region Public Properties

	[field: SerializeField, ReadOnly]
	public float RemainingDistance { get; private set; }

	[field: SerializeField, ReadOnly]
	public float StoppingDistance { get; private set; }

	[field: SerializeField, ReadOnly]
	public float DistanceToPlayer { get; private set; }

	[field: SerializeField, ReadOnly]
	public Vector3 Velocity { get; private set; }

	#endregion Public Properties

	#region Public Methods

	public void WalkTowardsDestination(Vector3 destination)
	{
		if (navMeshAgent.isStopped)
			navMeshAgent.isStopped = false;

		if (navMeshAgent.speed != 2)
			navMeshAgent.speed = 2;

		navMeshAgent.SetDestination(destination);
	}

	public void RunTowardsDestination(Vector3 destination)
	{
		if (navMeshAgent.isStopped)
			navMeshAgent.isStopped = false;

		if (navMeshAgent.speed != 4)
			navMeshAgent.speed = 4;

		navMeshAgent.SetDestination(destination);
	}

	public void SetAngularSpeed(float speed)
	{
		if (navMeshAgent.angularSpeed != speed)
			navMeshAgent.angularSpeed = speed;
	}

	public void StopMoving()
	{
		if (navMeshAgent.isStopped)
			navMeshAgent.isStopped = true;

		if (navMeshAgent.speed != 0)
			navMeshAgent.speed = 0;
	}

	public void SetStoppingDistance(float stoppingDistance)
	{
		if (navMeshAgent.stoppingDistance != stoppingDistance)
			navMeshAgent.stoppingDistance = stoppingDistance;
	}

	public void LookAtPosition(Vector3 targetPosition)
	{
		targetPosition.y = navMeshAgent.transform.position.y;
		Vector3 lookDirection = (targetPosition - navMeshAgent.transform.position).normalized;

		if (lookDirection == Vector3.zero)
			return;

		Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
		navMeshAgent.transform.rotation = Quaternion.RotateTowards(navMeshAgent.transform.rotation, lookRotation, 350 * Time.deltaTime);
	}

	public void LookInRandomDirection()
	{
		Vector2 direction2d = Random.insideUnitCircle;
		Vector3 direction3d = new(direction2d.x, 0, direction2d.y);
		navMeshAgent.transform.LookAt(navMeshAgent.transform.position + direction3d);
	}

	public Vector3 FindBestAttackPosition(bool isPlayerVisible, bool hasLineOfFire, LayerMask lineOfFireLayerMask)
	{
		if (!isPlayerVisible)
			return sharedData.LastKnownPlayerPosition;

		float keepDistanceToPlayer = 3;

		if (hasLineOfFire)
		{
			Vector3 pos = playerTransform.position + ((navMeshAgent.transform.position - playerTransform.position).normalized * keepDistanceToPlayer);

			if (IsValidPosition(pos))
				return pos;
		}

		Vector3[] points =
		{
			playerTransform.position + Vector3.forward * keepDistanceToPlayer,
			playerTransform.position + Vector3.back * keepDistanceToPlayer,
			playerTransform.position + Vector3.left * keepDistanceToPlayer,
			playerTransform.position + Vector3.right * keepDistanceToPlayer
		};

		points = points.OrderBy(point => Vector3.Distance(point, navMeshAgent.transform.position)).ToArray();

		foreach (Vector3 point in points)
			if (IsValidPosition(point) && !Physics.Linecast(point, playerTransform.position, lineOfFireLayerMask))
				return point;

		return playerTransform.position + ((navMeshAgent.transform.position - playerTransform.position).normalized * keepDistanceToPlayer);
	}

	public bool IsValidPosition(Vector3 position)
	{
		bool isPositionOnNavMesh = NavMesh.SamplePosition(position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas);

		if (!isPositionOnNavMesh)
			return false;

		NavMeshPath path = new();
		bool isPositionReachable = NavMesh.CalculatePath(navMeshAgent.transform.position, position, NavMesh.AllAreas, path);

		if (!isPositionReachable)
			return false;

		bool isPathValidAndRelativelySimple = path.status == NavMeshPathStatus.PathComplete && path.corners.Length <= 5;
		return isPathValidAndRelativelySimple;
	}

	#endregion Public Methods

	#region Private Methods

	private void Awake()
	{
		navMeshAgent.avoidancePriority = navMeshAgent.transform.GetSiblingIndex() + 1;
	}

	private void Update()
	{
		RemainingDistance = navMeshAgent.remainingDistance;
		StoppingDistance = navMeshAgent.stoppingDistance;
		DistanceToPlayer = Vector3.Distance(navMeshAgent.transform.position, playerTransform.position);
		Velocity = navMeshAgent.velocity;
	}

	#endregion Private Methods
}