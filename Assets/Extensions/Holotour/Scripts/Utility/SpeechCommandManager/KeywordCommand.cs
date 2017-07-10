using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A KeywordCommand represents the mapping of a keyword recognized by the speech recognition engine
/// and an associated action. Additionally, there can be bound a KeyCode for debugging.
/// </summary>
public class KeywordCommand {

	/// <summary>
	/// The action that will be performed after recognition of the keyword.
	/// </summary>
	/// <value>The action.</value>
	public Action action
	{ get; private set; }

	/// <summary>
	/// The keyword to recognize.
	/// </summary>
	/// <value>The keyword.</value>
	public string Keyword
	{ get; private set; }

	/// <summary>
	/// The HotKey KeyCode to listen on for debugging.
	/// </summary>
	/// <value>The hotkey.</value>
	public KeyCode Hotkey
	{ get; private set; }

    private Condition ConditionActive
    { get; set; }

    public bool IsActive
    { get { return ConditionActive.IsTrue; } }

    private Condition ConditionVisible
    { get; set; }

    public bool IsVisible
    { get { return ConditionVisible.IsTrue; } }

    /// <summary>
    /// Gets a value indicating whether this instance has a hotkey.
    /// </summary>
    /// <value><c>true</c> if this instance has hotkey; otherwise, <c>false</c>.</value>
    public bool HasHotkey
	{ get { return Hotkey != KeyCode.Escape; } }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="KeywordCommand"/> class.
    /// </summary>
    /// <param name="action">Action.</param>
    /// <param name="keyword">Keyword.</param>
    public KeywordCommand(Action action, string keyword) : this(action, keyword, KeyCode.Escape)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeywordCommand"/> class.
    /// </summary>
    /// <param name="action">Action.</param>
    /// <param name="keyword">Keyword.</param>
    public KeywordCommand(Action action, Condition condition, string keyword) : this(action, Condition.TRUE, keyword, KeyCode.Escape, Condition.TRUE)
    {
    }

	/// <summary>
	/// Initializes a new instance of the <see cref="KeywordCommand"/> class.
	/// </summary>
	/// <param name="action">Action.</param>
	/// <param name="keyword">Keyword.</param>
	/// <param name="hotkey">Hotkey.</param>
	public KeywordCommand(Action action, string keyword, KeyCode hotkey) : this (action, Condition.TRUE, keyword, hotkey, Condition.TRUE) {
    }

    public KeywordCommand(Action action, Condition conditionActive, string keyword, KeyCode hotkey) : this(action, conditionActive, keyword, hotkey, Condition.TRUE)
    {
    }

    /// <summary>
	/// Initializes a new instance of the <see cref="KeywordCommand"/> class.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="conditionActive"></param>
    /// <param name="keyword"></param>
    /// <param name="hotkey"></param>
    public KeywordCommand(Action action, Condition conditionActive, string keyword, KeyCode hotkey, Condition conditionVisible)
    {
        this.action = action;
        this.Keyword = keyword;
        this.Hotkey = hotkey;
        this.ConditionActive = conditionActive;
        this.ConditionVisible = conditionVisible;
    }
}