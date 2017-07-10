using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeButton : MonoBehaviour, IGazeable, IKeywordCommandProvider
{
    [Tooltip("Time it takes for the button to be activated in seconds.")]
    [SerializeField] private float fillUpSpeed = 2;
    [SerializeField] private float fillDownSpeed = 1;
    [SerializeField] private AudioSource appStartReverseSound;
    private float audioLength = 3.34f;

	[SerializeField] private Image slider; 


    private float timeGazeEnter;
    private bool isInGaze;
    private bool hasBeenTriggered;

    void Start()
    {
		if (slider == null)
        {
            throw new UnassignedReferenceException("App Start UI slider and bg needs to be assigned in editor!");
        }
		slider.fillAmount = 0;
		KeywordCommandManager.Instance.AddKeywordCommandProvider (this);
    }

   

    private void Update()
    {    
        if (isInGaze == true)
        {
            slider.fillAmount +=  0.1f * Time.deltaTime * fillUpSpeed;
			if (slider.fillAmount >= 1.0f)
            {
				slider.fillAmount = 1;
				isInGaze = false;
                TriggerWelcome();
            }
        }
        else if ((isInGaze == false) && (hasBeenTriggered == false))
        {
            slider.fillAmount -= 0.1f * Time.deltaTime * fillDownSpeed;
        } 
    }

     void IGazeable.OnGazeEnter(RaycastHit hitinfo)
    {
        isInGaze = true;
        timeGazeEnter = Time.time;
        appStartReverseSound.time = audioLength * slider.fillAmount;
        appStartReverseSound.Play();
    }

    void IGazeable.OnGazeExit(RaycastHit hitinfo)
    {
        isInGaze = false;
        if (hasBeenTriggered == false)
        {
            appStartReverseSound.Stop();
        }
        
    }

    void IGazeable.OnGazeStay(RaycastHit hitinfo)
    {
    }

    private void TriggerWelcome()
    {
        if (!hasBeenTriggered)
        {
          hasBeenTriggered = true;  
          AvatarManager.Instance.OnAppStartTriggered();
        }
    }

    public List<KeywordCommand> GetSpeechCommands()
    {
        return new List<KeywordCommand> {
            new KeywordCommand(() => { TriggerWelcome(); }, "Hello Paula")
        };
    }

}
