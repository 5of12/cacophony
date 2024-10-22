using Cacophony;
using UnityEngine;

[CreateAssetMenu(menuName = "Cacophony/HandGesture", fileName = "Gesture")]
public class HandGestureDefinition : IGestureDefinition<DetectableHandPose, SimpleHandPose>
{
    [Range(0,1)]
    public float confidenceThreshold = 0.95f;
    
    public override float Evaluate(SimpleHandPose input)
    {
        float positives = 0;
        float negatives = 0;

        foreach(var pose in positivePoses)
        {
            positives = Mathf.Max(positives, pose.Evaluate(input));
        }
        foreach(var pose in negativePoses)
        {
            negatives = Mathf.Max(negatives, pose.Evaluate(input));
        }

        return Mathf.Clamp01(positives - negatives);
    }
}
