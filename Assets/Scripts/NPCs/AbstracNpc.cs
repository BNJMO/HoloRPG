using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstracNpc : PoiAnchor, INpc, IKeywordCommandProvider {
    public IQuest[] Quests { get { return quests; } }
    public IQuest CurrentQuest { get { return currentQuest; } }

    private AudioSource myAudioSource;
    private Animator myAnimator;
    private Quaternion initialRotation;
    private float turnSpeed = 5;

    [SerializeField] private AbstractQuest[] quests;
    private AbstractQuest currentQuest;
	private int questIndex = 0;
    private bool noMoreQuest;

    private QuestUI myQuestUI;
    private HintUI myHintUI;

    private bool isTalking = false;
    private bool isWaitingForAnswer = false;
    private bool playerWasInRange = false;
    


    protected override void Awake()
    {
        base.Awake();

        var mixer = Resources.Load<UnityEngine.Audio.AudioMixer>(SoundAndEffectManager.MIXER_NAME);
        var group = mixer.FindMatchingGroups("NPCs")[0];
        myAudioSource = Utils.AddAudioListener(gameObject, true, 1.0f, false, group);

        myAnimator = GetComponentInChildren<Animator>();
        initialRotation = transform.rotation;

        // init quests
        if (quests.Length == 0)
        {
            Debug.LogError("NPC has no quests!");
        }
        currentQuest = quests[0];
        questIndex = 0;
        foreach (AbstractQuest quest in quests)
        {
            quest.Populate(this);
        }
        // init quest and hint UI
        myQuestUI = Instantiate(Resources.Load<QuestUI>("QuestUI"), transform);
        myQuestUI.transform.localPosition = new Vector3 (0, 2.5f, 0);
        if (currentQuest.IsAvailable == true)
        {
            myQuestUI.SetQuestAvailable();
        }
        myHintUI = Instantiate(Resources.Load<HintUI>("HintUI"), transform);
        myHintUI.transform.localPosition = new Vector3(0, 1.4f, 1f);
    }

    protected override void Start()
    {
        base.Start();

        KeywordCommandManager.Instance.AddKeywordCommandProvider(this);
    }

    
    protected override void Update()
    {
        base.Update();

        if (GameManger.Instance.IsInUserMode == true)
        {
            if (PlayerInRange == true)
            {
                TurnTo(CameraHelper.Stats.camPos);
                if (playerWasInRange == false)
                {
                    myHintUI.SetHintText("Say <b>HELLO</b>");
                    playerWasInRange = true;
                }
            }
            else
            {
                TurnTo(initialRotation);
                myHintUI.HideHintText();
                playerWasInRange = false;
            }
        }
    }

    

    private void NextQuest()
    {
        // drop reward
        if (currentQuest.Reward != null)
        {
            InventoryManager.Instance.DropItem(currentQuest.Reward.ItemID, CameraHelper.Stats.camPos);
        }
        
        // Update QuestManager on quest complete
        QuestManager.Instance.OnQuestCompleted(currentQuest);

        // get next quest
        questIndex++;
        if (questIndex >= quests.Length)
        {
            noMoreQuest = true;
            myQuestUI.SetNoQuest();
        }
        else // still more quests available
        {
            currentQuest = quests[questIndex];
            if (currentQuest.IsAvailable == true)
            {
                myQuestUI.SetQuestAvailable();
            }
        }
    }

    private void TurnTo(Vector3 position)
    {
        Vector3 finalDestination = GetRelativePosition(position);
        Quaternion finalRotation = Quaternion.LookRotation((finalDestination - transform.position).normalized);
        AnchorRotation = Quaternion.Lerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }

    private void TurnTo(Quaternion rotation)
    {
        AnchorRotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    public List<KeywordCommand> GetSpeechCommands()
    {
        List<KeywordCommand> result = new List<KeywordCommand>();
        Condition condIsUserMode    = Condition.New(() => GameManger.Instance.IsInUserMode == true);
        Condition condPlayerInRange = Condition.New(() => PlayerInRange == true);
        Condition condIsNotTalking  = Condition.New(() => isTalking == false);
        Condition condYesNoQuestion = Condition.New(() => isWaitingForAnswer == true); // REPLACE WITH == !!!! (only for testing purpose)
        // TODO : implement rest
   //     result.Add(new KeywordCommand(() => { OnHi(); }, condPlayerInRange.And(condIsNotTalking), "Hi", KeyCode.M));
        result.Add(new KeywordCommand(() => { OnHi(); }, condPlayerInRange.And(condIsNotTalking), "Hello", KeyCode.M));
        result.Add(new KeywordCommand(() => { OnYes(); }, condPlayerInRange.And(condYesNoQuestion), "Yes", KeyCode.N));
        result.Add(new KeywordCommand(() => { OnYes(); }, condPlayerInRange.And(condYesNoQuestion), "Ok"            ));
        result.Add(new KeywordCommand(() => { OnNo(); }, condPlayerInRange.And(condYesNoQuestion), "No", KeyCode.B));

        return result;
    }

    private void OnHi()
    {
        // for testing
        Debug.Log ("hi");
        myHintUI.HideHintText();

        Say(currentQuest.welcomeClip, () => 
        {
            if (noMoreQuest == false)
            {
                // completed
                Debug.Log (currentQuest.QuestID + " is completed : " + currentQuest.IsCompleted);
                if (currentQuest.IsCompleted == true)
                {
                    Debug.Log (currentQuest.ShortDescription + " : completed quest");
                    Say(currentQuest.completedClip, () =>
                    {
                        NextQuest();
                    });
                }
                // not completed but taken
                else if (QuestManager.Instance.HasTakenQuest(currentQuest) == true)
                {
                    Debug.Log (currentQuest.ShortDescription + " : you have a quest pending");
                    Say(currentQuest.pendingClip);
                }
                else // not completed nor taken yet
                {
                    if (currentQuest.IsAvailable == true)
                    {
                        Debug.Log (currentQuest.ShortDescription + " : Your quest is to save the world");
                        Say(currentQuest.mainQuestClip, () => 
                        {
                            isWaitingForAnswer = true;
                            myHintUI.SetHintText("say <b>YES</b> or <b>NO</b>");
                        });
                    }
                    else
                    {
                        Debug.Log (currentQuest.ShortDescription + " : Current quest is still not available yet");
                    }
                }
            }
        });
    }

    private void OnYes()
    {
        StartCoroutine(OnYesRoutine());
    }
    IEnumerator OnYesRoutine()
    {
        Debug.Log ("You have said yes");
        Say(currentQuest.confirmationClip);
        QuestManager.Instance.AcceptQuest(currentQuest);
        isWaitingForAnswer = false;
        // update UI
        myQuestUI.SetQuestPending();
        myHintUI.HideHintText();
        // wait a frame for QuestManager to instantiate quest
        yield return new WaitForEndOfFrame();
        currentQuest = (AbstractQuest) QuestManager.Instance.GetQuestInstance(currentQuest.QuestID);
        
    }

    private void OnNo()
    {
        Debug.Log ("You have said no");
        isWaitingForAnswer = false;
    }

    // private methods for sequential speaking
    private void Say(AudioClip clip)
    {
        StartCoroutine(SayRoutine(clip));
    }

    private void Say(AudioClip clip, Action callback)
    {
        StartCoroutine(SayRoutine(clip, callback));
    }
    IEnumerator SayRoutine(AudioClip clip)
    {
        if (clip != null)
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
            isTalking = true;
            myAnimator.SetBool("isTalking", true);
            yield return new WaitForSeconds(clip.length + 0.5f);
            isTalking = false;
            myAnimator.SetBool("isTalking", false);
        }
    }
    IEnumerator SayRoutine(AudioClip clip, Action callback)
    {
        if (clip != null)
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
            isTalking = true;
            myAnimator.SetBool("isTalking", true);
            yield return new WaitForSeconds(clip.length + 0.5f);
            isTalking = false;  
            myAnimator.SetBool("isTalking", false);
        }
        callback.Invoke();
    }
}
