using System;
using UnityEngine;
using Zenject;

namespace StealthTD.GameLogic
{
	public class GoalMarkerController : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly ObjectiveManager objectiveManager;

		[SerializeField, ReadOnly]
		private bool hasCompletedAllObjectives;

		[SerializeField]
		private ColorSettings meshColors;

		[SerializeField]
		private ColorSettings lightColors;

		[SerializeField]
		private float rotationSpeed = 3;

		[SerializeField]
		private MeshRenderer meshRenderer;

		[SerializeField]
		private Light spotLight;

		[SerializeField]
		private GameObject particles;

		private bool reachedGoal;

		#endregion Private Fields

		#region Public Events

		public event Action OnReachedGoal;

		#endregion Public Events

		#region Private Methods

		private void Awake()
		{
			meshRenderer.material.color = meshColors.onStartColor;
			spotLight.color = lightColors.onStartColor;
			particles.SetActive(false);

			objectiveManager.OnAllObjectivesCompleted += ObjectiveManager_OnAllObjectivesCompleted;
		}

		private void OnDestroy()
		{
			objectiveManager.OnAllObjectivesCompleted -= ObjectiveManager_OnAllObjectivesCompleted;
		}

		private void Update()
		{
			transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!hasCompletedAllObjectives || reachedGoal)
				return;

			reachedGoal = true;
			OnReachedGoal?.Invoke();
		}

		private void ObjectiveManager_OnAllObjectivesCompleted()
		{
			hasCompletedAllObjectives = true;
			meshRenderer.material.color = meshColors.onCompleteColor;
			spotLight.color = lightColors.onCompleteColor;
			particles.SetActive(true);
		}

		#endregion Private Methods

		#region Private Classes

		[Serializable]
		private class ColorSettings
		{
			#region Public Fields

			public Color onStartColor;
			public Color onCompleteColor;

			#endregion Public Fields
		}

		#endregion Private Classes
	}
}