using UnityEngine;
using Zenject;

namespace StealthTD.UI
{
	public class FollowWorldSpaceObject : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly Camera mainCamera;

		[SerializeField]
		private Transform targetTransform;

		[SerializeField]
		private Vector2 screenSpaceOffset;

		[SerializeField]
		private bool clampWithinScreenBounds;

		[SerializeField]
		private float padding = 10.0f;

		#endregion Private Fields

		#region Public Methods

		public void SetTargetTransform(Transform targetTransform)
		{
			this.targetTransform = targetTransform;
		}

		#endregion Public Methods

		#region Private Methods

		private void Update()
		{
			if (targetTransform == null)
				return;

			if (clampWithinScreenBounds)
				SetClampedPosition();
			else
				SetPosition();
		}

		private void SetPosition()
		{
			transform.position = mainCamera.WorldToScreenPoint(targetTransform.position) + (Vector3)screenSpaceOffset;
		}

		private void SetClampedPosition()
		{
			Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetTransform.position);

			float minX = padding;
			float maxX = Screen.width - padding;
			float minY = padding;
			float maxY = Screen.height - padding;

			bool isWithinBounds = screenPosition.x >= minX && screenPosition.x <= maxX
							   && screenPosition.y >= minY && screenPosition.y <= maxY;

			if (isWithinBounds)
				screenPosition += (Vector3)screenSpaceOffset;

			screenPosition.x = Mathf.Clamp(screenPosition.x, minX, maxX);
			screenPosition.y = Mathf.Clamp(screenPosition.y, minY, maxY);

			transform.position = screenPosition;
		}

		#endregion Private Methods
	}
}