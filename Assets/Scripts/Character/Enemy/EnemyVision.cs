using StealthTD.Character;
using StealthTD.Enemy.Data;
using StealthTD.Interfaces;
using StealthTD.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace StealthTD.Enemy.AI
{
	public class EnemyVision : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly EnemyLocalData localData;

		[Inject]
		private readonly CharacterBoneReferences bones;

		[SerializeField]
		private LayerMask detectableLayerMask;

		[SerializeField]
		private LayerMask obstacleLayerMask;

		[SerializeField]
		private float maxAngle = 70;

		[SerializeField]
		private float maxRadius = 10;

		[SerializeField]
		private float refreshInterval = 0.2f;

		private bool isRunning;
		private IVisionDetectable mostImportantDetectable;

		#endregion Private Fields

		#region Public Methods

		public bool CanSeeIncapacitatedEnemy(out EnemyAgent enemy)
		{
			if (mostImportantDetectable is not null and EnemyAgent enemyDetectable)
			{
				enemy = enemyDetectable;
				return true;
			}

			enemy = null;
			return false;
		}

		public void StartVision()
		{
			StopAllCoroutines();
			StartCoroutine(VisionRoutine());
			isRunning = true;
		}

		public void StopVision()
		{
			StopAllCoroutines();
			isRunning = false;
			mostImportantDetectable = null;
			localData.PlayerDetectedAmount = 0;
			localData.PlayerDetectionThreshold = 1;
		}

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			StartVision();
		}

		private void Update()
		{
			if (!isRunning)
				return;

			CalculatePlayerDetection();
		}

		private IEnumerator VisionRoutine()
		{
			YieldInstruction waitInstruction = new WaitForSeconds(refreshInterval);

			while (true)
			{
				mostImportantDetectable = GetMostImportantDetectable();
				yield return waitInstruction;
			}
		}

		private IVisionDetectable GetMostImportantDetectable()
		{
			List<IVisionDetectable> incapacitatedEnemies = new();

			foreach (Collider collider in Physics.OverlapSphere(bones.Eyes.position, maxRadius, detectableLayerMask))
			{
				if (collider.TryGetComponent(out IVisionDetectable detectable)
					&& detectable.IsDetectable
					&& IsTargetVisible(detectable.Transform))
				{
					switch (detectable)
					{
						case PlayerAgent: return detectable;
						case EnemyAgent: incapacitatedEnemies.Add(detectable); break;
					}
				}
			}

			if (incapacitatedEnemies.Count == 0)
				return null;

			if (incapacitatedEnemies.Count == 1)
				return incapacitatedEnemies.First();

			return incapacitatedEnemies.OrderBy(enemy => Vector3.Distance(transform.position, enemy.Transform.position)).First();
		}

		private bool IsPlayerVisible(out PlayerAgent player)
		{
			if (mostImportantDetectable is not null and PlayerAgent playerDetectable)
			{
				player = playerDetectable;
				return true;
			}

			player = null;
			return false;
		}

		private bool IsTargetVisible(Transform targetTransform)
		{
			Vector3 directionToTarget = (targetTransform.position - bones.Eyes.position).normalized;
			bool isTargetWithinFieldOfView = Vector3.Angle(bones.Eyes.forward, directionToTarget) <= maxAngle;
			bool hasLineOfSightToTarget = !Physics.Linecast(bones.Eyes.position, targetTransform.position, obstacleLayerMask);
			return isTargetWithinFieldOfView && hasLineOfSightToTarget;
		}

		private void CalculatePlayerDetection()
		{
			if (localData.Health <= 0)
				return;

			float playerDetectedAmount = localData.PlayerDetectedAmount;
			float playerDetectionThreshold = localData.PlayerDetectionThreshold;

			if (IsPlayerVisible(out PlayerAgent player))
			{
				playerDetectedAmount += Time.deltaTime;
				playerDetectionThreshold = Vector3.Distance(transform.position, player.transform.position) / 10f;
			}
			else
			{
				playerDetectedAmount -= Time.deltaTime;
			}

			localData.PlayerDetectedAmount = Mathf.Clamp(playerDetectedAmount, 0, localData.PlayerDetectionThreshold + 0.1f);
			localData.PlayerDetectionThreshold = playerDetectionThreshold;
		}

		#endregion Private Methods
	}
}