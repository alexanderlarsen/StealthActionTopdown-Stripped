using StealthTD.GameLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace StealthTD.FSM
{
	public class StateMachine : MonoBehaviour
	{
		#region Protected Fields

		[Inject]
		protected readonly GameStateManager gameStateManager;

		#endregion Protected Fields

		#region Private Fields

		[SerializeField, ReadOnly]
		private string currentState;

#if UNITY_EDITOR
		[SerializeField]
		private List<string> stateHistoryDebug = new(); 
#endif

		#endregion Private Fields

		#region Public Events

		public event Action<State> OnStateChanged;

		#endregion Public Events

		#region Public Properties

		public State CurrentState { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void ChangeState(State newState)
		{
			if (CurrentState != null)
				CurrentState.enabled = false;

			CurrentState = newState;
			CurrentState.enabled = true;
			OnStateChanged?.Invoke(newState);

#if UNITY_EDITOR
			stateHistoryDebug.Add(newState.ToString());
#endif
		}

		#endregion Public Methods
	}
}