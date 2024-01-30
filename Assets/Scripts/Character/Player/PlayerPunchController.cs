using StealthTD.Animation;
using StealthTD.Audio;
using StealthTD.Character;
using StealthTD.Extensions;
using StealthTD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace StealthTD.Player
{
	public class PlayerPunchController : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly AnimationEventRelayer eventRelayer;

		[Inject]
		private readonly Animator animator;

		[Inject]
		private readonly CharacterBoneReferences bones;

		[Inject]
		private readonly AudioManager audioManager;

		[SerializeField]
		private LayerMask punchableLayerMask;

		[SerializeField]
		private int attackCount;

		[SerializeField]
		private float attackTimer;

		private Action onCompleteCallback;
		private bool hitSomething;
		private bool isAttackRunning;
		private bool wasAttackPressedSinceLastAnimation;
		private float continueAttackTimer;

		#endregion Private Fields

		#region Public Methods

		public void Attack(Action onComplete = null)
		{
			onCompleteCallback = onComplete;
			attackTimer = 0.4f;
			wasAttackPressedSinceLastAnimation = true;

			if (!isAttackRunning)
				StartCoroutine(ExecuteAttack());
		}

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			eventRelayer.Subscribe("OnLeftPunchImpact", AnimationEvent_OnLeftPunchImpact);
			eventRelayer.Subscribe("OnRightPunchImpact", AnimationEvent_OnRightPunchImpact);
		}

		private void OnDestroy()
		{
			eventRelayer.Unsubscribe("OnLeftPunchImpact", AnimationEvent_OnLeftPunchImpact);
			eventRelayer.Unsubscribe("OnRightPunchImpact", AnimationEvent_OnRightPunchImpact);
		}

		private void Update()
		{
			if (!isAttackRunning)
				return;

			if (attackTimer > 0)
				attackTimer -= Time.deltaTime;

			if (continueAttackTimer > 0)
				continueAttackTimer -= Time.deltaTime;
		}

		private IEnumerator ExecuteAttack()
		{
			isAttackRunning = true;

			while (attackTimer > 0)
			{
				string stateName = attackCount % 2 == 0 ? "RightPunch" : "LeftPunch";
				animator.PlayStateInBothLayers(stateName, 0.1f);
				continueAttackTimer = 0.4f;

				yield return new WaitUntil(() => hitSomething || continueAttackTimer <= 0);
				hitSomething = false;

				if (wasAttackPressedSinceLastAnimation)
				{
					attackCount++;
					wasAttackPressedSinceLastAnimation = false;
				}
			}

			attackCount = 0;
			attackTimer = 0;
			animator.PlayStateInBothLayers("IdleMove", 0.1f);
			onCompleteCallback?.Invoke();
			isAttackRunning = false;
		}

		private void HandlePunchImpact(Transform handTransform)
		{
			Collider[] colliderResults = new Collider[10];

			int numColliders = Physics.OverlapSphereNonAlloc(handTransform.position, 0.2f, colliderResults, punchableLayerMask);

			if (numColliders > 0)
				audioManager.CharacterAudio.PlayPunchAudio(handTransform.position, transform);

			HashSet<GameObject> uniqueGameObjects = new();

			for (int i = 0; i < numColliders; i++)
			{
				Collider collider = colliderResults[i];

				if (uniqueGameObjects.Contains(collider.gameObject))
					continue;

				if (!collider.TryGetComponent(out IPunchable punchable))
					continue;

				hitSomething = true;
				punchable.TakePunch();
				uniqueGameObjects.Add(collider.gameObject);
			}
		}

		private void AnimationEvent_OnRightPunchImpact()
		{
			HandlePunchImpact(bones.RightHand);
		}

		private void AnimationEvent_OnLeftPunchImpact()
		{
			HandlePunchImpact(bones.LeftHand);
		}

		#endregion Private Methods
	}
}