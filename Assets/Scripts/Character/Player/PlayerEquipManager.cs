using StealthTD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StealthTD.Player
{
	public class PlayerEquipManager : MonoBehaviour
	{
		#region Public Fields

		public string closestItem;

		#endregion Public Fields

		#region Private Fields

		private readonly HashSet<IEquipable> inRange = new();
		private IEquipable currentlyEquipped;
		private IEquipable closest;

		#endregion Private Fields

		#region Public Events

		public event Action<IEquipable> OnEquipped;
		public event Action<IEquipable> OnUnequipped;
		public event Action<IEquipable> OnEquipableInRange;

		#endregion Public Events

		#region Public Methods

		public void EquipClosestItem()
		{
			if (inRange.Count == 0
				|| closest == null) return;

			currentlyEquipped = closest;
			inRange.Remove(currentlyEquipped);
			OnEquipped?.Invoke(currentlyEquipped);
		}

		public void UnequipCurrentItem()
		{
			if (currentlyEquipped == null) return;

			OnUnequipped?.Invoke(currentlyEquipped);
			currentlyEquipped = null;
		}

		#endregion Public Methods

		#region Private Methods

		private void FixedUpdate()
		{
			IEquipable currentClosest = GetClosestEquipable();

			if (closest != currentClosest)
			{
				closest = currentClosest;
				OnEquipableInRange?.Invoke(closest);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent(out IEquipable equipable)
				&& !equipable.IsEquipped
				&& !inRange.Contains(equipable))
				inRange.Add(equipable);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IEquipable equipable)
				&& inRange.Contains(equipable))
				inRange.Remove(equipable);
		}

		private IEquipable GetClosestEquipable()
		{
			if (inRange.Count == 0)
				return null;
			else if (inRange.Count == 1)
				return inRange.First();

			return inRange
				.OrderBy(equipable => Vector3.Distance(transform.position, equipable.EquipableTransform.position))
				.First();
		}

		#endregion Private Methods
	}
}