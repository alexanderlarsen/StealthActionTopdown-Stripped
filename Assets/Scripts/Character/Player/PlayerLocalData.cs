using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace StealthTD.Player.Data
{
	public class PlayerLocalData : MonoBehaviour, INotifyPropertyChanged
	{
		#region Private Fields

		[SerializeField, ReadOnly]
		private int health = 100;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public int Health
		{
			get => health;
			set
			{
				if (health == value)
					return;

				health = value;
				InvokePropertyChanged();
			}
		}

		#endregion Public Properties

		#region Private Methods

		private void InvokePropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion Private Methods
	}
}