using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cacophony
{
    public class GestureActionEventIndicator : IGestureConsumer
    {
        public GameObject stateRow;
        public Transform rowParent;
        private Dictionary<ActionEventType, Color> stateColors =
            new Dictionary<ActionEventType, Color>() {
        {ActionEventType.START,  new Color32(166, 141, 212, 255)},
        {ActionEventType.HOLD, new Color32(139, 197, 232, 255)},
        {ActionEventType.PROGRESS, new Color32(248, 243, 160, 255)},
        {ActionEventType.COMPLETE, new Color32(156, 220, 203, 255)},
        {ActionEventType.CANCEL, new Color32(232, 177, 183, 255)}
        };

        private List<GestureEventDebugRowUI> eventRows = new();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            foreach (ActionEventType eventType in Enum.GetValues(typeof(ActionEventType)))
            {
                var newRow = Instantiate(stateRow, rowParent);
                GestureEventDebugRowUI rowUI = newRow.GetComponent<GestureEventDebugRowUI>();
                rowUI.stateText.text = eventType.ToString();
                rowUI.stateBarImage.color = stateColors[eventType];
                rowUI.eventType = eventType;
                eventRows.Add(rowUI);
                rowUI.ResetFill();
            }
        }

        private void UpdateIndicatorForActionEvent(ActionEventType eventType, float progress)
        {
            foreach (GestureEventDebugRowUI row in eventRows)
            {
                // Special case progress, to get the progress value...
                if (row.eventType == ActionEventType.PROGRESS)
                {
                    row.SetCompletion(progress > 0.1 ? progress : 0.1f, false);
                }
                else{
                    row.SetCompletion(eventType == row.eventType ? 1f : 0.1f);
                }
            }
        }

        protected override void HandleStart(ActionEventArgs arg)
        {
            //StopCoroutine(resetRoutine);
            UpdateIndicatorForActionEvent(ActionEventType.START, arg.progress);
        }

        protected override void HandleHold(ActionEventArgs arg)
        {
            UpdateIndicatorForActionEvent(ActionEventType.HOLD, arg.progress);
        }

        protected override void HandleEnd(ActionEventArgs arg)
        {
            UpdateIndicatorForActionEvent(ActionEventType.COMPLETE, arg.progress);
        }

        protected override void HandleCancel()
        {
            // Cancel doesn't carry ActionEventArgs, so create a dummy one...
            UpdateIndicatorForActionEvent(ActionEventType.CANCEL, 0f);
        }
    }
}
