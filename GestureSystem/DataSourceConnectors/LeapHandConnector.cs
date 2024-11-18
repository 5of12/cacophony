
#if ULTRALEAP
using Leap;
#endif

using UnityEngine;
using Cacophony;

public class LeapHandConnector : MonoBehaviour
{
    #if ULTRALEAP
    public LeapServiceProvider leap;
    public SimpleHandPose simplePose;
    public GameObject reciever;

    [Header ("Hand Management")]
    [Tooltip("Which hand to track and forward data for")]
    public Chirality handChirality;
    [Tooltip("The time in seconds to retain the last hand when hand is lost")]
    public float lostHandPersistence = 0.25f;
    public Collider interactionBounds;

    private Hand hand;
    private float timeLastSeen;

    private bool handFound;
    void Start()
    {
        if (reciever == null) reciever = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Hand newHand = null;
        if (leap.CurrentFrame.Hands.Count > 1)
        {
            newHand = leap.CurrentFrame.GetHand(handChirality);
        }
        else if (leap.CurrentFrame.Hands.Count == 1)
        {
            newHand = leap.CurrentFrame.Hands[0];
        }

        if (newHand != null && HandInInteractionBounds(newHand))
        {
            hand = newHand;
            timeLastSeen = Time.time;
            if (!handFound)
            {
                handFound = true;
                if (reciever != null) reciever.SendMessage("EnableGesture");
            }
            else
            {
                simplePose = LeapHandToSimpleHand(hand);
                if (reciever != null) reciever.SendMessage("SetHandPose", simplePose);
                if (reciever != null) reciever.SendMessage("SetHandPosition", hand.PalmPosition);
            }
        }
        else if (handFound && Time.time - timeLastSeen > lostHandPersistence)
        {
            if (reciever != null) reciever.SendMessage("DisableGesture");
            handFound = false;
        }
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

