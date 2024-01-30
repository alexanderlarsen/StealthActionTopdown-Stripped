using UnityEngine;

namespace StealthTD.Audio
{
	public class MenuAudioManager : MonoBehaviour
	{
		#region Private Fields

		[SerializeField]
		private AudioClip buttonPressAudio;

		[SerializeField]
		private AudioClip invalidButtonPressAudio;

		[SerializeField]
		private AudioClip onLeaveTitleScreenAudio;

		[SerializeField]
		private AudioClip levelCompleteAudio;

		[SerializeField]
		private AudioClip gameOverAudio;

		private Camera mainCamera;

		#endregion Private Fields

		#region Public Methods

		public void PlayButtonPressAudio() => PlayAudio(buttonPressAudio);

		public void PlayInvalidButtonPressAudio() => PlayAudio(invalidButtonPressAudio);

		public void PlayLeaveTitleScreenAudio() => PlayAudio(onLeaveTitleScreenAudio);

		public void PlayGameOverAudio() => PlayAudio(gameOverAudio);

		public void PlayLevelCompleteAudio() => PlayAudio(levelCompleteAudio);

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			mainCamera = Camera.main;
		}

		private void PlayAudio(AudioClip audioClip)
		{
			AudioSource.PlayClipAtPoint(audioClip, mainCamera.transform.position);
		}

		#endregion Private Methods
	}
}