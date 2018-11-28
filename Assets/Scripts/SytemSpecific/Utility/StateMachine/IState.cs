using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for state machine states.
/// </summary>
public interface IState<TState, TEvent> where TState : IState<TState, TEvent>
{
	/// <summary>
	/// Gets called when state machine events are posted opn the state machine.
	/// </summary>
	/// <returns>The state, following in the state machine graph as result of the handled event.</returns>
	/// <param name="e">E.</param>
	TState HandleEvent (TEvent e);

	/// <summary>
	/// Gets called then the update method on the statemachine is called.
	/// </summary>
	/// <returns>The state, following in the state machine graph as result of the update call.</returns>
	TState Update();

	/// <summary>
	/// Gets called once, when this states get entered.
	/// </summary>
	void OnStateEnter ();

	/// <summary>
	/// Determines whether this instance is of Type T .
	/// </summary>
	/// <returns><c>true</c> if this instance is ; otherwise, <c>false</c>.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	bool Is<T>();

}