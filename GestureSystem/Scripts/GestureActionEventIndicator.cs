using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cacophony
{
    public class GestureConsumerEventIndicator : MonoBehaviour
    {
        public HandGestureDetector gestureDetector;
        public GameObject stateRow;
        public Transform rowParent;
        public Dictionary<GestureState, Color> stateColors =
            new Dictionary<GestureState, Color>() {
        {GestureState.IDLE,  new Color32(166, 141, 212, 255)},
        {GestureState.DETECTING, new Color32(139, 197, 232, 255)},
        {GestureState.READY, new Color32(156, 220, 203, 255)},
        {GestureState.HOLD, new Color32(248, 243, 160, 255)},
        {GestureState.RESET, new Color32(232, 177, 183, 255)}
        };
        private List<HandGestureDetectorRowUI> gestureRows = new();
        public GestureState activeState;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (gestureDetector == null)
            {
                Debug.LogWarning("HandGestureDetector was not set for: " + name +
                    ". Will find first instance but may not match the desired Gesture if multiple exist!");
                gestureDetector = FindFirstObjectByType<HandGestureDetector>();
                if (gestureDetector == null)
                {
                    Debug.LogError("Unable to get HandGestureDetector for: " + name);
                }
            }
            SetupUI();
        }

        private void SetupUI()
        {
            foreach (GestureState state in Enum.GetValues(typeof(GestureState)))
            {
                var newRow = Instantiate(stateRow, rowParent);
                HandGestureDetectorRowUI rowUI = newRow.GetComponent<HandGestureDetectorRowUI>();
                rowUI.stateText.text = state.ToString();
                rowUI.stateBarImage.color = stateColors[state];
                rowUI.state = state;
                gestureRows.Add(rowUI);
                rowUI.ResetFill();
            }
        }

        private void UpdateIndicatorFromGestureDetector()
        {
            activeState = gestureDetector.state;
            foreach (HandGestureDetectorRowUI row in gestureRows)
            {
                row.SetCompletion(activeState == row.state ? 1f : 0.1f);
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateIndicatorFromGestureDetector();
        }
    }
}
