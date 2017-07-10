using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVBoundScaler : MonoBehaviour, IKeywordCommandProvider {


	[SerializeField] private RectTransform[] bounds; // clockwise 0 : left
    [SerializeField] private float offset = 0.1f;

    void Start()
    {
        KeywordCommandManager.Instance.AddKeywordCommandProvider (this);
    }

    private void MoveLeft(int boundNr)
    {
        bounds[boundNr].position -= Vector3.right * offset;
    }

    private void MoveRight(int boundNr)
    {
        bounds[boundNr].position += Vector3.right * offset;
    }

    private void MoveUp(int boundNr)
    {
        bounds[boundNr].position += Vector3.up * offset;
    }

    private void MoveDown(int boundNr)
    {
        bounds[boundNr].position -= Vector3.up * offset;
    }

    public List<KeywordCommand> GetSpeechCommands()
    {
        List<KeywordCommand> result = new List<KeywordCommand>();
        // left
        result.Add(new KeywordCommand(() => { MoveLeft(0); }, "left left", KeyCode.H));
        result.Add(new KeywordCommand(() => { MoveRight(0); }, "left right", KeyCode.K));
        // up
        result.Add(new KeywordCommand(() => { MoveUp(1); }, "top up", KeyCode.U));
        result.Add(new KeywordCommand(() => { MoveDown(1); }, "top down", KeyCode.J));
        // right
        result.Add(new KeywordCommand(() => { MoveLeft(2); }, "right left"));
        result.Add(new KeywordCommand(() => { MoveRight(2); }, "right right"));
        // down
        result.Add(new KeywordCommand(() => { MoveUp(3); }, "buttom up"));
        result.Add(new KeywordCommand(() => { MoveDown(3); }, "buttom down"));

        return result;
    }
}
