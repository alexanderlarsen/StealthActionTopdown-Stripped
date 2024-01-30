using StealthTD.Interfaces;
using StealthTD.Player;
using StealthTD.Player.Data;
using StealthTD.Player.Weapons;
using StealthTD.Weapons;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Zenject;

namespace StealthTD.UI.Player
{
	public class PlayerUIModel : IInitializable, IDisposable, INotifyPropertyChanged
	{
		#region Private Fields

		[Inject]
		private readonly PlayerWeaponController weaponController;

		[Inject]
		private readonly PlayerEquipManager equipManager;

		[Inject]
		private readonly PlayerLocalData localData;

		private bool hasShootingWeapon;
		private bool shouldReload;
		private IEquipable currentEquipable;
		private int currentAmmo;
		private int currentHealth;
		private int maxAmmo;
		private ShootingWeapon shootingWeapon;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public bool HasShootingWeapon
		{
			get => hasShootingWeapon;
			set
			{
				if (hasShootingWeapon == value)
					return;

				hasShootingWeapon = value;
				InvokePropertyChanged();
			}
		}

		public bool ShouldReload
		{
			get => shouldReload;
			set
			{
				if (shouldReload == value)
					return;

				shouldReload = value;
				InvokePropertyChanged();
			}
		}

		public int CurrentAmmo
		{
			get => currentAmmo;
			set
			{
				if (currentAmmo == value)
					return;

				currentAmmo = value;
				InvokePropertyChanged();
			}
		}

		public int MaxAmmo
		{
			get => maxAmmo;
			set
			{
				if (maxAmmo == value)
					return;

				maxAmmo = value;
				InvokePropertyChanged();
			}
		}

		public IEquipable CurrentEquipable
		{
			get => currentEquipable;
			set
			{
				if (currentEquipable == value)
					return;

				currentEquipable = value;
				InvokePropertyChanged();
			}
		}

		public int CurrentHealth
		{
			get => currentHealth;
			set
			{
				if (currentHealth == value)
					return;

				currentHealth = value;
				InvokePropertyChanged();
			}
		}

		#endregion Public Properties

		#region Public Methods

		public void Initialize()
		{
			localData.PropertyChanged += LocalData_PropertyChanged;
			equipManager.OnEquipped += EquipManager_OnItemEquipped;
			equipManager.OnUnequipped += EquipManager_OnItemUnequipped;
			equipManager.OnEquipableInRange += EquipManager_OnItemInEquipRange;
			weaponController.OnReloadStarted += WeaponController_OnReloadStarted;
			weaponController.OnReloadComplete += WeaponController_OnReloadComplete;
		}

		public void Dispose()
		{
			localData.PropertyChanged -= LocalData_PropertyChanged;
			equipManager.OnEquipped -= EquipManager_OnItemEquipped;
			equipManager.OnUnequipped -= EquipManager_OnItemUnequipped;
			equipManager.OnEquipableInRange -= EquipManager_OnItemInEquipRange;
			weaponController.OnReloadStarted -= WeaponController_OnReloadStarted;
			weaponController.OnReloadComplete -= WeaponController_OnReloadComplete;
		}

		#endregion Public Methods

		#region Private Methods

		private void LocalData_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Health")
				CurrentHealth = localData.Health;
		}

		private void Equipable_OnEquipped()
		{
			CurrentEquipable = null;
		}

		private void EquipManager_OnItemEquipped(IEquipable equipable)
		{
			equipable.OnEquipped += Equipable_OnEquipped;

			if (equipable is ShootingWeapon shootingWeapon)
			{
				shootingWeapon.OnCurrentAmmoChanged += ShootingWeapon_OnCurrentAmmoChanged;
				HasShootingWeapon = true;
				CurrentAmmo = shootingWeapon.CurrentAmmo;
				MaxAmmo = shootingWeapon.MaxAmmo;
				ShouldReload = (float)CurrentAmmo / MaxAmmo <= 0.2f;
				this.shootingWeapon = shootingWeapon;
			}
			else
			{
				HasShootingWeapon = false;
				ShouldReload = false;
			}
		}

		private void EquipManager_OnItemUnequipped(IEquipable equipable)
		{
			equipable.OnEquipped -= Equipable_OnEquipped;

			if (equipable is ShootingWeapon shootingWeapon)
			{
				HasShootingWeapon = false;
				ShouldReload = false;
				shootingWeapon.OnCurrentAmmoChanged -= ShootingWeapon_OnCurrentAmmoChanged;
				this.shootingWeapon = null;
			}
		}

		private void EquipManager_OnItemInEquipRange(IEquipable equipable)
		{
			CurrentEquipable = equipable;
		}

		private void ShootingWeapon_OnCurrentAmmoChanged()
		{
			CurrentAmmo = shootingWeapon.CurrentAmmo;
			ShouldReload = (float)CurrentAmmo / MaxAmmo <= 0.2f;
		}

		private void WeaponController_OnReloadStarted()
		{
			ShouldReload = false;
		}

		private void WeaponController_OnReloadComplete()
		{
			CurrentAmmo = shootingWeapon.CurrentAmmo;
			MaxAmmo = shootingWeapon.MaxAmmo;
			ShouldReload = false;
		}

		private void InvokePropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion Private Methods
	}
}