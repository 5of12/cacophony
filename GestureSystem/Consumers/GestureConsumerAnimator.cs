using UnityEngine;
using UnityEngine.Events;

namespace Cacophony
{
[RequireComponent(typeof(Animator))]
public class GestureConsumerAnimator : MonoBehaviour
{
    public HandGestureManager manager;
    public Animator animator;

    public float debounceTimer = 0.1f;

    [Header("AnimationStates")]
    public string start = "Start";
    public string hold = "Hold";
    public string end = "End";
    public string cancel = "Cancel";

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

        if (animator == null)
        {
            animator = GetComponent<Animator>();
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
            animator.SetTrigger(start);
            started = true;
        }
        animator.ResetTrigger(end);
        animator.ResetTrigger(cancel);
        animator.SetFloat(hold, 0);
    }

    protected void HandleHold(ActionEventArgs eventData)
    {
        if (started)
        {
            animator.SetFloat(hold, eventData.progress);
        }
    }

    protected void HandleEnd(ActionEventArgs eventData)
    {
        animator.SetTrigger(end);
        started = false;
        endTime = Time.time;

        animator.ResetTrigger(start);
    }

    protected void HandleCancel()
    {
        animator.SetTrigger(cancel);
        started = false;
        endTime = Time.time;
        animator.ResetTrigger(start);
    }

}
}
