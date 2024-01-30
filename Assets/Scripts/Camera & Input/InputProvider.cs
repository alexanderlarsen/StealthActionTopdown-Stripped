using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace StealthTD.Input
{
	public class InputProvider
	{
		#region Private Fields

		[Inject]
		private readonly Camera mainCamera;

		private readonly InputAction lookAction;
		private readonly InputAction moveAction;

		#endregion Private Fields

		#region Public Constructors

		public InputProvider(PlayerInput playerInput)
		{
			Attack = playerInput.actions.FindAction("Attack", true);
			DropItem = playerInput.actions.FindAction("DropWeapon", true);
			Interact = playerInput.actions.FindAction("Interact", true);
			lookAction = playerInput.actions.FindAction("Look", true);
			moveAction = playerInput.actions.FindAction("Move", true);
			Reload = playerInput.actions.FindAction("ReloadWeapon", true);
			Sneak = playerInput.actions.FindAction("Sneak", true);
			PauseGame = playerInput.actions.FindAction("PauseGame", true);
		}

		#endregion Public Constructors

		#region Public Properties

		public InputAction Attack { get; private set; }
		public InputAction DropItem { get; private set; }
		public InputAction Interact { get; private set; }
		public InputAction Reload { get; private set; }
		public InputAction Sneak { get; private set; }
		public InputAction PauseGame { get; private set; }
		public Vector2 LookPosition => lookAction.ReadValue<Vector2>();
		public Vector2 MoveDirection => moveAction.ReadValue<Vector2>();

		#endregion Public Properties

		#region Public Methods

		public Vector3 GetMouseWorldPosition(Transform measureDistanceFromCameraToTransform)
		{
			Vector2 mousePos = LookPosition;
			float distanceToCamera = Mathf.Abs(mainCamera.transform.position.y - measureDistanceFromCameraToTransform.position.y);
			return mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distanceToCamera));
		}

		#endregion Public Methods
	}
}