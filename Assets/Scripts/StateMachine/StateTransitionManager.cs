using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace StealthTD.FSM
{
	public abstract class StateTransitionManager<BaseState> : MonoBehaviour
		where BaseState : State
	{
		#region Private Fields

		[Inject]
		private readonly StateMachine stateMachine;

		private List<IStateTransitions> stateTransitions = new();

		#endregion Private Fields

		#region Protected Methods

		protected abstract BaseState GetInitialState();

		protected abstract List<IStateTransitions> DefineStateTransitions();

		protected virtual void Start()
		{
			stateTransitions = DefineStateTransitions();
			stateMachine.ChangeState(GetInitialState());
		}

		protected virtual void OnDestroy()
		{
			foreach (IStateTransitions transition in stateTransitions)
				if (transition is IDisposable disposable)
					disposable?.Dispose();
		}

		protected virtual void Update()
		{
			if (stateMachine.CurrentState == null)
				return;

			foreach (IStateTransitions transition in stateTransitions)
			{
				if (transition.ShouldTransition(out State nextState))
				{
					stateMachine.ChangeState(nextState);
					OnStateChange(nextState);
					break;
				}
			}
		}

		protected virtual void OnStateChange(State nextState)
		{
		}

		#endregion Protected Methods
	}
}