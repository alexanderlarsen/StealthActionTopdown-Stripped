using StealthTD.Animation;
using StealthTD.Audio;
using StealthTD.Enemy.AI;
using StealthTD.Enemy.Data;
using StealthTD.FSM;
using StealthTD.VFX;
using UnityEngine;
using Zenject;

namespace StealthTD.Enemy.States
{
	public abstract class EnemyState : State
	{
		#region Protected Fields

		[Inject(Id = "PlayerTransform")]
		protected readonly Transform playerTransform;

		[Inject]
		protected readonly EnemyMoveController moveController;

		[Inject]
		protected readonly VfxManager vfxManager;

		[Inject]
		protected readonly EnemyLocalData localData;

		[Inject]
		protected readonly EnemySharedData sharedData;

		[Inject]
		protected readonly Animator animator;

		[Inject]
		protected readonly AnimationEventRelayer eventRelayer;

		[Inject]
		protected readonly AudioManager audioManager;

		[Inject]
		protected readonly EnemyPatrolPath patrolPath;

		[Inject]
		protected readonly EnemyAgent enemy;

		[Inject]
		protected readonly EnemyWeaponController weaponController;

		[Inject]
		protected readonly EnemyVision vision;

		[Inject]
		protected EnemyAgent controller;

		#endregion Protected Fields
	}
}