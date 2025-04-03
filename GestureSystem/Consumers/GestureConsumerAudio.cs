using UnityEngine;

namespace Cacophony
{
[RequireComponent(typeof(AudioSource))]
public class GestureConsumerAudio : MonoBehaviour
{
    public HandGestureManager manager;
    public AudioSource audioSource;

    public float debounceTimer = 0.1f;
    public float pitchShiftRange = 0.25f;

    [Header("Audio Clips")]
    public AudioClip start;
    public AudioClip hold;
    public AudioClip end;
    public AudioClip cancel;

    private bool started = false;
    private float endTime = 0f;

    private void Awake()
    {
        if (manager == null)
        {
            manager = FindFirstObjectByType<HandGestureManager>();
            if (manager == null)
            {
                Debug.LogError("Could not find HandGestureManager for GestureConsumer: " + name);
            }
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void OnEnable()
    {
        if (manager != null)
        {
            Initialise();
        }
    }

    public void Initialise()
    {
        manager.actionProcessor.OnStart.AddListener(HandleStart);
        manager.actionProcessor.OnHold.AddListener(HandleHold);
        manager.actionProcessor.OnEnd.AddListener(HandleEnd);
        manager.actionProcessor.OnCancel.AddListener(HandleCancel); 
    }

    void OnDisable()
    {
        manager.actionProcessor.OnStart.RemoveListener(HandleStart);
        manager.actionProcessor.OnHold.RemoveListener(HandleHold);
        manager.actionProcessor.OnEnd.RemoveListener(HandleEnd);
        manager.actionProcessor.OnCancel.RemoveListener(HandleCancel);
    }

    protected void HandleStart(ActionEventArgs eventData)
    {
        if (!started && Time.time - endTime > debounceTimer)
        {
            if (start != null) audioSource.PlayOneShot(start);
            started = true;
        }
    }

    protected void HandleHold(ActionEventArgs eventData)
    {
        if (started)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(hold);
            }
            audioSource.pitch = 1 + (eventData.progress * pitchShiftRange);
        }
    }

    protected void HandleEnd(ActionEventArgs eventData)
    {
        audioSource.pitch = 1;
        if (end != null) audioSource.PlayOneShot(end);

        started = false;
        endTime = Time.time;
    }

    protected void HandleCancel()
    {
        audioSource.pitch = 1;
        if (cancel != null) audioSource.PlayOneShot(cancel);

        started = false;
        endTime = Time.time;
    }

}
}

