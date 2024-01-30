using DG.Tweening;
using StealthTD.Animation;
using StealthTD.Character;
using StealthTD.Extensions;
using StealthTD.Input;
using StealthTD.Interfaces;
using StealthTD.Weapons;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace StealthTD.Player.Weapons
{
	public class PlayerWeaponController : MonoBehaviour
	{
		#region Private Fields

		private const float mouseAimDeadzone = 0.5f;

		[Inject]
		private readonly Animator animator;

		[Inject]
		private readonly AnimationEventRelayer eventRelayer;

		[Inject]
		private readonly InputProvider inputProvider;

		[Inject]
		private readonly PlayerEquipManager equipManager;

		[Inject]
		private readonly CharacterBoneReferences bones;

		[SerializeField]
		private bool autoReload;

		#endregion Private Fields

		#region Public Events

		public event Action OnReloadStarted;
		public event Action OnReloadComplete;

		#endregion Public Events

		#region Public Properties

		[field: SerializeField, ReadOnly]
		public bool IsReloading { get; private set; }

		public Weapon CurrentWeapon { get; private set; }

		public bool HasWeapon => CurrentWeapon != null;

		#endregion Public Properties

		#region Public Methods

		public void UseCurrentWeapon(string returnToAnimationState = null)
		{
			if (CurrentWeapon == null)
				return;

			if (CurrentWeapon.IsCoolingDown || IsReloading)
				return;

			if (CurrentWeapon is ShootingWeapon shootingWeapon
				&& shootingWeapon.CurrentAmmo == 0
				&& autoReload)
			{
				ReloadCurrentWeapon(returnToAnimationState);
				return;
			}

			StopAllCoroutines();
			StartCoroutine(UseWeaponRoutine(returnToAnimationState));
		}

		public void ReloadCurrentWeapon(string returnToAnimationState = null)
		{
			if (CurrentWeapon == null)
				return;

			if (CurrentWeapon is not ShootingWeapon shootingWeapon
				|| shootingWeapon.CurrentAmmo == shootingWeapon.MaxAmmo)
				return;

			if (IsReloading)
				return;

			IsReloading = true;
			StopUsingCurrentWeapon();
			StopAllCoroutines();
			StartCoroutine(ReloadWeaponRoutine(returnToAnimationState));
		}

		public void StopUsingCurrentWeapon()
		{
			if (CurrentWeapon == null)
				return;

			CurrentWeapon.StopUsingWeapon();
		}

		public void HandleWeaponRotation()
		{
			if (CurrentWeapon == null)
				return;

			if (IsReloading)
				return;

			Quaternion targetRotation = Quaternion.identity;

			if (CurrentWeapon.IsClippingIntoWall())
				targetRotation = Quaternion.Euler(0, -90, 0);
			else if (CurrentWeapon is ShootingWeapon)
			{
				Vector3 mousePosition = inputProvider.GetMouseWorldPosition(CurrentWeapon.transform);
				Vector3 directionToMouse = mousePosition - CurrentWeapon.transform.position;
				float distanceToMouse = Vector3.Distance(CurrentWeapon.transform.position, mousePosition);

				if (distanceToMouse > mouseAimDeadzone)
				{
					Vector3 localForward = CurrentWeapon.transform.parent.InverseTransformDirection(directionToMouse);
					Vector3 worldUp = Vector3.up;
					Vector3 localUp = transform.parent.InverseTransformDirection(worldUp);
					targetRotation = Quaternion.LookRotation(localForward, localUp);
				}
			}

			CurrentWeapon.transform.localRotation = Quaternion.RotateTowards(CurrentWeapon.transform.localRotation, targetRotation, 500 * Time.fixedDeltaTime);
		}

		public void EquipWeapon(Weapon weapon)
		{
			StopAllCoroutines();
			weapon.SoundOwner = transform;
			weapon.SetImpactLayerMask(LayerMask.GetMask("Enemy", "Environment", "Ground"));
			weapon.Equip(transform, bones.RightHand);
			SubscribeWeaponAnimationEvents(weapon);
			animator.SetActiveAnimatorLayer(weapon.AnimatorLayer);
			CurrentWeapon = weapon;
		}

		public void UnequipWeapon(bool setAnimatorLayer)
		{
			if (CurrentWeapon == null)
				return;

			StopAllCoroutines();

			if (setAnimatorLayer)
				animator.SetActiveAnimatorLayer(AnimatorLayer.Unarmed);

			UnsubscribeWeaponAnimationEvents(CurrentWeapon);
			CurrentWeapon.Unequip();
			CurrentWeapon = null;
			IsReloading = false;
		}

		public void ResetWeaponRotation()
		{
			if (CurrentWeapon == null)
				return;

			DOTween.Kill("Player_ResetWeaponRotation");
			CurrentWeapon.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.2f).SetId("Player_ResetWeaponRotation");
		}

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			equipManager.OnEquipped += EquipManager_OnItemEquipped;
			equipManager.OnUnequipped += EquipManager_OnItemUnequipped;
		}

		private void OnDestroy()
		{
			equipManager.OnEquipped -= EquipManager_OnItemEquipped;
			equipManager.OnUnequipped -= EquipManager_OnItemUnequipped;
		}

		private void EquipManager_OnItemEquipped(IEquipable equipable)
		{
			if (equipable is Weapon weapon)
			{
				UnequipWeapon(false);
				EquipWeapon(weapon);
			}
		}

		private void EquipManager_OnItemUnequipped(IEquipable equipable)
		{
			if (equipable is Weapon weapon && weapon == CurrentWeapon)
			{
				UnequipWeapon(true);
				animator.PlayState("IdleMove", 0.25f, AnimatorLayer.UpperBody.ToInt());
			}
		}

		private void SubscribeWeaponAnimationEvents(Weapon weapon)
		{
			if (weapon is ShootingWeapon shootingWeapon)
				eventRelayer.Subscribe("OnReload", shootingWeapon.RefillAmmo);
			else if (weapon is MeleeWeapon meleeWeapon)
				eventRelayer.Subscribe("OnMeleeImpact", meleeWeapon.DealDamage);
		}

		private void UnsubscribeWeaponAnimationEvents(Weapon weapon)
		{
			if (weapon is ShootingWeapon shootingWeapon)
				eventRelayer.Unsubscribe("OnReload", shootingWeapon.RefillAmmo);
			else if (weapon is MeleeWeapon meleeWeapon)
				eventRelayer.Unsubscribe("OnMeleeImpact", meleeWeapon.DealDamage);
		}

		private IEnumerator UseWeaponRoutine(string returnToAnimationState)
		{
			CurrentWeapon.UseWeapon();
			if (!string.IsNullOrEmpty(returnToAnimationState))
				animator.PlayState("Attack", layer: AnimatorLayer.UpperBody.ToInt());

			if (CurrentWeapon is MeleeWeapon meleeWeapon)
			{
				yield return new WaitUntil(() => !meleeWeapon.IsCoolingDown);
				yield return new WaitForSeconds(0.2f);
			}

			if (!string.IsNullOrEmpty(returnToAnimationState))
				animator.PlayState(returnToAnimationState, layer: AnimatorLayer.UpperBody.ToInt());
		}

		private IEnumerator ReloadWeaponRoutine(string returnToAnimationState)
		{
			OnReloadStarted?.Invoke();
			if (!string.IsNullOrEmpty(returnToAnimationState))
				yield return animator.PlayAndWaitForState("Reload", normalizedTimeThreshold: 1, layer: AnimatorLayer.UpperBody.ToInt());

			if (!string.IsNullOrEmpty(returnToAnimationState))
				animator.PlayState(returnToAnimationState, layer: AnimatorLayer.UpperBody.ToInt(), fixedTransitionDuration: 0);

			IsReloading = false;
			OnReloadComplete?.Invoke();
		}

		#endregion Private Methods
	}
}