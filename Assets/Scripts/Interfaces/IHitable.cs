using System;
using UnityEngine;

namespace StealthTD.Interfaces
{
	public interface IHitable
	{
		#region Public Events

		public event Action<HitEventArgs> OnHit;

		#endregion Public Events

		#region Public Properties


		#endregion Public Properties

		#region Public Methods

		void Hit(Vector3 hitPosition, Vector3 hitNormal);

		#endregion Public Methods

		#region Public Classes

		public class HitEventArgs
		{
			#region Public Fields

			public readonly Vector3 hitPosition;
			public readonly Vector3 hitNormal;

			#endregion Public Fields

			#region Public Constructors

			public HitEventArgs(Vector3 hitPosition, Vector3 hitNormal)
			{
				this.hitPosition = hitPosition;
				this.hitNormal = hitNormal;
			}

			#endregion Public Constructors
		}

		#endregion Public Classes
	}
}