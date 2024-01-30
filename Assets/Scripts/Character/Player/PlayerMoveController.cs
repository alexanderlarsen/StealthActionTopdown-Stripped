using StealthTD.CameraControllers;
using StealthTD.GameLogic;
using StealthTD.Input;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace StealthTD.Player.Movement
{
	public class PlayerMoveController : MonoBehaviour
	{
		#region Private Fields

		private const float gravityConstant = -9.81f;

		[Inject]
		private readonly CharacterController characterController;

		[Inject]
		private readonly InputProvider inputProvider;

		[Inject]
		private readonly TrackPlayerCameraController cameraController;

		[Inject]
		private readonly GameStateManager gameStateManager;

		private bool isCameraInitializationComplete;

		#endregion Private Fields

		#region Public Properties

		public bool IsGrounded => characterController.isGrounded;
		public Vector3 Velocity => characterController.velocity;

		[field: SerializeField, ReadOnly]
		public float TimeNotGrounded { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void Move(Vector2 inputDirection, float moveSpeed)
		{
			if (gameStateManager.IsGameOver || !isCameraInitializationComplete)
				return;

			Vector3 velocity = new(inputDirection.x * moveSpeed, gravityConstant, inputDirection.y * moveSpeed);
			characterController.Move(Time.deltaTime * velocity);

			if (!IsMovingTowardsGround(inputDirection))
				ClampPositionWithinNavMesh();
		}

		public void Rotate(float rotationSpeed)
		{
			if (gameStateManager.IsGameOver)
				return;

			Vector3 aimTargetPosition = inputProvider.GetMouseWorldPosition(transform);
			Quaternion targetRotation = Quaternion.LookRotation(aimTargetPosition - transform.position);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
		}

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			cameraController.OnInitializationComplete += TrackPlayerCameraController_OnInitializationComplete;
		}

		private void OnDestroy()
		{
			cameraController.OnInitializationComplete -= TrackPlayerCameraController_OnInitializationComplete;
		}

		private void TrackPlayerCameraController_OnInitializationComplete()
		{
			isCameraInitializationComplete = true;
		}

		private void Update()
		{
			if (characterController.isGrounded)
				TimeNotGrounded = 0;
			else
				TimeNotGrounded += Time.deltaTime;
		}

		private void ClampPositionWithinNavMesh()
		{
			if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1, NavMesh.AllAreas))
			{
				Vector3 position = hit.position;
				position.y = transform.position.y;
				transform.position = position;
			}
		}

		private bool IsMovingTowardsGround(Vector2 moveDirection)
		{
			float horizontalOffsetScaler = 0.5f;
			Vector3 origin = transform.position;
			Vector3 offset = new(moveDirection.x * horizontalOffsetScaler, 2, moveDirection.y * horizontalOffsetScaler);
			bool hasFoundGround = Physics.Raycast(origin + offset, Vector3.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
			return hasFoundGround;
		}

		#endregion Private Methods
	}
}