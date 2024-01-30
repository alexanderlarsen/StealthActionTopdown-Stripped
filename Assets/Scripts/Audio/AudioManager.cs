using UnityEngine;

namespace StealthTD.Audio
{
	public class AudioManager : MonoBehaviour
	{
		#region Public Properties

		[field: SerializeField]
		public ImpactAudioHandler ImpactAudio { get; private set; }

		[field: SerializeField]
		public CharacterAudioHandler CharacterAudio { get; private set; }

		#endregion Public Properties
	}
}