using StealthTD.Input;
using StealthTD.Player;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace StealthTD.CameraControllers
{
	public class TrackPlayerCameraController : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly InputProvider inputProvider;

		[Inject]
		private readonly PlayerAgent player;

		private readonly float directionScaler = 0.3f;
		private readonly float smoothTime = 0.4f;
		private readonly float verticalOffset = 13;

		private bool isInitializationComplete;
		private Vector3 velocity = Vector3.zero;

		#endregion Private Fields

		#region Public Events

		public event Action OnInitializationComplete;

		#endregion Public Events

		#region Private Methods

		private IEnumerator Start()
		{
			while (!Mathf.Approximately(transform.position.y, player.transform.position.y + verticalOffset))
			{
				transform.position = Vector3.MoveTowards(transform.position, player.transform.position + Vector3.up * verticalOffset, 15 * Time.deltaTime);
				yield return new WaitForEndOfFrame();
			}

			isInitializationComplete = true;
			OnInitializationComplete?.Invoke();
		}

		private void LateUpdate()
		{
			if (!isInitializationComplete)
				return;

			TrackTarget();
		}

		private void TrackTarget()
		{
			Vector3 targetPosition = player.IsDead ? player.transform.position : GetPointAlongAimAxis();
			targetPosition.y += verticalOffset;
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		}

		private Vector3 GetPointAlongAimAxis()
		{
			Vector3 mousePosition = inputProvider.GetMouseWorldPosition(player.transform);
			Vector3 direction = mousePosition - player.transform.position;
			return player.transform.position + (direction * directionScaler);
		}

		#endregion Private Methods
	}
}