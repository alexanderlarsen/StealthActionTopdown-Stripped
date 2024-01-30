using StealthTD.Animation;
using StealthTD.Audio;
using StealthTD.FSM;
using StealthTD.Input;
using StealthTD.Player.Movement;
using StealthTD.Player.Weapons;
using StealthTD.VFX;
using UnityEngine;
using Zenject;

namespace StealthTD.Player.States
{
	public abstract class PlayerState : State
	{
		#region Protected Fields

		[Inject]
		protected readonly PlayerWeaponController weaponController;

		[Inject]
		protected readonly PlayerPunchController punchController;

		[Inject]
		protected readonly Animator animator;

		[Inject]
		protected readonly AnimationEventRelayer eventRelayer;

		[Inject]
		protected readonly InputProvider input;

		[Inject]
		protected readonly PlayerEquipManager equipManager;

		[Inject]
		protected readonly PlayerMoveController moveController;

		[Inject]
		protected readonly VfxManager vfxManager;

		[Inject]
		protected readonly AudioManager audioManager;

		#endregion Protected Fields
	}
}