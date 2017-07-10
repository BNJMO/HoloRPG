using UnityEngine;

public class AvatarAgent : MonoBehaviour
{
    private Billboard myBillBoard;
    private SimpleTagalong myTagAlong;
    private AudioSource myAudioSource;
    public Animator animator;

    [Header ("Tour Start/End")]
    [SerializeField] Transform tourStartPosition;
    [SerializeField] ParticleSystem[] appearParticleEffects;

    [Header("Rocket Sounds")]
    [SerializeField] AudioSource idle_rocketAudioSource;
    [SerializeField] AudioSource moving_rocketAudioSource;
    [Range (0.01f, 10)] [SerializeField] float soundTransitionSpeed = 1;
    [SerializeField] AudioSource appearanceSound;

    [Header("Trail Renderer")]
    [SerializeField] TrailRenderer[] trailRenderers;
    [SerializeField] float trailFadingSpeed = 1;

    [Header("Conversation Hint")]
    [SerializeField] Transform conversationHint;

    private float trailInitialTime;

    private bool hasAppeared = false;
    private bool wasMoving = false; // in last frame to check for state change
    

    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
        myBillBoard = GetComponent<Billboard>();
        myTagAlong = GetComponent<SimpleTagalong>();
        myAudioSource = GetComponent<AudioSource>();

        animator = transform.GetChild(0).GetComponent<Animator>();

        if (trailRenderers.Length != 0)
            trailInitialTime = trailRenderers[0].time;

        if ((myBillBoard == null) || (myTagAlong == null))
            Debug.LogError("Fatal error! Avatar agent needs to have a Billboard and SimpleTagAlong script attached to it");
    }

    void Start ()
    {
        AvatarManager.Instance.OnTourStarted += Appear;
        AvatarManager.Instance.OnTourEnded += Disappear; 
    }

   void Update ()
    {
        UpdateSoundVolumes();
        UpdateTrailRenderer();
        wasMoving = AvatarManager.Instance.IsMoving;
    }

	/// <summary>
    /// Make the Avatar Agent look at a specific point in the world.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="isYAxis">wether to freeze X rotation (true) or keep it free (false)</param>
    public void LookeAt(Transform destination, bool isYAxis)
    {
        myBillBoard.targetTransform = destination;
        if (isYAxis)
        {
            myBillBoard.pivotAxis = Billboard.PivotAxis.Y;
        }
        else
        {
            myBillBoard.pivotAxis = Billboard.PivotAxis.Free;
        }
        
    }

    public void PlayClip(AudioClip newClip)
    {
        myAudioSource.Stop();
        myAudioSource.clip = newClip;
        myAudioSource.Play();
    }
	
    public void DisableTagAlong()
    {
        myTagAlong.enabled = false;
        Interpolator inter = GetComponent<Interpolator>();
        if (inter != null)
            inter.enabled = false;
    }

    public void EnableTagAlong()
    {
        //myTagAlong.enabled = true;
    }

    public void Appear()
    {
        transform.position = tourStartPosition.position;
        transform.rotation = tourStartPosition.rotation;
        AnimateThis.With(this).CancelAll();
        AnimateThis.With(this).Transformate().FromScale(0).ToScale(initialScale).Ease(AnimateThis.EaseOutQuintic).Duration(2).Delay(1).Start();
        foreach (TrailRenderer trailRenderer in trailRenderers)
        { 
           // trailRenderer.enabled = true;
            trailRenderer.Clear();
        }
        StartCoroutine(PlayParticlesWithDelay(1));
        hasAppeared = true;
    }
    System.Collections.IEnumerator PlayParticlesWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        appearanceSound.Play();
        foreach (ParticleSystem p in appearParticleEffects)
        {
            p.Emit(50);
        }
       
    }

    public void Disappear()
    {
        AnimateThis.With(this).CancelAll();
        AnimateThis.With(this).Transformate().FromScale(initialScale).ToScale(0).Ease(AnimateThis.EaseInOutSmooth).Duration(2).Delay(1).Start();
        /*foreach (TrailRenderer trailRenderer in trailRenderers)
        {
            trailRenderer.enabled = false;
        }*/
        StartCoroutine(PlayParticlesWithDelay(0.5f));
        hasAppeared = false;
    }

    private void UpdateSoundVolumes()
    {
         
        if (hasAppeared == true)
        {
            // check for state change
            if (AvatarManager.Instance.IsMoving == true)
            {
                if (moving_rocketAudioSource.volume < 1)
                {
                    moving_rocketAudioSource.volume += 0.01f * soundTransitionSpeed;
                }
                if (idle_rocketAudioSource.volume > 0)
                {
                    idle_rocketAudioSource.volume -= 0.01f * soundTransitionSpeed;
                }
            }
            else if (AvatarManager.Instance.IsMoving == false) // isIdle
            {
                if (idle_rocketAudioSource.volume < 1)
                {
                    idle_rocketAudioSource.volume += 0.01f * soundTransitionSpeed;
                }
                if (moving_rocketAudioSource.volume > 0)
                {
                    moving_rocketAudioSource.volume -= 0.01f * soundTransitionSpeed;
                }
    
            }
        }
        else if (hasAppeared == false)
        {
             if (idle_rocketAudioSource.volume > 0)
                {
                    idle_rocketAudioSource.volume -= 0.01f * soundTransitionSpeed;
                }
                if (moving_rocketAudioSource.volume > 0)
                {
                    moving_rocketAudioSource.volume -= 0.01f * soundTransitionSpeed;
                }
        }
    }

    private void UpdateTrailRenderer()
    {
        if (hasAppeared == true)
        {
            // check for state change
            if (AvatarManager.Instance.IsMoving == true)
            {
                if (wasMoving == false)
                {
                    foreach (TrailRenderer trailRenderer in trailRenderers)
                    {
                        trailRenderer.minVertexDistance = 0.1f;
                    }
                } 
            }
            else if (AvatarManager.Instance.IsMoving == false) // isIdle
            {
                if (wasMoving == true)
                {
                    foreach (TrailRenderer trailRenderer in trailRenderers)
                    {
                        trailRenderer.minVertexDistance = 50f;
                    }
                }
                
                
            }

            if (!AvatarManager.Instance.IsMoving)
            {
                foreach (TrailRenderer trail in trailRenderers) // fade out trail
                {
                    if (trail.time > 0)
                    {
                        trail.time -= 0.1f * Time.deltaTime * trailFadingSpeed;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                foreach (TrailRenderer trail in trailRenderers) // fade in trail
                {
                    if (trail.time < trailInitialTime)
                    {
                        trail.time += 0.1f * Time.deltaTime * trailFadingSpeed;
                    }
                    else
                    {
                        break;
                    }  
                }
            }
        }
    }

    public void ShowConversationHint(bool visible)
    {
        if (conversationHint != null) {
            if (visible)
            {
                AnimateThis.With(conversationHint).CancelAll().Transformate().ToScale(1).Duration(1f).Ease(AnimateThis.EaseOutQuintic).Start();
            } else
            {
                AnimateThis.With(conversationHint).CancelAll().Transformate().ToScale(0).Duration(0.25f).Start();
            }
        }
    }
   
}
