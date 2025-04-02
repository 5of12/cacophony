using UnityEngine;
using UnityEngine.Events;

namespace Cacophony
{
[RequireComponent(typeof(Animator))]
public class GestureConsumerAnimator : MonoBehaviour
{
    public HandGestureManager manager;
    public Animator animator;

    [Header("AnimationStates")]
    public string start = "Start";
    public string hold = "Hold";
    public string end = "End";
    public string cancel = "Cancel";

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

    protected void HandleStart(ActionEventArgs pos)
    {
        animator.ResetTrigger(end);
        animator.SetTrigger(start);
    }

    protected void HandleHold(ActionEventArgs eventData)
    {
        animator.ResetTrigger(start);
        animator.SetFloat(hold, eventData.progress);
    }

    protected void HandleEnd(ActionEventArgs pos)
    {
        animator.ResetTrigger(start);
        animator.SetTrigger(end);
    }

    protected void HandleCancel()
    {
        animator.SetTrigger(cancel);
        animator.ResetTrigger(start);
        animator.ResetTrigger(end);
    }

}
}
