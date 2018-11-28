using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// Keyword command manager. Its responsible for detecting spoken keywords and executing the corresponding actions.
/// </summary>
public class KeywordCommandManager : Singleton<KeywordCommandManager>
{
   
    private class KeywordCommandManagerCommands : IKeywordCommandProvider
    {
        private Action showAllCommandsAction;

        public KeywordCommandManagerCommands(Action showAllCommandsAction) {
            this.showAllCommandsAction = showAllCommandsAction;
        }

        public List<KeywordCommand> GetSpeechCommands()
        {
            return new List<KeywordCommand>()
            { new KeywordCommand(showAllCommandsAction, Condition.TRUE, "Show All Commands", KeyCode.Space, EXPERT_MODE) };
        }
    }

    public static Condition EXPERT_MODE = Condition.New(() => KeywordCommandManager.Instance.showAllCommands);

    private List<IKeywordCommandProvider> speechCommandProviders = new List<IKeywordCommandProvider> ();
	private KeywordRecognizer keywordRecognizer;
    private List<KeywordCommand> speechCommands = new List<KeywordCommand>();
    private bool showAllCommands = false;

    void Start()
    {
        AddKeywordCommandProvider(new KeywordCommandManagerCommands(() => showAllCommands = true)); 
    }

	void Update()
	{
        InvokeCommandForHotKey();
    }

    private void SetSpeechCommandContext()
	{
		if (keywordRecognizer != null) {
			keywordRecognizer.OnPhraseRecognized -= OnKeywordRecognized;
			keywordRecognizer.Stop ();
			keywordRecognizer.Dispose ();
			keywordRecognizer = null;
		}

		speechCommands.Clear ();

		List<string> keywords = new List<string> ();

		foreach (IKeywordCommandProvider provider in speechCommandProviders) {
			List<KeywordCommand> commands = provider.GetSpeechCommands ();
			if (commands != null) {
                speechCommands.AddRange(commands);

                foreach (KeywordCommand speechCommand in commands) {
					string lowercaseSpeechCommands = speechCommand.Keyword.ToLowerInvariant ();
					if (!keywords.Contains(lowercaseSpeechCommands)) {
						keywords.Add (speechCommand.Keyword);
					}
				}
			}
		}

		if (keywords.Count > 0)
		{
#if UNITY_EDITOR == false	
            keywordRecognizer = new KeywordRecognizer(keywords.ToArray());
			keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
			keywordRecognizer.Start();
#endif
		}
	}

	private void OnKeywordRecognized(PhraseRecognizedEventArgs args)
	{
		string keyword = args.text.ToLowerInvariant();
        foreach(KeywordCommand keywordCommand in speechCommands)
        {
            if (keywordCommand.IsActive && keywordCommand.Keyword.ToLowerInvariant().Equals(keyword)) {
                try
                {
                    Notify.Beep();
                    keywordCommand.action.Invoke();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    Notify.Show(e.Message);
                }
            }
        }
	}

    private void InvokeCommandForHotKey()
    {
        foreach (KeywordCommand keywordCommand in speechCommands)
        {
            if (keywordCommand.HasHotkey && Input.GetKeyDown(keywordCommand.Hotkey) && keywordCommand.IsActive)
            {
                try
                {
                    Notify.Beep();
                    keywordCommand.action.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.StackTrace.ToString());
                    Debug.Log(e.Message);
                    Notify.Show(e.Message);
                }
            }
        }
    }

    /// <summary>
    /// Adds a keyword command provider.
    /// </summary>
    /// <param name="provider">Provider.</param>
    public void AddKeywordCommandProvider(IKeywordCommandProvider provider)
	{
		speechCommandProviders.Add (provider);
		SetSpeechCommandContext ();
	}

	/// <summary>
	/// Gets the active keyword strings.
	/// </summary>
	/// <returns>The active keyword strings.</returns>
	public List<KeywordCommand> GetKeywordCommands()
	{
		return new List<KeywordCommand> (speechCommands);
	}
}