using DG.Tweening;
using StealthTD.Animation;
using StealthTD.Character;
using StealthTD.Extensions;
using StealthTD.Weapons;
using System.Collections;
using UnityEngine;
using Zenject;

namespace StealthTD.Enemy.AI
{
	public class EnemyWeaponController : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly Animator animator;

		[Inject]
		private readonly AnimationEventRelayer eventRelayer;

		[Inject]
		private readonly CharacterBoneReferences bones;

		[SerializeField]
		private Weapon startingWeapon;

		#endregion Private Fields

		#region Public Properties

		[field: SerializeField, ReadOnly]
		public bool IsReloading { get; private set; }

		public Weapon CurrentWeapon { get; private set; }

		public bool ShouldReload => CurrentWeapon != null
			&& !IsReloading
			&& CurrentWeapon is ShootingWeapon shootingWeapon
			&& shootingWeapon.CurrentAmmo == 0;

		#endregion Public Properties

		#region Public Methods

		public void UseCurrentWeapon()
		{
			if (CurrentWeapon == null || IsReloading)
				return;

			CurrentWeapon.UseWeapon();
		}

		public void ReloadCurrentWeapon(string returnToAnimationState)
		{
			if (CurrentWeapon == null
				|| IsReloading
				|| CurrentWeapon is not ShootingWeapon shootingWeapon
				|| shootingWeapon.CurrentAmmo == shootingWeapon.MaxAmmo)
				return;

			StopAllCoroutines();
			StartCoroutine(ReloadWeaponRoutine(returnToAnimationState));
		}

		public void CancelReload()
		{
			if (CurrentWeapon == null
				|| CurrentWeapon is not ShootingWeapon shootingWeapon
				|| !IsReloading)
				return;

			StopAllCoroutines();
			IsReloading = false;
		}

		public void StopUsingCurrentWeapon()
		{
			if (CurrentWeapon == null)
				return;

			CurrentWeapon.StopUsingWeapon();
		}

		public void HandleWeaponRotation(bool isTargetVisible, Vector3 targetPosition)
		{
			if (CurrentWeapon == null)
				return;

			Quaternion targetRotation = Quaternion.identity;

			if (!IsReloading && CurrentWeapon.IsClippingIntoWall())
			{
				targetRotation = Quaternion.Euler(0, -90, 0);
			}
			else if (!IsReloading && isTargetVisible && CurrentWeapon is ShootingWeapon)
			{
				targetPosition += Vector3.up * 0.5f;
				Vector3 directionToPlayer = targetPosition - CurrentWeapon.transform.position;
				float distanceToTarget = Vector3.Distance(CurrentWeapon.transform.position, targetPosition);

				if (distanceToTarget > 0.1f)
				{
					Vector3 localForward = CurrentWeapon.transform.parent.InverseTransformDirection(directionToPlayer);
					Vector3 worldUp = Vector3.up;
					Vector3 localUp = transform.parent.InverseTransformDirection(worldUp);
					targetRotation = Quaternion.LookRotation(localForward, localUp);
				}
			}

			CurrentWeapon.transform.localRotation = Quaternion.RotateTowards(CurrentWeapon.transform.localRotation, targetRotation, 500 * Time.fixedDeltaTime);
		}

		public void ResetWeaponRotation()
		{
			if (CurrentWeapon == null)
				return;

			string id = $"{gameObject.GetInstanceID()}_Enemy_ResetWeaponRotation";
			DOTween.Kill(id);
			CurrentWeapon.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.2f).SetId(id);
		}

		public void EquipWeapon(Weapon weapon)
		{
			StopAllCoroutines();
			weapon.SoundOwner = transform;
			weapon.SetImpactLayerMask(LayerMask.GetMask("Player", "Environment", "Ground"));
			weapon.Equip(transform, bones.RightHand);
			SubscribeWeaponAnimationEvents(weapon);
			animator.SetActiveAnimatorLayer(weapon.AnimatorLayer);
			CurrentWeapon = weapon;
		}

		public void UnequipWeapon()
		{
			if (CurrentWeapon == null)
				return;

			StopAllCoroutines();
			UnsubscribeWeaponAnimationEvents(CurrentWeapon);
			CurrentWeapon.Unequip();
			CurrentWeapon = null;
			IsReloading = false;
		}

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			EquipWeapon(startingWeapon);
		}

		private IEnumerator ReloadWeaponRoutine(string returnToAnimationState)
		{
			IsReloading = true;
			yield return animator.PlayAndWaitForState("Reload", normalizedTimeThreshold: 0.9f, layer: AnimatorLayer.UpperBody.ToInt());
			animator.PlayState(returnToAnimationState, layer: AnimatorLayer.UpperBody.ToInt());
			IsReloading = false;
		}

		private void SubscribeWeaponAnimationEvents(Weapon weapon)
		{
			if (weapon is ShootingWeapon shootingWeapon)
				eventRelayer.Subscribe("OnReload", shootingWeapon.RefillAmmo);
		}

		private void UnsubscribeWeaponAnimationEvents(Weapon weapon)
		{
			if (weapon is ShootingWeapon shootingWeapon)
				eventRelayer.Unsubscribe("OnReload", shootingWeapon.RefillAmmo);
		}

		#endregion Private Methods
	}
}