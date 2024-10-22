
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


    private bool handFound;
    void Start()
    {
        if (reciever == null) reciever = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Hand hand = leap.CurrentFrame.GetHand(Chirality.Left);
        if (hand != null)
        {
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
        else if (handFound)
        {
            if (reciever != null) reciever.SendMessage("DisableGesture");
            handFound = false;
        }
    }

    public SimpleHandPose LeapHandToSimpleHand(Hand hand)
    {
        SimpleHandPose simpleHandPose = new SimpleHandPose();
        Matrix4x4 handMatrix = Matrix4x4.TRS( 
            hand.WristPosition, 
            Quaternion.LookRotation(hand.Direction, -hand.PalmNormal),
            Vector3.one );

        PoseTargetThumb(hand.Thumb, simpleHandPose.thumb, handMatrix);
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
        target.splay = Mathf.Abs(localCarpalDir.x);
    }

    public void PoseTargetThumb(Finger finger, SimpleFinger target, Matrix4x4 localMatrix)
    {
        Vector3 localCarpalDir = Matrix4x4.Inverse(localMatrix) * finger.bones[1].Direction;
        target.bend = 1 - Vector3.Dot(finger.bones[1].Direction, finger.bones[2].Direction);
        target.curl = 1 - Vector3.Dot(finger.bones[2].Direction, finger.bones[3].Direction);
        target.splay = Mathf.Abs(localCarpalDir.x);
    }
    #endif
}

