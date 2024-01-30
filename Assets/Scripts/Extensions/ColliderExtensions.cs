using StealthTD.Interfaces;
using UnityEngine;

namespace StealthTD.Extensions
{
	public static class ColliderExtensions
	{
		#region Public Methods

		public static void FindDamagablesAndDealDamage(this Collider[] colliders, int damage)
		{
			foreach (Collider collider in colliders)
				if (collider.TryGetComponent(out IDamagable damagable))
					damagable.TakeDamage(damage);
		}

		#endregion Public Methods
	}
}