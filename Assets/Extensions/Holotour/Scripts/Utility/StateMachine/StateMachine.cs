using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This implements a generic state machine. This implementation can be used to represent all sorts of 
/// states and should be the preffered way to manage state machines.
/// </summary>
public class StateMachine<T, E> where T : IState<T, E>
{
	private IState<T, E> currentState;
    public IState<T, E> CurrentState
    { get { return currentState; } }

    public event System.Action<T, T> OnStateChanged;

	public StateMachine(IState<T, E> initState){
		SetState (initState);
	}

	private void SetState(IState<T, E> newState)
	{
		if (currentState != newState)
		{
			T oldState = (T) currentState;
			currentState = newState;
			if (OnStateChanged != null) {
				OnStateChanged.Invoke ((T) oldState, (T) newState);
			}
			currentState.OnStateEnter ();
			Debug.Log ("State changed to " + newState.GetType ().ToString ());
		}
	}

	/// <summary>
	/// Delegate the MonoBehaviour Update calls to this method.
	/// </summary>
	public void Update()
	{
		if (currentState != null) {
			SetState (currentState.Update ());
		}
	}

	/// <summary>
	/// Call this method to trigger transitions on the state machine graph.
	/// </summary>
	/// <param name="e">E.</param>
	public void PostEvent(E e) {
		if (currentState != null) {
			SetState (currentState.HandleEvent (e));
		}
	}

	public bool IsCurrentState<T>() {
		return currentState.GetType() == typeof(T);
	}

}