using UnityEngine;

namespace StealthTD.Weapons
{
	public class Silencer : MonoBehaviour
	{
		#region Public Properties

		[field: SerializeField]
		public AudioClip FireAudio { get; private set; }

		[field: SerializeField]
		public Transform MuzzleOut { get; private set; }

		#endregion Public Properties
	}
}