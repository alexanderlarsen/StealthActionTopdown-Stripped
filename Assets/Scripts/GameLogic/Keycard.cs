using System;
using UnityEngine;

namespace StealthTD.GameLogic
{
	public class Keycard : MonoBehaviour
	{
		#region Private Fields

		[SerializeField]
		private AudioClip pickUpAudio;

		#endregion Private Fields

		#region Public Events

		public event Action OnPickUp;

		#endregion Public Events

		#region Private Methods

		private void Update()
		{
			transform.Rotate(Vector3.up, 50f * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				OnPickUp?.Invoke();
				AudioSource.PlayClipAtPoint(pickUpAudio, Camera.main.transform.position);
				Destroy(gameObject);
			}
		}

		#endregion Private Methods
	}
}