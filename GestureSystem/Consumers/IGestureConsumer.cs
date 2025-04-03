using System;
using UnityEngine;

namespace Cacophony
{
    public class IGestureConsumer : MonoBehaviour
    {
        // The base class GestureConsumer, for example implmentations, see:
        // GestureConsumerUnityEvents / GestureConsumerAnimator/ GestureConsumerAudio
        public HandGestureManager manager;
        protected virtual void Awake()
        {
            if (manager == null)
            {
                manager = FindFirstObjectByType<HandGestureManager>();
                if (manager == null)
                {
                    Debug.LogError("Could not find HandGestureManager for GestureConsumer: " + name);
                }
            }
        }

        protected virtual void OnEnable()
        {
            if (manager != null)
            {
                Initialise();
            }
        }

        protected virtual void Initialise()
        {
            manager.actionProcessor.OnStart.AddListener(HandleStart);
            manager.actionProcessor.OnHold.AddListener(HandleHold);
            manager.actionProcessor.OnEnd.AddListener(HandleEnd);
            manager.actionProcessor.OnCancel.AddListener(HandleCancel); 
        }

        protected virtual void OnDisable()
        {
            manager.actionProcessor.OnStart.RemoveListener(HandleStart);
            manager.actionProcessor.OnHold.RemoveListener(HandleHold);
            manager.actionProcessor.OnEnd.RemoveListener(HandleEnd);
            manager.actionProcessor.OnCancel.RemoveListener(HandleCancel);
        }

        protected virtual void HandleStart(ActionEventArgs arg)
        {
            //StopCoroutine(resetRoutine);
            throw new NotImplementedException("IGestureConsumer must create a HandleStart method!");
        }

        protected virtual void HandleHold(ActionEventArgs arg)
        {
            throw new NotImplementedException("IGestureConsumer must create a HandleHold method!");
        }

        protected virtual void HandleEnd(ActionEventArgs arg)
        {
            throw new NotImplementedException("IGestureConsumer must create a HandleEnd method!");
        }

        protected virtual void HandleCancel(ActionEventArgs arg)
        {
            throw new NotImplementedException("IGestureConsumer must create a HandleCancel method!");
        }
    }
}
