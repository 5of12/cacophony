using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cacophony
{
    public enum ActionEventType { START, HOLD, PROGRESS, COMPLETE, CANCEL };
    public class GestureEventDebugRowUI : MonoBehaviour
    {
        public ActionEventType eventType;
        public Text stateText;
        public Image stateBarImage;
        bool isAnimating = false;

        public void ResetFill()
        {
            StartCoroutine(SmoothFillFunction(0.1f, 0.25f));
        }

        public void SetCompletion(float amount, bool animated = true)
        {
            if (stateBarImage.fillAmount == amount)
            {
                // Nothing to do, return...
                return;
            }
            // Else set the fill Amount...
            if (animated)
            {
                if (!isAnimating)
                {
                    StartCoroutine(SmoothFillFunction(amount, 0.25f));
                }
                
            }
            else
            {
                stateBarImage.fillAmount = amount;
            }
        }

        IEnumerator SmoothFillFunction(float fillAmount, float duration)
        {
            float time = 0;
            float startValue = stateBarImage.fillAmount;
            isAnimating = true;
            while (time < duration)
            {
                stateBarImage.fillAmount = Mathf.Lerp(startValue, fillAmount, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            stateBarImage.fillAmount = fillAmount;
            isAnimating = false;
        }
    }
}