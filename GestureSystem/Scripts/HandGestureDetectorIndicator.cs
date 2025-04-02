using UnityEngine;
using UnityEngine.UI;

namespace Cacophony
{
    public class HandGestureDetectorStateIndicator : MonoBehaviour
    {
        public HandGestureDetector gestureDetector;
        public Text stateText;
        public Color idleColor = Color.grey;
        public Color detectingColor = Color.magenta;
        public Color readyColor = Color.green;
        public Color holdColor = Color.cyan;
        public Color cancelColor = Color.red;

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
        }

        private void SetIndicatorFromGestureDetector()
        {
            stateText.text = gestureDetector.state.ToString();
            switch (gestureDetector.state)
            {
                case GestureState.IDLE:
                    stateText.color = idleColor;
                    break;
                case GestureState.DETECTING:
                    stateText.color = detectingColor;
                    break;
                case GestureState.READY:
                    stateText.color = readyColor;
                    break;
                case GestureState.HOLD:
                    stateText.color = holdColor;
                    break;
                case GestureState.RESET:
                    stateText.color = cancelColor;
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            SetIndicatorFromGestureDetector();
        }
    }
}
