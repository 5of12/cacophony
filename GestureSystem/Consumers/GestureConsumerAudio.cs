using UnityEngine;

namespace Cacophony
{
    [RequireComponent(typeof(AudioSource))]
    public class GestureConsumerAudio : IGestureConsumer
    {
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

        protected override void Awake()
        {
            base.Awake();
            if (audioSource == null){
                audioSource = GetComponent<AudioSource>();
            }
        }

        protected override void HandleStart(ActionEventArgs eventData)
        {
            if (!started && Time.time - endTime > debounceTimer)
            {
                if (start != null) audioSource.PlayOneShot(start);
                started = true;
            }
        }

        protected override void HandleHold(ActionEventArgs eventData)
        {
            if (started)
            {
                if (!audioSource.isPlaying && hold != null)
                {
                    audioSource.PlayOneShot(hold);
                }
                audioSource.pitch = 1 + (eventData.progress * pitchShiftRange);
            }
        }

        protected override void HandleEnd(ActionEventArgs eventData)
        {
            audioSource.pitch = 1;
            if (end != null) audioSource.PlayOneShot(end);

            started = false;
            endTime = Time.time;
        }

        protected override void HandleCancel(ActionEventArgs pos)
        {
            audioSource.pitch = 1;
            if (cancel != null) audioSource.PlayOneShot(cancel);

            started = false;
            endTime = Time.time;
        }
    }
}
