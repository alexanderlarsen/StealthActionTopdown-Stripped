using StealthTD.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StealthTD.Animation
{
	public class AnimationEventRelayer : MonoBehaviour
	{
		#region Private Fields

		private readonly Dictionary<string, Action> eventActions = new Dictionary<string, Action>();

		#endregion Private Fields

		#region Public Methods

		public void TriggerAnimationEvent(string eventName)
		{
			if (eventActions.TryGetValue(eventName, out Action action))
				action?.Invoke();
		}

		public void Subscribe(string eventName, Action callback)
		{
			if (eventActions.ContainsKey(eventName))
				eventActions[eventName] += callback;
			else
				eventActions[eventName] = callback;
		}

		public void Unsubscribe(string eventName, Action callback)
		{
			if (eventActions.ContainsKey(eventName))
				eventActions[eventName] -= callback;
		}

		#endregion Public Methods
	}
}