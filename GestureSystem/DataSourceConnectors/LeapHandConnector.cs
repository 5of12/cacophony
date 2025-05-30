
#if ULTRALEAP
using Leap;
#endif

using UnityEngine;
using Cacophony;
using UnityEngine.Events;

public class LeapHandConnector : IHandDataConnector
{
    #if ULTRALEAP
    public LeapServiceProvider leap;
    public SimpleHandPose simplePose;

    [Header ("Hand Management")]
    [Tooltip("Which hand to track and forward data for")]
    public Chirality handChirality;

    [Tooltip("The active hand chirality")]
    public Chirality activeChirality;
    public bool allowAnyHand = true;

    private Hand hand;
    private float timeLastSeen;
    public UnityEvent<Chirality> OnHandChiralityChanged;

    private bool handFound;

    private void Awake()
    {
        // Check that we have a LeapService Provider available...
        leap = FindFirstObjectByType<LeapServiceProvider>();
        if (leap == null) {
            Debug.LogError("Unable to obtain a LeapService Provider! Check one exists in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Hand newHand = GetActiveHand();
        if (newHand != null && HandInInteractionBounds(newHand))
        {
            Chirality newChirality = newHand.GetChirality();
            if (activeChirality != newChirality)
            {
                OnHandChiralityChanged.Invoke(newChirality);
            }
            activeChirality = newChirality;
            hand = newHand;
            timeLastSeen = Time.time;
            if (!handFound)
            {
                handFound = true;
                OnHandFound.Invoke();
            }
            else
            {
                simplePose = LeapHandToSimpleHand(hand);
                OnNewData.Invoke(new HandDataEventArgs { handPose = simplePose, handPosition = hand.PalmPosition });
            }
        }
        else if (handFound && Time.time - timeLastSeen > lostHandPersistence)
        {
            OnNoHandPresentAfterTimeout.Invoke();
            handFound = false;
        }
    }

    private Hand GetActiveHand()
    {
        Hand hand = null;
        if(allowAnyHand)
        {
            if (leap.CurrentFrame.Hands.Count > 1)
            {
                // Return the hand closest to object origin
                float a = Vector3.Distance(leap.CurrentFrame.Hands[0].PalmPosition, transform.position);
                float b = Vector3.Distance(leap.CurrentFrame.Hands[1].PalmPosition, transform.position);
                hand = a < b ? leap.CurrentFrame.Hands[0] : leap.CurrentFrame.Hands[1];
            }
            else if (leap.CurrentFrame.Hands.Count > 0)
            {
                hand = leap.CurrentFrame.Hands[0];
            }
        }
        else
        {
            hand = leap.CurrentFrame.GetHand(handChirality);
        }
        
        return hand;
    }

    public bool HandInInteractionBounds(Hand hand)
    {
        return interactionBounds == null ? true : interactionBounds.bounds.Contains(hand.PalmPosition);
    }

    public SimpleHandPose LeapHandToSimpleHand(Hand hand)
    {
        SimpleHandPose simpleHandPose = new SimpleHandPose();
        Matrix4x4 handMatrix = Matrix4x4.TRS( 
            hand.WristPosition, 
            Quaternion.LookRotation(hand.Direction, -hand.PalmNormal),
            Vector3.one );

        PoseTargetThumb(hand.Thumb, hand.Index, simpleHandPose.thumb);
        PoseTargetFinger(hand.Index, simpleHandPose.index, handMatrix);
        PoseTargetFinger(hand.Middle, simpleHandPose.middle, handMatrix);
        PoseTargetFinger(hand.Ring, simpleHandPose.ring, handMatrix);
        PoseTargetFinger(hand.Pinky, simpleHandPose.pinky, handMatrix);

        simpleHandPose.palmDirection = hand.Direction;
        simpleHandPose.palmNormal = hand.PalmNormal;

        return simpleHandPose;
    }

    public void PoseTargetFinger(Finger finger, SimpleFinger target, Matrix4x4 localMatrix)
    {
        Vector3 localCarpalDir = Matrix4x4.Inverse(localMatrix) * finger.bones[1].Direction;
        target.bend = 1 - Vector3.Dot(finger.bones[0].Direction, finger.bones[1].Direction);
        target.curl = 1 - Vector3.Dot(finger.bones[1].Direction, finger.bones[2].Direction);
        target.splay = localCarpalDir.x / 2;
    }

    public void PoseTargetThumb(Finger thumb, Finger index, SimpleFinger target)
    {
        target.bend = 1 - Vector3.Dot(thumb.bones[1].Direction, thumb.bones[2].Direction);
        target.curl = 1 - Vector3.Dot(thumb.bones[2].Direction, thumb.bones[3].Direction);
        target.splay = 1 - Vector3.Dot(thumb.bones[1].Direction, index.bones[0].Direction);
    }
    #endif
}

