using System;
using UnityEngine;

namespace StealthTD.Interfaces
{
	public interface IEquipable
	{
		#region Public Events

		event Action OnEquipped;

		#endregion Public Events

		#region Public Properties

		bool IsEquipped { get; }
		Transform EquipableTransform { get; }
		string EquipableName { get; }

		#endregion Public Properties

		#region Public Methods

		void Equip(Transform characterRoot, Transform handTransform);

		void Unequip();

		#endregion Public Methods
	}
}