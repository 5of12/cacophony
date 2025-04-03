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
        {ActionEventType.INPROGRESS, new Color32(248, 243, 160, 255) },
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

        private void UpdateIndicatorForActionEvent(ActionEventArgs arg)
        {
            foreach (GestureEventDebugRowUI row in eventRows)
            {
                // Special case progress, to get the progress value...
                if (row.eventType == ActionEventType.INPROGRESS)
                {
                    row.SetCompletion(arg.progress > 0.1 ? arg.progress : 0.1f, false);
                }
                else{
                    row.SetCompletion(arg.eventType == row.eventType ? 1f : 0.1f);
                }
            }
        }

        protected override void HandleStart(ActionEventArgs arg)
        {
            //StopCoroutine(resetRoutine);
            UpdateIndicatorForActionEvent(arg);
        }

        protected override void HandleHold(ActionEventArgs arg)
        {
            UpdateIndicatorForActionEvent(arg);
        }

        protected override void HandleEnd(ActionEventArgs arg)
        {
            UpdateIndicatorForActionEvent(arg);
        }

        protected override void HandleCancel(ActionEventArgs arg)
        {
            UpdateIndicatorForActionEvent(arg);
        }
    }
}
