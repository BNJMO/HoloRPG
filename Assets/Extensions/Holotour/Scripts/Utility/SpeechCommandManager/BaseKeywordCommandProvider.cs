using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract keyword command provider. Can be subclassed for convenience.
/// </summary>
public abstract class AbstractKeywordCommandProvider : Singleton<AbstractKeywordCommandProvider>, IKeywordCommandProvider {

	private List<KeywordCommand> empty = new List<KeywordCommand>();

	public List<KeywordCommand> GetSpeechCommands()
	{
		if (IsProviderActive ()) {
			return GetListInternal ();
		} else {
			return empty;
		}
	}

	/// <summary>
	/// Implement this.
	/// </summary>
	/// <returns>The list internal.</returns>
	protected abstract List<KeywordCommand> GetListInternal ();


	/// <summary>
	/// Determines whether this instance is provider active. If this method returns false, the provider
	/// does not return anything. This is useful for context aware keywords.
	/// </summary>
	/// <returns><c>true</c> if this instance is provider active; otherwise, <c>false</c>.</returns>
	protected abstract bool IsProviderActive ();

}