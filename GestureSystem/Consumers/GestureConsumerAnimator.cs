using UnityEngine;

namespace Cacophony
{
    [RequireComponent(typeof(Animator))]
    public class GestureConsumerAnimator : IGestureConsumer
    {
        public Animator animator;
        public float debounceTimer = 0.1f;

        [Header("AnimationStates")]
        public string start = "Start";
        public string hold = "Hold";
        public string end = "End";
        public string cancel = "Cancel";

        private bool started = false;
        private float endTime = 0f;

        protected override void Awake()
        {
            base.Awake();

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        protected override void HandleStart(ActionEventArgs eventData)
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

        protected override void HandleHold(ActionEventArgs eventData)
        {
            if (started)
            {
                animator.SetFloat(hold, eventData.progress);
            }
        }

        protected override void HandleEnd(ActionEventArgs eventData)
        {
            animator.SetTrigger(end);
            started = false;
            endTime = Time.time;

            animator.ResetTrigger(start);
        }

        protected override void HandleCancel(ActionEventArgs eventData)
        {
            animator.SetTrigger(cancel);
            started = false;
            endTime = Time.time;
            animator.ResetTrigger(start);
        }
    }
}
