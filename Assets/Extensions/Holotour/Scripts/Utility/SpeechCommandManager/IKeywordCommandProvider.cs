using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to implement for providing new KeywordCommands,
/// </summary>
public interface IKeywordCommandProvider {
	/// <summary>
	/// Returns a list of KeywordCommands.
	/// </summary>
	/// <returns>The speech commands.</returns>
	List<KeywordCommand> GetSpeechCommands();
}